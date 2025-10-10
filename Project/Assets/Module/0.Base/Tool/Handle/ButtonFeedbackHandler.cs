using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFeedbackHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    Button button;

    bool isInteractable = true;
    bool isOnTutorial = false;

    public bool noScale;

    private Coroutine reenableCoroutine;
    
    //音效类型
    public enum UIButtonSFXType
    {
        None,
        Generic,    //通用
        OpenUI,     //打开UI
        CloseUI,    //关闭UI
    }

    // 震动类型,这里只给开和不开的选项，
    // 后续震动在UtilityHaptic中处理
    // 避免重复代码选项太多不好统一维护
    public enum UIButtonHapticType
    {
        Off,
        On,
    }

    //节流模式，避免重复点击
    public enum UIButtonThrottleType
    {
        Timer, // 通过时间截流
        TillEnable, // 永久截流，直到下次Enable这个按钮
        None = 99, // 不截流
    }

    //音效类型
    public UIButtonSFXType sfxType;

    //震动类型
    public UIButtonHapticType hapticType;

    //节流模式      
    public UIButtonThrottleType throttleType;

    //截流时长
    float throttle = 0.35f;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void OnClick()
    {
        if(throttleType == UIButtonThrottleType.None) return;

        isInteractable = false;
        //OnPointerUp(null);
        button.interactable = isInteractable;

        //两种节流模式
        if (throttleType == UIButtonThrottleType.Timer)
        {
            Invoke(nameof(Reenable), throttle);
        }
    }

    //按钮触发间隔，避免段时间多次触发
    
    void Reenable()
    {
        isInteractable = true;
        button.interactable = true;
    }

    //启用
    void OnEnable()
    {
        button.interactable = true;
        isInteractable = true;
        button.onClick.AddListener(OnClick);
    }

    //销毁
    void OnDestroy()
    {
        Reset();
    }

    //禁用
    void OnDisable()
    {
        Reset();
    }

    void Reset()
    {
        transform.DOComplete();
        transform.DOKill();
        transform.localScale = Vector2.one;
        if(button != null)
        {
            button.onClick.RemoveListener(OnClick);
        }
        
        //停止重新启用按钮的协程
        if(reenableCoroutine != null)
        {
            StopCoroutine(reenableCoroutine);
            reenableCoroutine = null;
        }
    }

    //抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isInteractable) return;

        if (!noScale)
        {
            transform.DOKill();
            transform.DOScale(Vector2.one, 0.1f).SetUpdate(true);
        }

        
        switch (sfxType)
        {
            case UIButtonSFXType.Generic:
                AudioManager.Instance.PlaySFX("sfx_ui_shared_btn_generic");
                break;
            case UIButtonSFXType.OpenUI:
                AudioManager.Instance.PlaySFX("sfx_ui_shared_btn_page_open");
                break;
            case UIButtonSFXType.CloseUI:
                AudioManager.Instance.PlaySFX("sfx_ui_shared_btn_page_close");
                break;
        }
    }

    //按下
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isInteractable) return;

        if (!noScale)
        {
            transform.DOKill();
            transform.localScale = Vector2.one;
            transform.DOScale(new Vector2(0.92f, 0.92f), 0.2f).SetUpdate(true);
        }

        switch (hapticType)
        {
            case UIButtonHapticType.On:
                UtilityHaptic.OnHapticButton();
                break;
            default:
                break;
        }

        if (isOnTutorial)
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("tip/tip_tutorial_block"));
        }
    }

    public void SetOnTutorial(bool isTutorial)
    {
        isOnTutorial = isTutorial;
        GetComponent<Button>().interactable = !isTutorial;
    }
}

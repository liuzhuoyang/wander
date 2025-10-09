using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

public class UILanding : Singleton<UILanding>
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textProgress;

    float progression = 0f;
    float progressNum = 0;

    public TextMeshProUGUI textVersion;
    public GameObject objDebugHint;

    public Image imgLogo;
    public Sprite spriteLogoDefault;
    public Sprite spriteLogoZH;
    public Sprite spriteLogoJA;

    public RectTransform rectBar;
    [SerializeField] TextMeshProUGUI textHint;
    [SerializeField] TextMeshProUGUI textUserID;
    // [SerializeField] GameObject objSupportButton;
    public void Start()
    {
        // objSupportButton.SetActive(false);
        textHint.gameObject.SetActive(false);

        EventManager.StartListening<LandingUIArgs>(EventNameLanding.EVENT_LOADING_REFRESH_UI, OnLoadingProgressUI);
        EventManager.StartListening<LandingUIArgs>(EventNameLanding.EVENT_LOADING_CLOSE_UI, OnCloseUI);

        textVersion.text = Application.version; //+ "\n" + TGameSDK.OPStrategyVersion();
        objDebugHint.SetActive(GameConfig.main.debugTool == DebugTool.On);

        string lang = UtilityLocalization.GetSystemLanguageCode();

        switch (lang)
        {
            case ConstantLocKey.LANGUAGE_ZHS:
                imgLogo.sprite = spriteLogoZH;
                break;
            case ConstantLocKey.LANGUAGE_ZHT:
                imgLogo.sprite = spriteLogoZH;
                break;
            case ConstantLocKey.LANGUAGE_JA:
                imgLogo.sprite = spriteLogoJA;
                break;
            default:
                imgLogo.sprite = spriteLogoDefault;
                break;
        }

        // Invoke("OnShowSupportButton", 10f);
    }

    // void OnShowSupportButton()
    // {
    //     objSupportButton.SetActive(true);
    // }

    private void OnDestroy()
    {
        EventManager.StopListening<LandingUIArgs>(EventNameLanding.EVENT_LOADING_REFRESH_UI, OnLoadingProgressUI);
        EventManager.StopListening<LandingUIArgs>(EventNameLanding.EVENT_LOADING_CLOSE_UI, OnCloseUI);
    }

    void OnLoadingProgressUI(LandingUIArgs args)
    {
        float duration = 0.05f;
        progression = args.currentProgress / 1f;

        OnUpdateProgressBar(progression, duration);

        if (args.isShowHintText)
        {
            textHint.gameObject.SetActive(true);
            textHint.text = UtilityLocalization.GetLocalization("landing/hint/connecting");
            OnPlayHintAnimation();
        }
        else
        {
            textHint.gameObject.SetActive(false);
        }

        DOTween.To(() => progressNum, x => progressNum = x, args.currentProgress, duration);
    }

    void OnUpdateProgressBar(float progression, float duration)
    {
        rectBar.DOKill();

        // 获取当前的 anchorMin 和 anchorMax
        Vector2 currentAnchorMin = rectBar.anchorMin;
        Vector2 currentAnchorMax = rectBar.anchorMax;

        // 只修改 anchorMax 的 x 值，而保持 y 值不变
        float targetProgress = progression; // progression 是你的进度值 (0.0 ~ 1.0)
        rectBar.DOAnchorMax(new Vector2(targetProgress, currentAnchorMax.y), duration).SetEase(Ease.Linear);
        //float targetX = originBarWidth * progression;
        //rectBar.DOSizeDelta(new Vector2(targetX, originBarHeight), duration).SetEase(Ease.Linear);
    }

    private void Update()
    {
        textProgress.text = (int)(progressNum * 100) + "%";
        //用户id
        if (GameData.userData?.userAccount?.userID != null && GameData.userData.userAccount.userID != 0)
        {
            textUserID.gameObject.SetActive(true);
            int uid = GameData.userData.userAccount.userID;
            textUserID.text = "userID: " + (uid < 0 ? "U" + Math.Abs(uid) : uid.ToString());
        }
        else
        {
            textUserID.gameObject.SetActive(false);
        }
    }

    void OnCloseUI(LandingUIArgs args)
    {
        canvasGroup.DOFade(0, 0.35f).OnComplete(() =>
        {
            Destroy(gameObject);
        }).SetEase(Ease.OutSine);
    }

    async void OnPlayHintAnimation()
    {
        int index = 0;
        string textDot = ".";
        string textHintOriginal = textHint.text;
        while (true)
        {
            index++;
            textHint.text = textHint.text + textDot;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            if (index > 2)
            {
                index = 0;
                textHint.text = textHintOriginal;
                textDot = ".";
            }
        }
    }
}

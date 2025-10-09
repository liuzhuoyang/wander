using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHealthBase : MonoBehaviour
{
    public SlicedBar imgSlicedBar;
    public Image imgBarManaFill;
    public Image imgHealthBg;
    // [SerializeField] GameObject objTextPrefab, objTextPos, objFillShield,objFillHealth;
    [SerializeField] TextMeshProUGUI textHealth, textMana;

    // int oldHealth = -1;
    // bool isShowText;
    // bool showBar;
    Tween fadeTween; // 存储 Tween 对象
    bool isLowHealth = false;

    public void Init()
    {
        // imgSlicedBar.OnSetFill(1.0f, 0.1f);
        // imgSlicedBarMana.OnSetFill(1.0f, 0.1f, true, null, true);

        //UpdateHealth(BaseControl.Instance.BaseObject.Health, BaseControl.Instance.BaseObject.MaxHealth, false);
    }

    public void UpdateHealth(float currentHealth, float maxhealth, bool isChangeHealth)
    {
        //血量向上取整 不显示小数点
        int tempHealth = Mathf.CeilToInt(currentHealth);
        imgSlicedBar.OnSetFill(tempHealth / maxhealth, 0.1f);
        textHealth.text = tempHealth.ToString();
        // if (!showBar)
        // {
        //     return;
        // }
        if (isChangeHealth)
        {
            //改变血量状态
            if (currentHealth / maxhealth <= 0.3f)
            {
                //血量小于30% 显示红色
                if (!isLowHealth)
                {
                    isLowHealth = true;
                    UtilityHaptic.Haptic(HapticTypes.Warning);
                }
                // 创建一个循环的透明度动画
                fadeTween = imgHealthBg.DOFade(0.5f, 1f) // 从当前透明度到0.5，持续1秒
                    .SetLoops(-1, LoopType.Yoyo) // 无限循环，Yoyo模式来回切换
                    .SetEase(Ease.InOutSine); // 使用平滑的缓动效果
            }
            else
            {
                isLowHealth = false;
                //血量大于30% 恢复默认颜色
                if (fadeTween != null)
                {
                    fadeTween.Kill();
                    fadeTween = null;
                    imgHealthBg.DOFade(1f, 1f);
                }
            }
        }
        //文本显示
        // if (isShowText && oldHealth != -1 && oldHealth != tempHealth)
        // {
        //     GameObject obj = Instantiate(objTextPrefab, transform);
        //     Vector2 pos = objTextPos.transform.position;
        //     obj.transform.position = pos;
        //     if (oldHealth > tempHealth)
        //     {
        //         obj.GetComponent<HudTextUIHealth>().Init("-" + (oldHealth - tempHealth), false);
        //     }
        //     else
        //     {
        //         obj.GetComponent<HudTextUIHealth>().Init("+" + (tempHealth - oldHealth), true);
        //     }
        // }
        // oldHealth = tempHealth;
    }

    // public void UpdateShield(float currentShield, float maxShield)
    // {
    //     textShield.text = Mathf.CeilToInt(currentShield).ToString();
    //     // if (!showBar)
    //     // {
    //     //     return;
    //     // }
    //     imgSlicedBarShield.OnSetFill(currentShield / maxShield, 0.1f, false, null, true);
    // }

    public void UpdateMana(float currentMana, float maxMana)
    {
        textMana.text = Mathf.CeilToInt(currentMana).ToString();
        imgBarManaFill.fillAmount = currentMana / maxMana;
    }
}

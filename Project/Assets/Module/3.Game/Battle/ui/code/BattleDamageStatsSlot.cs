using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleDamageStatsSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textDamagePercent, textName;
    [SerializeField] SlicedFilledImage fill;

    private string damageStatsName;

    public void Init(DamageStatsArgs args)
    {
        damageStatsName = args.damageStatsName;
        //判断是否为武器
        /*
        if (args.damageStatsName.Contains("_"))
        {
            //判断是否为五级装备
            textName.text = GearUtil.GetGearName(args.damageStatsName, args.damageStatsName.Contains("_5_"));
            //武器icon
            string spriteName = $"gear_{args.damageStatsName}";
            GameAssetsManager.Instance.AssignIcon(spriteName, image);
        }
        else
        {
            textName.text = string.Format(LocalizationUtility.GetLocalizationMapping("mapping/generic/mapping_generic_" + args.damageStatsName));
        }
        */
        SetDamage(Mathf.CeilToInt(args.damage), args.damagePercent);
    }

    public string GetDamageStatsName()
    {
        return damageStatsName;
    }

    public void SetDamage(int damage, float damagePercent)
    {
        //伤害
        textDamagePercent.text = UtilityTextFormat.GetNumFormat(damage);
        //伤害百分比
        fill.fillAmount = damagePercent;
    }

    public void SetAlpha(float alpha)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); // 如果没有 CanvasGroup，自动添加
        }

        canvasGroup.alpha = alpha;
    }
}
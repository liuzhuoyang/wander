using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textName, textRarity, textAttr, textAttrValue, textNextValue, textDetail, textShardCount, textStar, textNextStar;
    [SerializeField] Image imgRarityBg, imgBg, imgIcon, imgAttr, imgShard, imgShardBg;
    [SerializeField] GameObject objAction, objNextAttr;
    [SerializeField] SlicedFilledImage fillBar;


    RelicSlotViewArgs slotArgs;
    public void Init(RelicSlotViewArgs args)
    {
        slotArgs = args;
        //名字
        textName.text = UtilityLocalization.GetLocalization(args.relicData.displayName);
        //品质
        textRarity.text = UtilityLocalization.GetLocalization("generic/rarity/generic_rarity_" + args.relicData.rarity.ToString().ToLower());
        //背景
        GameAssetControl.AssignSpriteUI("relic_slot_" + args.relicData.rarity.ToString().ToLower(), imgBg);
        //图标
        GameAssetControl.AssignIcon(args.relicData.relicName, imgIcon);
        //详情
        textDetail.text = UtilityLocalization.GetLocalization(args.relicData.info);
        //属性
        string key = "mapping/attr/mapping_attr_" + Utility.ConvertCamelToSnake(args.relicData.effectAttributeType.ToString());
        string damageKey = "mapping/damage_type/mapping_damage_type_" + args.relicData.effectDamageType.ToString().ToLower();
        textAttr.text = UtilityLocalization.GetLocalization(damageKey) + " " + UtilityLocalization.GetLocalization(key);
        //消耗碎片icon
        GameAssetControl.AssignIcon(args.relicData.relicName, imgShard);
        //判断是否解锁
        if (args.star == -1)
        {
            //未解锁
            objAction.SetActive(false);
            RefreshAttrValue(0, textAttrValue, textStar);
            RefreshAttrValue(1, textNextValue, textNextStar);
            return;
        }
        //刷新星级信息
        RefreshStar();
        GameAssetControl.AssignSpriteUI("relic_slot_" + args.relicData.rarity.ToString().ToLower(), imgShardBg);
    }

    void RefreshAttrValue(int star, TextMeshProUGUI textAttrValue, TextMeshProUGUI textStar)
    {
        textStar.text = "x" + star;
        float temp = RelicFomular.GetRelicAttributeAddition(slotArgs.relicData.relicName, star);
        textAttrValue.text = (temp * 100).ToString("F1") + "%";
    }

    public void RefreshStar()
    {
        //当前星级信息
        RefreshAttrValue(slotArgs.star, textAttrValue, textStar);
        //是否是最大星级
        if (slotArgs.star >= EventNameRelic.RELIC_STAR_MAX)
        {
            objNextAttr.SetActive(false);
            objAction.SetActive(false);
            return;
        }
        //下一星级信息
        objNextAttr.SetActive(true);
        objAction.SetActive(true);
        RefreshAttrValue(slotArgs.star + 1, textNextValue, textNextStar);
        //升星消耗
        textShardCount.text = UtilityTextFormat.FormatTextColor(slotArgs.count, slotArgs.needCount);
        fillBar.fillAmount = (float)slotArgs.count / slotArgs.needCount;
    }
    public void OnClickStarUp()
    {
        RelicSystem.Instance.OnClickStarUp(slotArgs);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
        RelicSystem.Instance.OnCloseInfo();
    }
}

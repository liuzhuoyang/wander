using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 选择物品弹窗
/// </summary>
public class PopupGearInfoArgs : PopupArgs
{
    public string gearName;
}


public class PopupGearInfo : PopupBase
{
    [SerializeField] Image imgGear, imgNeed1, imgNeed2;
    [SerializeField] TextMeshProUGUI textName, textLevel, textInfo, textNeed1, textNeed2;
    [SerializeField] RectTransform rectGet;
    [SerializeField] Transform propertyTransform;


    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        PopupGearInfoArgs popupGearInfoArgs = args as PopupGearInfoArgs;
        GearData gearData = AllGear.dictData[popupGearInfoArgs.gearName];
        UserGearArgs userGearArgs = GameData.userData.userGear.dictGear[popupGearInfoArgs.gearName];
        textName.text = UtilityLocalization.GetLocalization(gearData.displayName);
        textLevel.text = "Lv." + userGearArgs.level.ToString();
        textInfo.text = UtilityLocalization.GetLocalization(gearData.info);


        int newAtk = gearData.attack;
        int addAtk = GearFormula.GetGearAttack(newAtk, userGearArgs.level);



        SetDetail(propertyTransform.GetChild(0), newAtk, addAtk, DetailType.Atk);
        SetDetail(propertyTransform.GetChild(1), gearData.triggerSpeed, 0, DetailType.TriggerSpeed);


        string needCard = GearFormula.GetGearCard(popupGearInfoArgs.gearName);

        GameAssetControl.AssignIcon(needCard, imgNeed1);
        GameAssetControl.AssignIcon(ConstantItem.COIN, imgNeed2);

        int num = ItemSystem.Instance.GetItemNum(needCard);
        textNeed1.text = GearFormula.GetGearNeedCardCount(userGearArgs.level, gearData.rarity).ToString() + "/" + num;
        textNeed2.text = GearFormula.GetGearCoin(userGearArgs.level).ToString();


        StartCoroutine(RefreshLayoutNextFrame());
    }

    private void SetDetail(Transform detail, float value, float addValue, DetailType detailType)
    {
        string valueText = ((int)value).ToString();
        switch (detailType)
        {
            case DetailType.Atk:
                valueText = (value).ToString();
                break;
            case DetailType.TriggerSpeed:
                valueText = (value).ToString();
                break;
        }

        detail.Find("stats/value").GetComponent<TextMeshProUGUI>().text = valueText;
        Transform addText = detail.Find("stats/add");
        addText.gameObject.SetActive(addValue > 0);
        addText.GetComponent<TextMeshProUGUI>().text = "+" + ((int)addValue).ToString();
    }

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectGet);
    }
}

public enum DetailType
{
    Atk,
    TriggerSpeed,
}
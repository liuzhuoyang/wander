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
    [SerializeField] Image imgGear, imgNeed1;
    [SerializeField] TextMeshProUGUI textName, textLevel, textInfo, textNeed1;
    [SerializeField] RectTransform rectGet;
    [SerializeField] Transform propertyTransform,equipTransform;
    [SerializeField] UIGearPropertySlot uiGearPropertySlot1, uiGearPropertySlot2;

    string gearName;
    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        PopupGearInfoArgs popupGearInfoArgs = args as PopupGearInfoArgs;
        gearName = popupGearInfoArgs.gearName;
        RefreshUI();
    }

    public void RefreshUI()
    {
        GearData gearData = AllGear.dictData[gearName];
        UserGearArgs userGearArgs = GameData.userData.userGear.dictGear[gearName];
        textName.text = UtilityLocalization.GetLocalization(gearData.displayName);
        textLevel.text = "Lv." + userGearArgs.level.ToString();
        textInfo.text = UtilityLocalization.GetLocalization(gearData.info);
        GameAssetControl.AssignIcon(GearFormula.GetGearIconName(gearName), imgGear);


        int newAtk = gearData.attack;
        int addAtk = GearFormula.GetGearAttack(newAtk, userGearArgs.level);


        //属性
        uiGearPropertySlot1.SetDetail(propertyTransform.GetChild(0), newAtk, addAtk, DetailType.Atk);
        uiGearPropertySlot2.SetDetail(propertyTransform.GetChild(1), gearData.triggerSpeed, 0, DetailType.TriggerSpeed);


        string needCard = GearFormula.GetGearCard(gearName);

        GameAssetControl.AssignIcon(needCard, imgNeed1);
        
        bool isEquip = GameData.userData.userGear.dictEquipGear.ContainsValue(gearName);
        if (isEquip)
        {
            equipTransform.gameObject.SetActive(false);
        }
        else
        {
            equipTransform.gameObject.SetActive(true);
        }

        int num = ItemSystem.Instance.GetItemNum(needCard);
        textNeed1.text = num + "/" + GearFormula.GetGearNeedCardCount(userGearArgs.level, gearData.rarity);
        //  GameAssetControl.AssignIcon(ConstantItem.COIN, imgNeed2);
        //  textNeed2.text = GearFormula.GetGearCoin(userGearArgs.level).ToString();


        StartCoroutine(RefreshLayoutNextFrame());
    }



    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectGet);
    }

    //点击开始装备模式
    public void OnClickEquip()
    {
        GearSystem.Instance.OnEquipStart(gearName);
        OnClose();
    }

    //点击升级
    public void OnClickUpgrade()
    {
        GearSystem.Instance.OnUpgradeGear(gearName);
        RefreshUI();
    }
}

public enum DetailType
{
    Atk,
    TriggerSpeed,
}
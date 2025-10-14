using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GearSystem : Singleton<GearSystem>
{
    public string currentEquipGearName;

    public async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("gear", "home");
        //添加功能事件，点击按钮后开启战斗功能
        FeatureUtility.OnAddFeature(FeatureType.Gear, () =>
        {
            Open();
        });
    }

    public void Open()
    {
        ModeHomeControl.OnOpen("gear");
        HeaderControl.OnShowMainHideHub(new List<string>() { "gem" });
        FooterControl.OnSelect("gear");

        Refresh();
    }

    public void Refresh()
    {
        List<GearSlotViewArgs> listGearSlotViewArgs = new List<GearSlotViewArgs>();

        foreach (var item in AllGear.dictData)
        {
            GearData gearData = item.Value;
            string gearName = item.Key;

            GearSlotViewArgs args = new GearSlotViewArgs();

            args.isLocked = GameData.userData.userGear.dictGear.ContainsKey(gearName); // 装备未解锁
            args.isEquip = GameData.userData.userGear.dictEquipGear.ContainsValue(gearName); // 装备已装备到槽位

            string itemName = GearFormula.GetGearCard(item.Key);
            string iconName = GearFormula.GetGearIconName(item.Key);

            args.gearName = item.Key;
            args.displayName = gearData.displayName;
            args.iconName = iconName;

            if (args.isLocked)
            {
                UserGearArgs userGearArgs = GameData.userData.userGear.dictGear[gearName];
                int needCount = GearFormula.GetGearNeedCardCount(userGearArgs.level, gearData.rarity);
                int count = ItemSystem.Instance.GetItemNum(itemName);
                args.count = count;
                args.needCount = needCount;
                args.progress = (float)count / needCount;
                args.level = userGearArgs.level;
            }


            listGearSlotViewArgs.Add(args);
        }


        EventManager.TriggerEvent<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, new UIGearArgs()
        {
            listGearSlotViewArgs = listGearSlotViewArgs,
            unlockSlot = GameData.userData.userGear.unlockGearCount,
            dictEquipGear = GameData.userData.userGear.dictEquipGear,
        });


    }

    /// <summary>
    /// 点击武器详情
    /// </summary>
    /// <param name="gearName"></param>
    public void OnClickSlot(string gearName)
    {
        PopupGearInfoArgs args = new PopupGearInfoArgs
        {
            popupName = "popup_gear_info",
            gearName = gearName,
        };

        PopupManager.Instance.OnPopup(args);
    }


    //点击确认装备武器
    public void OnEquipGear(int index)
    {
        if (GameData.userData.userGear.dictEquipGear.ContainsKey(index))
        {
            GameData.userData.userGear.dictEquipGear[index] = currentEquipGearName;
        }
        else
        {
            GameData.userData.userGear.dictEquipGear.Add(index, currentEquipGearName);
        }
        Refresh();
    }

    //开始装备模式
    public void OnEquipStart(string gearName)
    {
        currentEquipGearName = gearName;
        EventManager.TriggerEvent<UIGearArgs>(GearEventName.EVENT_GEAR_EQUIP_START, new UIGearArgs());

    }
    //结束装备模式
    public void OnEquipEnd()
    {
        currentEquipGearName = null;
        EventManager.TriggerEvent<UIGearArgs>(GearEventName.EVENT_GEAR_EQUIP_END, new UIGearArgs());
        Refresh();
    }

    //升级
    public void OnUpgradeGear(string gearName)
    {
        GearData gearData = AllGear.dictData[gearName];
        UserGearArgs userGearArgs = GameData.userData.userGear.dictGear[gearName];
        userGearArgs.level++;
        Refresh();
    }

    //解锁
    public void OnUnlockGear(string gearName)
    {
        GameData.userData.userGear.OnUnlockGear(gearName);
        Refresh();
    }

}

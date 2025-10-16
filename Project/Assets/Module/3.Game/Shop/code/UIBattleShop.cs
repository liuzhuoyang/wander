using System.Collections.Generic;
using UnityEngine;

public class UIBattleShop : MonoBehaviour
{
    public List<UIBattleShopSlot> listUIBattleShopSlot;

    public void Awake()
    {
        EventManager.StartListening<UIBattleShopArgs>(EventNameBattleShop.EVENT_REFRESH_BATTLE_SHOP_UI, OnBattleShopRefresh);
        listUIBattleShopSlot = new List<UIBattleShopSlot>();
        foreach (Transform child in transform)
        {
            listUIBattleShopSlot.Add(child.GetComponent<UIBattleShopSlot>());
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening<UIBattleShopArgs>(EventNameBattleShop.EVENT_REFRESH_BATTLE_SHOP_UI, OnBattleShopRefresh);
    }

    private void OnBattleShopRefresh(UIBattleShopArgs args)
    {
        //刷新商店
        for (int i = 0; i < args.listFormationItemData.Count; i++)
        {
            listUIBattleShopSlot[i].Init(args.listFormationItemData[i]);
        }
    }
}

public class UIBattleShopArgs : UIBaseArgs
{
    public List<FormationItemConfig> listFormationItemData;
}

public class EventNameBattleShop
{
    public const string EVENT_ON_BATTLE_SHOP_UI = "EVENT_ON_BATTLE_SHOP_UI";
    public const string EVENT_REFRESH_BATTLE_SHOP_UI = "EVENT_REFRESH_BATTLE_SHOP_UI";
}

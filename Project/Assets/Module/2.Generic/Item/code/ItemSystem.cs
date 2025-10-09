using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleAudioSystem;

public class ItemSystem : Singleton<ItemSystem>
{
    public void Init()
    {
        //执行更新自定义过期时间的物品更新，删除过期的或者因为更新物品不在的
        ItemUtility.OnUpdateItemCustomExpiry();
    }

    #region 获得物品
    ///获取物品数量，货币，金币钻石包括xp也属于物品
    public int GetItemNum(string itemName)
    {
        if (!AllItem.dictData.ContainsKey(itemName))
        {
            Debug.LogWarning("=== InventorySystem GetItemNum: item key:" + itemName + " not exist ===");
            return 0;
        }

        int num = 0;
        GameData.userData.userItem.userItemDict.TryGetValue(itemName, out num);

        return num;
    }

    // 得到物品
    public void GainItem(string itemName, int num)
    {
        ItemData itemData = null;
        if(AllItem.dictData.ContainsKey(itemName))
        {
            itemData = AllItem.dictData[itemName];
        }
        
        if (itemData == null)
        {
            Debug.LogError("=== InventorySystem GainItem: item key:" + itemName + " not exist ===");
            return;
        }

        if (!GameData.userData.userItem.userItemDict.ContainsKey(itemName))
        {
            GameData.userData.userItem.userItemDict.Add(itemName, 0);
        }

        GameData.userData.userItem.userItemDict[itemName] += num;
        
        //发送物件变更事件
        EventManager.TriggerEvent<ItemChangeArgs>(EventNameItem.EVENT_ON_ITEM_CHANGED, new ItemChangeArgs
        {
            listItem = new List<string>() { itemName },
            listItemChangeValue = new List<int>() { num },
        });

        HeaderControl.OnCheckHeaderItemRefresh(itemName, num);
    }

    // 使用物品
    public void UseItem(string itemName, int useNum, Action callbackSucceed, Action callbackFailed = null, string LocalizationKey = ConstantLocKey.TIP_LACK_ITEM) //TODO
    {
        if (!AllItem.dictData.ContainsKey(itemName))
        {
            callbackFailed?.Invoke();
            Debug.LogError("=== InventorySystem UseItem: item key:" + itemName + " not exist ===");
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization(LocalizationKey));
            return;
        }

        if (GameData.userData.userItem.userItemDict.ContainsKey(itemName))
        {
            if (GetItemNum(itemName) < useNum)
            {
                callbackFailed?.Invoke();
                TipManager.Instance.OnTip(UtilityLocalization.GetLocalization(LocalizationKey));
                return;
            }
                
            GameData.userData.userItem.userItemDict[itemName] -= useNum;
            callbackSucceed?.Invoke();
            HeaderControl.OnCheckHeaderItemRefresh(itemName, -useNum);
        }
        else
        {
            callbackFailed?.Invoke();
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization(LocalizationKey));
            return;
        }
    }
    #endregion

    //所有物件都齐全才会消耗
    public void UseItemBatch(List<ItemUsageArgs> listItemUsageArgs, Action callbackSucceed, Action callbackFailed = null)
    {
        bool isAllEnough = true;

        for (int i = 0; i < listItemUsageArgs.Count; i++)
        {
            string itemName = listItemUsageArgs[i].itemName;
            int costNum = listItemUsageArgs[i].costNum;
            if (GetItemNum(itemName) < costNum)
            {
                isAllEnough = false;
                TipManager.Instance.OnTip(UtilityLocalization.GetLocalization(ConstantLocKey.TIP_LACK_ITEM));//GameData.AllLocalization[ConstantLocKey.TIP_LACK_ITEM].content);
                break;
            }
        }
        if (isAllEnough)
        {
            for (int i = 0; i < listItemUsageArgs.Count; i++)
            {
                string itemName = listItemUsageArgs[i].itemName;
                int costNum = listItemUsageArgs[i].costNum;
                UseItem(itemName, costNum, null, null);
            }
            callbackSucceed?.Invoke();

            OnCheckUpdateCurrency(listItemUsageArgs);
        }
        else
        {
            callbackFailed?.Invoke();
        }
    }
    void OnCheckUpdateCurrency(List<ItemUsageArgs> listItemUsageArgs)
    {
        foreach (ItemUsageArgs usageArgs in listItemUsageArgs)
        {

        }
    }
    
    #region 物件表现
    //物品掉落音效
    public static void OnPlayItemDropSFX(string itemName)
    {
        ItemData itemArgs = AllItem.dictData[itemName];
        string sfxName = AllItem.dictData[itemName].sfxDrop;

        //没有定义的，都播放item
        if (string.IsNullOrEmpty(itemArgs.sfxDrop))
            sfxName = "sfx_item_drop_generic";

        AudioManager.Instance.PlaySFX(sfxName);
    }

    //物品收集音效
    public static void OnPlayItemCollectSFX(string itemName)
    {
        ItemData itemArgs = AllItem.dictData[itemName];
        string sfxName = AllItem.dictData[itemName].sfxCollect;

        //没有定义的，都播放item
        if (string.IsNullOrEmpty(itemArgs.sfxCollect))
            sfxName = "sfx_item_collect_generic";

        AudioManager.Instance.PlaySFX(sfxName);
    }
    #endregion

    #region 清空物件
    /// <summary>
    /// 设置物件数量为0
    /// </summary>
    public void EmptyItem(string itemName)
    {
        if (GameData.userData.userItem.userItemDict.ContainsKey(itemName))
            GameData.userData.userItem.userItemDict[itemName] = 0;
    }
    #endregion

    #region 跳转物品
    public void OnItemInfoPopup(string itemName)
    {
        PopupItemInfoArgs args = new PopupItemInfoArgs(){
            popupName = "popup_item_info",
            itemName = itemName,
        };
        PopupManager.Instance.OnPopup(args);
    }
    #endregion
}

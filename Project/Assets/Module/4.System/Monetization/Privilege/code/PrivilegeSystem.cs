using System;
using System.Collections.Generic;

/// <summary>
/// 赛季卡系统
/// </summary>
public class PrivilegeSystem : Singleton<PrivilegeSystem>
{
    UserPrivilege userPrivilege;
    #region 初始化
    public void Init()
    {
        userPrivilege = GameData.userData.userPrivilege;
        //判断特权是否过期
        List<string> listPrivilegeToRemove = new List<string>();
        foreach (var item in userPrivilege.dictPrivilege)
        {
            PrivilegeData privilegeData = AllPrivilege.dictData[item.Key];
            if (privilegeData.isPermanent)
            {
                continue;
            }

            /*
            if (!ItemExpirationSystem.Instance.CheckIsItemExpiration(privilegeData.itemName))
            {
                continue;
            }*/

            listPrivilegeToRemove.Add(item.Key);
        }
        foreach (var item in listPrivilegeToRemove)
        {
            userPrivilege.dictPrivilege.Remove(item);
        }
        //红点
        gameObject.AddComponent<PrivilegePinHandler>().Init();
    }
    #endregion

    #region 打开界面
    public async void Open()
    {
        await UIMain.Instance.OpenUI("privilege", UIPageType.Normal);
        HeaderControl.OnShowUIHideHub("ui_privilege", new List<string>() { "gem" });
        EventManager.TriggerEvent<UIPrivilegeArgs>(EventNamePrivilege.EVENT_PRIVILEGE_INIT_UI, null);
    }
    #endregion

    #region 购买赛季卡
    public void OnPurchase(PrivilegeData data, Action onSuccess)
    {
        //已购买
        if (data.isPermanent)
        {
            if (ItemSystem.Instance.GetItemNum(data.privilegeItemName) > 0)
            {
                return;
            }
        }
        else
        {
            if (!ItemUtility.CheckIsItemExpiration(data.privilegeItemName))
            {
                return;
            }
        }
        //购买
        string sku = AllIap.dictData[data.productID].sku.skuID;
        // IAPControl.Instance.OnPurchaseConsumable(sku, data.productID, () =>
        // {
            OnUnlockPrivilege(data);
            onSuccess?.Invoke();
        // }, () =>
        // {
            //购买失败
        // });
    }

    public List<RewardArgs> OnUnlockPrivilege(PrivilegeData data, bool showReward = true)
    {
        //购买成功
        userPrivilege.dictPrivilege.Add(data.privilegeName, new UserPrivilegeData()
        {
            //30天
            claimed = false,
        });
        //发放购买奖励
        // 优化，这里时间的概念应该做到Item里
        if (data.isPermanent)
        {
            ItemSystem.Instance.GainItem(data.privilegeItemName, 1);
        }
        else
        {
            ItemUtility.OnAddItemCustomExpiry(data.privilegeItemName);
        }
        var reward = UtilityReward.GetRewardString(data.unlockReward);
        List<RewardArgs> listReward = new List<RewardArgs>();
        listReward.Add(new RewardArgs() { reward = reward.reward, num = reward.count });
        //检查红点
        gameObject.GetComponent<PrivilegePinHandler>().CheckPrivilegePin();
        if (showReward)
        {
            RewardSystem.Instance.OnReward(listReward);
        }
        return listReward;
    }
    #endregion

    #region 领取每日奖励
    public void OnClaim(PrivilegeData data, Action onSuccess)
    {
        //判断是否购买
        if (data.isPermanent)
        {
            if (ItemSystem.Instance.GetItemNum(data.privilegeItemName) < 0)
            {
                return;
            }
        }
        else
        {
            if (ItemUtility.CheckIsItemExpiration(data.privilegeItemName))
            {
                return;
            }
        }
        //判断是否领取
        if (userPrivilege.dictPrivilege[data.privilegeName].claimed)
        {
            return;
        }
        //发放每日奖励
        List<RewardArgs> listRewardArgs = new List<RewardArgs>();
        for (int i = 0; i < data.claimReward.Count; ++i)
        {
            var reward = UtilityReward.GetRewardString(data.claimReward[i]);
            listRewardArgs.Add(new RewardArgs() { reward = reward.reward, num = reward.count });
        }
        userPrivilege.dictPrivilege[data.privilegeName].claimed = true;
        RewardSystem.Instance.OnReward(listRewardArgs);
        onSuccess?.Invoke();
        //检查红点
        gameObject.GetComponent<PrivilegePinHandler>().CheckPrivilegePin();
    }
    #endregion

    #region 检查是否拥有特权
    public bool CheckIsOwnPrivilege(PrivilegeType privilegeType)
    {
        foreach (var item in userPrivilege.dictPrivilege)
        {
            if (!AllPrivilege.dictData.ContainsKey(item.Key))
            {
                continue;
            }
            PrivilegeData privilegeData = AllPrivilege.dictData[item.Key];
            if (privilegeData.listPrivilegeType.Contains(privilegeType))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
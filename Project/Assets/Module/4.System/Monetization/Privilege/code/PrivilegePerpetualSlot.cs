using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrivilegePerpetualSlot : MonoBehaviour
{
    [Header("名称")]
    [SerializeField] TextMeshProUGUI textName;
    [Header("激活状态")]
    [SerializeField] GameObject objActive;
    [Header("解锁奖励")]
    [SerializeField] ItemViewSlot itemViewSlot;
    [SerializeField] GameObject objUnlock;
    [Header("每日奖励")]
    [SerializeField] List<ItemViewSlot> listItemViewSlot;
    [Header("按钮")]
    [SerializeField] GameObject objPrice;
    [SerializeField] GameObject objClaim;
    [SerializeField] GameObject objClaimed;

    PrivilegeData data;
    public void OnInit(PrivilegeData data)
    {
        this.data = data;
        //名称
        textName.text = UtilityLocalization.GetLocalization(data.displayName);
        //购买奖励
        var purchaseReward = UtilityReward.GetRewardString(data.unlockReward);
        itemViewSlot.Init(purchaseReward.reward, purchaseReward.count);
        //每日奖励
        for (int i = 0; i < data.claimReward.Count; ++i)
        {
            var reward = UtilityReward.GetRewardString(data.claimReward[i]);
            listItemViewSlot[i].Init(reward.reward, reward.count);
        }
        for (int i = data.claimReward.Count; i < listItemViewSlot.Count; ++i)
        {
            listItemViewSlot[i].gameObject.SetActive(false);
        }
        //刷新按钮
        RefreshBtn();
    }

    void RefreshBtn()
    {
        //判断是否解锁
        if (ItemSystem.Instance.GetItemNum(data.privilegeItemName) <= 0)
        {
            objActive.SetActive(false);
            objUnlock.SetActive(false);
            objClaim.SetActive(false);
            objClaimed.SetActive(false);
            objPrice.SetActive(true);
            return;
        }
        objPrice.SetActive(false);
        objActive.SetActive(true);
        objUnlock.SetActive(true);

        var userPrivilegeData = GameData.userData.userPrivilege.dictPrivilege[data.privilegeName];
        //判断是否领取
        objClaim.SetActive(!userPrivilegeData.claimed);
        objClaimed.SetActive(userPrivilegeData.claimed);
    }

    public void OnClickPurchase()
    {
        PrivilegeSystem.Instance.OnPurchase(data, () =>
        {
            RefreshBtn();
        });
    }

    public void OnClickClaim()
    {
        PrivilegeSystem.Instance.OnClaim(data, () =>
        {
            RefreshBtn();
        });
    }
}
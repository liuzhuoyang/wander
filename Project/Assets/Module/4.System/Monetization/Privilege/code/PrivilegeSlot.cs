using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 赛季卡槽位
/// </summary>
public class PrivilegeSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] GameObject objOn;
    [SerializeField] ItemViewSlot itemPurchaseReward;
    [SerializeField] GameObject objPurchaseRewardClaim;
    [SerializeField] TextMeshProUGUI textPrice, textDay;
    [SerializeField] GameObject objTime;
    [SerializeField] RectTransform rectTime;
    [SerializeField] TimeViewBase timeViewBase;
    [SerializeField] List<ItemViewSlot> listItemViewSlot;
    [SerializeField] GameObject objLock, objClaim, objPrice;

    PrivilegeData data;
    public void OnInit(PrivilegeData data)
    {
        this.data = data;
        //名称
        textName.text = UtilityLocalization.GetLocalization(data.displayName);
        //购买奖励
        var purchaseReward = UtilityReward.GetRewardString(data.unlockReward);
        itemPurchaseReward.Init(purchaseReward.reward, purchaseReward.count);
        //每日奖励
        for (int i = 0; i < listItemViewSlot.Count; ++i)
        {
            var reward = UtilityReward.GetRewardString(data.claimReward[i]);
            listItemViewSlot[i].Init(reward.reward, reward.count);
        }
        //刷新按钮
        RefreshBtn();
    }

    void RefreshBtn()
    {
        //判断是否解锁
        if (ItemUtility.CheckIsItemExpiration(data.privilegeItemName))
        {
            objOn.SetActive(false);
            objPurchaseRewardClaim.SetActive(false);
            objPrice.SetActive(true);
            objClaim.SetActive(false);
            objLock.SetActive(false);
            objTime.SetActive(false);
            textDay.gameObject.SetActive(true);
            textDay.text = UtilityLocalization.GetLocalization("page/privilege/page_privilege_info");
            textPrice.text = IAPControl.Instance.GetLocalPriceString(data.productID);
            return;
        }
        objPurchaseRewardClaim.SetActive(true);
        objOn.SetActive(true);
        var userPrivilegeData = GameData.userData.userPrivilege.dictPrivilege[data.privilegeName];
        objPrice.SetActive(false);
        //判断是否领取
        objClaim.SetActive(!userPrivilegeData.claimed);
        objLock.SetActive(userPrivilegeData.claimed);
        //剩余天数
        int days = ItemUtility.GetItemExpiration(data.privilegeItemName);
        if (days <= 1)
        {
            objTime.SetActive(true);
            textDay.gameObject.SetActive(false);
            timeViewBase.Refresh(TimeManager.Instance.GetSecondUntilNextDay());
            StartCoroutine(RefreshLayoutNextFrame());
        }
        else
        {
            objTime.SetActive(false);
            textDay.gameObject.SetActive(true);
            textDay.text = UtilityLocalization.GetLocalization("page/privilege/page_privilege_remaining", days.ToString());
        }
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

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTime);
    }
}
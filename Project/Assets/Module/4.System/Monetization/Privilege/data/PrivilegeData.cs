using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "privilege_data", menuName = "OniData/System/Monetization/Privilege/PrivilegeData", order = 1)]
public class PrivilegeData : ScriptableObject
{
    [BoxGroup("Info", LabelText = "基础信息")]
    [LabelText("特权名称"), ReadOnly] public string privilegeName;

    [BoxGroup("Edit", LabelText = "可编辑内容")]
    [LabelText("是否永久卡")] public bool isPermanent;

    [BoxGroup("Edit")]
    [LabelText("显示名称"), ValueDropdown("GetLocalizationKeyList")] public string displayName;

    [BoxGroup("Edit")]
    [OnValueChanged("OnProductIDChanged")]
    [LabelText("ProductID"), ValueDropdown("GetProductIDList")] public string productID;

    [BoxGroup("Edit")]
    [LabelText("特权物品名字"), ReadOnly] public string privilegeItemName;

    [BoxGroup("Edit")]
    [LabelText("解锁奖励"), ReadOnly] public string unlockReward;

    [BoxGroup("Edit")]
    [LabelText("兑现奖励"), ReadOnly] public List<string> claimReward;

    [BoxGroup("Edit")]
    [LabelText("特权类型")] public List<PrivilegeType> listPrivilegeType;

#if UNITY_EDITOR

    [BoxGroup("Preview", LabelText = "预览信息")]
    [ReadOnly]
    [LabelText("商品价格")]
    public string previewPrice;

    [BoxGroup("Ref")]
    [LabelText("特权卡物品")] public ItemData refPrivilegeItem;

    [BoxGroup("Ref", LabelText = "编辑器编辑引用")]
    [LabelText("解锁奖励物品")] public ItemDataSlot refUnlockRewardItem;

    [BoxGroup("Ref")]
    [LabelText("兑现奖励物品")] public List<ItemDataSlot> refListClaimReward;

    [BoxGroup("Action", LabelText = "操作")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public void InitData()
    {
        privilegeName = this.name;
        privilegeItemName = refPrivilegeItem.itemName;
        unlockReward = refUnlockRewardItem == null || refUnlockRewardItem.itemData == null ? string.Empty : refUnlockRewardItem.itemData.itemName + "^" + refUnlockRewardItem.num;
        claimReward = new List<string>();
        foreach (ItemDataSlot item in refListClaimReward)
        {
            claimReward.Add(item.itemData.itemName + "^" + item.num);
        }
        EditorUtility.SetDirty(this);
    }

    void OnProductIDChanged()
    {
        string path = GameDataControl.GetAssetPath("all_iap");
        List<IAPData> listIAPAsset = AssetsFinder.FindAllAssetsOfAllSubFolders<IAPData>(path);
        foreach (IAPData asset in listIAPAsset)
        {
            if(productID == asset.productID)
            {
                previewPrice = asset.priceUSD.ToString();
            }
        }
    }

    List<string> GetProductIDList()
    {
        List<string> list = new List<string>();
        list.Add("");
        string path = GameDataControl.GetAssetPath("all_iap");
        List<IAPData> listIAPAsset = AssetsFinder.FindAllAssetsOfAllSubFolders<IAPData>(path);
        foreach (IAPData asset in listIAPAsset)
        {
            list.Add(asset.productID);
        }
        return list;
    }

    private List<string> GetLocalizationKeyList()
    {
        List<string> list = new List<string>();
        list.Add("");
        string path = GameDataControl.GetLocPath("all_privilege");
        List<LocalizationData> listLocalizationAsset = AssetsFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData data in listLocalizationAsset)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                list.Add(item.key);
            }
        }
        return list;
    }
#endif
}
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FeatureData", menuName = "OniData/Generic/Feature/FeatureData", order = 1)]
public class FeatureData : ScriptableObject
{
    [ReadOnly]
    [BoxGroup("Info", LabelText = "基础信息")]
    public FeatureType featureType;

#if UNITY_EDITOR
    [PreviewField]
    [BoxGroup("Info")]
    public Sprite icon;
#endif

    [BoxGroup("Info")]
    [ReadOnly]
    public string iconName;

    [BoxGroup("Info")]
    [ValueDropdown("GetLocalizationKeyList")]
    [OnValueChanged("OnDisplayNameChanged")]
    public string displayName;

#if UNITY_EDITOR
    [BoxGroup("Info")]
    [ReadOnly]
    public string previewName;
#endif

    [BoxGroup("Entrance", LabelText = "入口配置")]
    public FeatureEntranceType entranceType;
    [BoxGroup("Entrance")]
    public int entrancePriority;

    [BoxGroup("Unlock")]
    public bool isShowUnlockAnimation;
    [LabelText("解锁入口"), ShowIf("isShowUnlockAnimation")]

    [BoxGroup("Unlock", LabelText = "解锁配置")]
    public FeatureType unlockViewFeatureType = FeatureType.None;

    [BoxGroup("Unlock")]
    public FeatureUnlockConditionType unlockConditionType;

    [BoxGroup("Unlock")]
    [ShowIf("unlockConditionType", FeatureUnlockConditionType.Level)]
    public int unlockLevelID;

#if UNITY_EDITOR
    [Button("Init Data")]
    void InitData()
    {
        string targetName = this.name.Replace("feature_", "");
        featureType = (FeatureType)Enum.Parse(typeof(FeatureType), targetName, true);
        unlockViewFeatureType = unlockViewFeatureType == FeatureType.None ? featureType : unlockViewFeatureType;
        if (featureType == FeatureType.None)
        {
            Debug.LogError("FeatureType is None");
        }

        iconName = icon == null ? string.Empty : icon.name;
    }

    void OnDisplayNameChanged()
    {
        previewName = LocalizationDataCollection.GetValue(displayName);
    }

    public List<string> GetLocalizationKeyList()
    {
        List<string> listKey = new List<string>();

        List<LocalizationData> listAssets = AssetsFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(FeatureDataCollection.LOC_PATH);
        foreach (LocalizationData asset in listAssets)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
#endif
}

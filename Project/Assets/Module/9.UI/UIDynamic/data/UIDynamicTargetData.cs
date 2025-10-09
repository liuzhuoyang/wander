using UnityEngine;
using Sirenix.OdinInspector;
using System;

[Serializable]
[CreateAssetMenu(fileName = "UIDynamicTargetData", menuName = "OniData/UI/UIDynamicTargetData", order = 1)]
public class UIDynamicTargetData : ScriptableObject
{
    [BoxGroup("Info", LabelText = "基础信息")]
    [ReadOnly]
    public string targetName;
    [BoxGroup("Edit", LabelText = "可编辑内容")]
    public UIDynamicTargetType targetType;

    #if UNITY_EDITOR
    [BoxGroup("Reference", LabelText = "参考数据")]
    [ShowIf("targetType", UIDynamicTargetType.Item)]
    public ItemData itemData;
    [BoxGroup("Reference")]
    [ShowIf("targetType", UIDynamicTargetType.Feature)]
    public FeatureData featureData;
    #endif

    [BoxGroup("Action", LabelText = "初始化")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public void InitData()
    {
        if(targetType == UIDynamicTargetType.Item)
        {
            targetName = itemData.itemName;
        }
        else if(targetType == UIDynamicTargetType.Feature)
        {
            targetName = featureData.featureType.ToString();
        }
    }
}

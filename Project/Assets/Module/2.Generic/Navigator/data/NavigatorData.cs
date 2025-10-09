using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.Linq;
#endif

[CreateAssetMenu(fileName = "NavigatorData", menuName = "OniData/Generic/Navigator/NavigatorData", order = 1)]
public class NavigatorData : ScriptableObject
{
    #if UNITY_EDITOR
    [OnValueChanged("InitData")]
    [BoxGroup("Reference", LabelText = "参考数据")]
    [Tooltip("这个Feature资源是拿来配置导航数据的，只会在编辑器中引用，不会在游戏中使用，也不会进入包里")]
    public FeatureData featureData;
    #endif

    [ReadOnly]
    [BoxGroup("Info", LabelText = "基础信息")]
    public string navigatorName;
    
    [ReadOnly]
    [BoxGroup("Info")]
    public string displayName;
    
    [Tooltip("是否可导航")]
    [BoxGroup("Edit", LabelText = "可编辑内容")]
    public bool isNavigable;

#if UNITY_EDITOR
    [ReadOnly]
    [BoxGroup("Preview", LabelText = "预览信息")]
    public string previewDisplayName;

    [ReadOnly]
    [PreviewField]
    [BoxGroup("Preview")]
    public Sprite previewIcon;
#endif

    [ReadOnly]
    [BoxGroup("Info")]
    public string iconName;

    [ReadOnly]
    [BoxGroup("Info")]
    public FeatureType featrue;

    [BoxGroup("Action", LabelText = "操作")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public void InitData()
    {
        navigatorName = featureData.name;
        
        displayName = featureData.displayName;
        iconName = featureData.iconName;
        featrue = featureData.featureType;

        #if UNITY_EDITOR
        previewDisplayName = LocalizationDataCollection.GetValue(displayName);
        previewIcon = featureData.icon;
        #endif
    }
}

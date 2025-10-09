using System.Collections.Generic;
using UnityEngine;

public static class FeatureUtility
{
    private static Dictionary<FeatureType, System.Action> featureAction = new Dictionary<FeatureType, System.Action>
    {
       
    };

    public static void OnFeature(FeatureType featureType)
    {
        Debug.Log($"=== FeatureUtility: {featureType} 开始执行 ===");
        if (featureAction.TryGetValue(featureType, out var action))
        {
            action.Invoke();
        }
        else
        {
            Debug.LogWarning($"=== FeatureUtility: {featureType} 没有正确配置，去到模块初始化的地方添加进功能字典 ===");
        }
    }

    //添加Feature功能开启事件
    public static void OnAddFeature(FeatureType featureType, System.Action action)
    {
        featureAction.TryAdd(featureType, action);
    }

    public static bool CheckIsUnlock(FeatureType featureType)
    {
        if (!AllFeature.dictData.ContainsKey(featureType))
        {
            UnityEngine.Debug.Log("=== FeatureData: key not found ===");
            return false;
        }

        FeatureData featureData = AllFeature.dictData[featureType];

        switch (featureData.unlockConditionType)
        {
            case FeatureUnlockConditionType.None:
                return true;
            case FeatureUnlockConditionType.Progress:
                bool isUnlock = GameData.userData.userLevel.levelProgressMain.levelIndex >= featureData.unlockLevelID; 
                return isUnlock;

            case FeatureUnlockConditionType.Coming:
                return false;
            default:
                return false;
        }
    }

}
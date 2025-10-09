using UnityEngine;
using System.Collections.Generic;

//整个方法需要重新处理
public class FeatureEntranceHandler : MonoBehaviour
{
    public FeatureEntranceType entranceType;

    bool isInitRunning = false;
    bool isPending = false;
    void Start()
    {
        if (entranceType == FeatureEntranceType.Default)
        {
            Debug.LogError("=== FeatureEntranceHandler: 不能配置为PreSet ===");
            return;
        }

        //监听功能解锁事件
        EventManager.StartListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_TRIGGER_UI, OnFeatureUnlockTrigger);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_TRIGGER_UI, OnFeatureUnlockTrigger);
    }


    async void OnInitEntranceBar()
    {
        if (isInitRunning)
        {
            isPending = true;
            return;
        }
        isPending = false;
        isInitRunning = true;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //获取所有需要生成入口的功能
        List<FeatureData> listFeatureToInit = new List<FeatureData>();
        foreach (FeatureData featureData in AllFeature.dictData.Values)
        {
            //检查与当前入口类型是否匹配
            if (featureData.entranceType == entranceType)
            {

            }
        }

        //根据优先级排序,从小到大
        listFeatureToInit.Sort((a, b) => a.entrancePriority.CompareTo(b.entrancePriority));

        //生成入口
        foreach (FeatureData featureData in listFeatureToInit)
        {
            //动态生成入口预制体
            Instantiate(await GameAsset.GetPrefabAsync("feature_entrance_" + featureData.featureType.ToString().ToLower()), transform);
        }

        isInitRunning = false;
    }

    //有新功能解锁 重新初始化入口
    void OnFeatureUnlockTrigger(UIFeatureArgs args)
    {
        //如果解锁的是当前入口类型,则重新初始化入口
        if (AllFeature.dictData[args.featureType].entranceType == entranceType)
        {
            OnInitEntranceBar();
        }
    }
}

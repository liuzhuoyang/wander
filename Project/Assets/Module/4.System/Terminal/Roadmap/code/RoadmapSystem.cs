using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 导航系统
/// </summary>
public class RoadmapSystem : Singleton<RoadmapSystem>
{
    UIRoadmapArgs uiRoadmapArgs;
    UserRoadmap userRoadmap;

    #region 初始化
    public void Init()
    {
        userRoadmap = GameData.userData.userRoadmap;
        uiRoadmapArgs = new UIRoadmapArgs();
        uiRoadmapArgs.listRoadmapSlot = new List<RoadmapSlotArgs>();
        //筛选根据进度解锁且有弹窗的 TODO 这里新框架有调整
        List<FeatureData> listFeatureData = AllFeature.dictData.Values.Where(x => x.unlockConditionType == FeatureUnlockConditionType.Progress).ToList();
        //根据进度排序
        listFeatureData = listFeatureData.OrderBy(x => x.unlockLevelID).ToList();
        //根据进度筛选
        foreach (FeatureData featureData in listFeatureData)
        {
            uiRoadmapArgs.listRoadmapSlot.Add(new RoadmapSlotArgs()
            {
                featureType = featureData.featureType,
                isClaimed = userRoadmap.listRewardType.Contains((int)featureData.featureType),
                canClaim = false
            });
        }
        CheckCanClaim();
        gameObject.AddComponent<RoadmapPinHandler>().Init();
        EventManager.StartListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_REFRESH_ROADMAP, OnRefreshRoadmap);
    }

    void OnRefreshRoadmap(UIFeatureArgs args)
    {
        CheckCanClaim();
        gameObject.GetComponent<RoadmapPinHandler>().CheckRoadmapPin();
    }
    #endregion

    #region 检查是否可以领取
    void CheckCanClaim()
    {
        foreach (RoadmapSlotArgs slotArgs in uiRoadmapArgs.listRoadmapSlot)
        {
            if (slotArgs.isClaimed || slotArgs.canClaim)
            {
                continue;
            }
            if (FeatureUtility.CheckIsUnlock(slotArgs.featureType))
            {
                slotArgs.canClaim = true;
            }
        }
    }

    public bool IsCanClaim()
    {
        return uiRoadmapArgs.listRoadmapSlot.Any(x => x.canClaim);
    }
    #endregion

    #region 打开
    public async void Open()
    {
        await UIMain.Instance.OpenUI("roadmap", UIPageType.Normal);
        EventManager.TriggerEvent<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_UI, uiRoadmapArgs);
    }
    #endregion

    #region 领取
    public void OnClaim(FeatureType featureType)
    {
        foreach (RoadmapSlotArgs slotArgs in uiRoadmapArgs.listRoadmapSlot)
        {
            if (slotArgs.featureType != featureType || !slotArgs.canClaim || slotArgs.isClaimed)
            {
                continue;
            }
            slotArgs.isClaimed = true;
            slotArgs.canClaim = false;
            userRoadmap.listRewardType.Add((int)slotArgs.featureType);
        }
        RewardSystem.Instance.OnReward(new List<RewardArgs>(){
            new RewardArgs(){
                reward = ConstantItem.GEM,
                num = 10
            }
        });
        gameObject.GetComponent<RoadmapPinHandler>().CheckRoadmapPin();
        EventManager.TriggerEvent<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_CLAIM, uiRoadmapArgs);
    }
    public void OnClaim()
    {
        int count = 0;
        foreach (RoadmapSlotArgs slotArgs in uiRoadmapArgs.listRoadmapSlot)
        {
            if (!slotArgs.canClaim)
            {
                continue;
            }
            slotArgs.isClaimed = true;
            slotArgs.canClaim = false;
            userRoadmap.listRewardType.Add((int)slotArgs.featureType);
            ++count;
        }
        if (count <= 0)
        {
            return;
        }
        RewardSystem.Instance.OnReward(new List<RewardArgs>(){
            new RewardArgs(){
                reward = ConstantItem.GEM,
                num = 10 * count
            }
        });
        gameObject.GetComponent<RoadmapPinHandler>().CheckRoadmapPin();
        EventManager.TriggerEvent<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_CLAIM, uiRoadmapArgs);
    }
    #endregion
}
using System.Collections.Generic;
using UnityEngine;

//功能锁脚本
public class FeatureLockHandler : MonoBehaviour
{
    public FeatureType featureType;

    public FeatureLockType featureLockType;

    GameObject objLock;

    void Start()
    {
        //找到子节点下的feature_lock对象
        objLock = transform.Find("feature_lock").gameObject;

        // 如果配置为预先放置，则添加到FeatureUnlockControl中
        // 一般底部导航栏的按钮都是预先放置Static，创建的页面里的lock都是动态的Dynamic
        if (featureLockType == FeatureLockType.Static)
        {
            FeatureUnlockControl.AddStaticLock(featureType, objLock);
            objLock.SetActive(true);
        }else
        {
            //Dynamic根据数据判断解锁
        }
    }

    public void OnClick()
    {
        FeatureData featureData = AllFeature.dictData[featureType];
        string content = "";
        switch (featureData.unlockConditionType)
        {
            case FeatureUnlockConditionType.Progress:
                content = UtilityLocalization.GetLocalization("feature/dynamic/unlock_at_level_x", $"{featureData.unlockLevelID}");
                break;
            case FeatureUnlockConditionType.Coming:
                content = UtilityLocalization.GetLocalization("generic/coming_soon");
                break;
        }
        TipManager.Instance.OnTip(content);
        //TooltipManager.Instance.ShowTooltipText(contentList, transform.parent.GetComponent<RectTransform>(), transform.position, Direction.Top);
    }
}

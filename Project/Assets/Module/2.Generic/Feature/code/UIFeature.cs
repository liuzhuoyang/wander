using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;

public class UIFeature : UIBase
{
    [SerializeField] GameObject prefabFlyer;
    [SerializeField] TextMeshProUGUI textFeature;
    [SerializeField] Image imgFeature;
    [SerializeField] Animator animator;

    UIFeatureArgs args;

    void Awake()
    {
        EventManager.StartListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_OPEN_UI, OnOpen);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_OPEN_UI, OnOpen);
    }

    void OnOpen(UIFeatureArgs args)
    {
        this.args = args;
        ShowFeature();
    }

    void ShowFeature()
    {
        FeatureData featureData = AllFeature.dictData[args.featureType];
        
        string displayNameKey = featureData.displayName;
        textFeature.text = UtilityLocalization.GetLocalization(displayNameKey);

        GameAssetControl.AssignSpriteUI(featureData.iconName, imgFeature);
    }

    public void OnContinue()
    {

        ActingSystem.Instance.OnActing(this.name);
        //获取功能数据
        FeatureData featureData = AllFeature.dictData[args.featureType];

        //播放关闭动画
        animator.SetTrigger("OnClose");

        //显示飞行器
        GameObject flyer = Instantiate(prefabFlyer, transform);
        flyer.transform.position = imgFeature.transform.position;
        GameAssetControl.AssignSpriteUI(featureData.iconName, flyer.GetComponent<Image>());

        //查找飞行目的
        Transform target = null;
        target = UIDynamicControl.Instance.GetDynamicTarget(featureData.unlockViewFeatureType.ToString());

        if (target == null)
        {
            Debug.LogError($"=== UIFeature: 找不到目标: {featureData.unlockViewFeatureType} ===");
            return;
        }

        // 获取起始位置和目标位置之间的距离
        //float distance = Vector3.Distance(flyer.transform.position, target.position);

        // 根据距离和速度计算移动时间
        float duration = 0.8f;

        flyer.transform.DOMove(target.position, duration).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            Destroy(flyer.gameObject);
            target.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                target.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            });

            ActingSystem.Instance.StopActing(this.name);
            OnDoneUnlockFeature();
        });
    }

    void OnDoneUnlockFeature()
    {
        CloseUI();
        args.callbackClose?.Invoke();
    }
}

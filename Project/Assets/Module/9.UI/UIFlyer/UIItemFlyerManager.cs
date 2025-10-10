using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using SimpleVFXSystem;
using System.Collections.Generic;

//UI道具飞行的现实管理，播放特效与音效
//使用场景：
//1. 领取金币，金币飞到目标ui位置
//2. 常规ui特效，播放一次
public class UIItemFlyerManager : Singleton<UIItemFlyerManager>
{
    private const string FLYER_POOL_KEY = "vfx_ui_flyer";

    //常规ui特效，播放一次
    void OnVFXUI(UIVFXArgs args)
    {
        // GameObject go = PoolManager.Instance.GetObject($"uivfx_{args.target}", GameAssetGenericManager.Instance.GetVFXPrefab(args.target), this.transform, true);
        var go = VFXManager.Instance.PlayVFX($"uivfx_{args.target}", new Vector2(args.posX, args.posY)).GetComponent<VFXItemFlyer>();
        StartCoroutine(TimerTick.Start(args.life, () => go.OnItemFlyEnd()));
    }
    //批量创建UI飞行物VFX
    public async void OnVFXFlayerBatchUI(List<RewardArgs> listReward)
    {
        OnVFXFlayerBatchUI(new UIVFXFlyerBatchArgs()
        {
            listReward = listReward
        });
    }
    //飞行特效
    //会创建一堆物品，散落在屏幕上，然后飞到目标位置
    public async void OnVFXFlayerBatchUI(UIVFXFlyerBatchArgs args)
    {
        int rewardTypeNum = args.listReward.Count;
        for (int i = 0; i < rewardTypeNum; i++)
        {
            string rewardName = args.listReward[i].reward;
            int rewardNum = Mathf.Clamp(args.listReward[i].num, 1, 15);

            Vector3 targetPosition = Vector3.zero;
            //获取目标位置
            targetPosition = UIDynamicControl.Instance.GetDynamicTarget(rewardName).position;

            ItemSystem.OnPlayItemDropSFX(rewardName);

            float x = Random.Range(-250, 250); // 假设Canvas中心为坐标原点
            float y = Random.Range(-250, 250);
            Vector2 spawmPosition = new Vector3(x, y, 0);

            float delay = 0;
            for (int j = 0; j < rewardNum; j++)
            {
                delay = j * 0.02f;

                var go = VFXManager.Instance.PlayVFX(FLYER_POOL_KEY, spawmPosition).GetComponent<VFXItemFlyer>();

                go.transform.localScale = Vector2.zero;
                RectTransform rectTransform = go.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = spawmPosition;

                GameAssetControl.AssignIcon(rewardName, go.GetComponent<Image>());

                go.transform.DOScale(Vector2.one, 0.2f).SetEase(Ease.OutSine).SetDelay(delay);

                Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * 350 + spawmPosition;

                rectTransform.DOLocalMove(randomPosition, 0.3f).SetDelay(delay).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    // 起始位置
                    Vector3 startPosition = go.transform.position;

                    // 获取屏幕中心的Y坐标
                    float screenCenterY = Screen.height / 2;

                    // 获取屏幕中心的Y坐标
                    float screenCenterX = Screen.width / 2;

                    // 根据目标位置相对于屏幕中心的位置调整Y坐标
                    float offsetY = startPosition.y >= screenCenterY ? -100 : 100;

                    // 往左边偏一点点
                    float offsetX = -50;//targetPosition.x >= screenCenterX ? -50 : 50;

                    // 计算中间点
                    Vector3 midPoint = startPosition + new Vector3(offsetX, offsetY, 0);//(startPosition + targetPosition) / 2 + new Vector3(0, 20, 0);

                    // 创建路径
                    Vector3[] path = new Vector3[] {
                        startPosition,  // 起点
                        midPoint,       // 中间弧度点
                        targetPosition  // 终点
                    };

                    // 使用DoPath移动GameObject
                    go.transform.DOPath(path, 1.0f, PathType.CatmullRom, PathMode.Sidescroller2D, 2)
                        .SetEase(Ease.InOutSine) // 设置缓动类型，根据需要调整
                        .SetDelay(1f) // 开始之前的延迟
                        .OnComplete(() =>
                        {
                            ItemSystem.OnPlayItemCollectSFX(rewardName);
                            // GameObject go = PoolManager.Instance.GetObject($"uivfx_{args.target}", GameAssetGenericManager.Instance.GetVFXPrefab(args.target), this.transform, true);
                            go.OnItemFlyEnd();
                            VFXManager.Instance.PlayVFX("vfx_ui_shared_impact_generic_001", targetPosition);
                        });
                });
            }

            await UniTask.WaitForSeconds(0.1f);
        }
        await UniTask.WaitForSeconds(1);
    }

    //单个创建UI飞行物VFX
    public void OnUIFlyerVFX(UIVFXFlyerArgs args)
    {
        // GameObject go = PoolManager.Instance.GetObject(FLYER_POOL_KEY, prefabFlyer, this.transform, true);
        var go = VFXManager.Instance.PlayVFX(FLYER_POOL_KEY, args.spawmPos).GetComponent<VFXItemFlyer>();

        //设置位置
        Vector2 spawmPos = args.spawmPos;

        //设置图标
        GameAssetControl.AssignIcon(args.rewardName, go.GetComponent<Image>());

        //设置缩放
        go.transform.localScale = Vector2.zero;
        go.transform.DOScale(Vector2.one * args.scale, 0.35f).SetEase(Ease.OutBack);

        //设置路径
        Vector3 targetPos = args.targetPos;
        go.transform.DOPath(new Vector3[] { spawmPos, targetPos }, 0.8f, PathType.CatmullRom, PathMode.Sidescroller2D).SetEase(Ease.InSine).SetDelay(args.delay).OnComplete(() =>
        {
            go.OnItemFlyEnd();
            // PoolManager.Instance.Release(FLYER_POOL_KEY, go);
        });
    }
}

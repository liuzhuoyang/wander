using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using BattleActor;
using SimpleVFXSystem;
using SimpleAudioSystem;

namespace BattleLaunch.Bullet
{
    public class BulletManager : Singleton<BulletManager>
    {
        [SerializeField] private BulletDataCollection bulletDataCollection;

        private Transform bulletRoot;
        private Dictionary<string, GameObject> bulletDict;

        #region 自身生命周期
        //加载初始资源，游戏进入时执行
        public async UniTask Init()
        {
            bulletDict = new Dictionary<string, GameObject>();
            await GameAsset.LoadAssets(bulletDataCollection.GetDataCollection(), LoadBulletPrefab);
        }
        #endregion

        #region 资源获取
        public BulletBase GetBulletInstance(string bulletKey, TeamMask excludeTeam, AttackData bulletAttackData)
        {
            var bulletData = bulletDataCollection.GetDataByKey(bulletKey);
            var bullet = Instantiate(bulletDict[bulletKey], bulletRoot).GetComponent<BulletBase>();
            bullet.Init(bulletAttackData, excludeTeam, bulletData);

            return bullet;
        }
        async UniTask LoadBulletPrefab(BulletData data)
        {
            //子弹素材获取
            GameObject go = await GameAsset.GetAssetAsync<GameObject>(data.m_bulletRef);
            if (go == null)
            {
                Debug.LogError($"未找到 {data.BulletKey} 的素材.");
            }
            if (!bulletDict.ContainsKey(data.BulletKey))
                bulletDict.Add(data.BulletKey, go);
        }
        #endregion

        #region 战斗节点处理
        public void StartBattle()
        {
            if(bulletRoot==null)
                bulletRoot = new GameObject("[Bullet]").transform;
        }
        public void CleanUpBullet()
        {
            foreach (Transform child in bulletRoot)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion

        #region 获取目标
        public List<IBattleActor> FindTargetsInRange(Vector2 center, float effectRange, TeamMask teamMask)
            => BattleActorScanSystem.Instance.FindTargets<IBattleActor>(center, effectRange, ActorScanOrder.Default, teamMask);
        #endregion

        #region 子弹效果
        public static void PlayBulletImpactEffect(string sfx_impact, string vfx_impact, Vector2 pos, float effectRange, float angle = 0, float volume = 1)
        {
            VFXManager.Instance.PlayVFX(vfx_impact, pos, angle, effectRange);
            AudioManager.Instance.PlaySFX(sfx_impact, volume);
        }
        #endregion
    }
}
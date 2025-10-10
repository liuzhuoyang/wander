using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SimpleVFXSystem
{
    public enum VFXManageMode
    {
        //由发起者手动创建与删除VFX，适合场景中持续存在的VFX
        Manual = 0,
        //播放后由PoolManager管理，VFXManager管理PoolManager，适合非Particle的一般VFX，例如Zone，ChangeColor，AddIcon等等
        Pooled = 1,
        //播放后由ParticleSystem自行管理，VFXManager只管理ParticleSystem
        ManagedParticle = 2,
    }
    public class VFXManager : Singleton<VFXManager>
    {
        [SerializeField] private VFXDataCollection_SO vfxDataCollection;
        //记录与管理vfx对象池
        private VFXPoolManager vfxPoolManager;
        //记录与管理Particles
        private VFXParticleManager vfxParticleManager;
        //VFX Prefab索引
        private Dictionary<string, GameObject> vfxPrefabDict = new Dictionary<string, GameObject>();

        #region 数据获取
        public GameObject GetVFXPrefab(string vfxKey)
        {
            if (vfxPrefabDict.TryGetValue(vfxKey, out var prefab))
            {
                return prefab;
            }
            Debug.LogError($"VFX prefab not found: {vfxKey}");
            return null;
        }
        async UniTask LoadVFXPrefab(VFXData_SO data)
        {
            GameObject go = await GameAsset.GetAssetAsync<GameObject>(data.vfxKey);
            if (!vfxPrefabDict.ContainsKey(data.vfxKey))
                vfxPrefabDict.Add(data.vfxKey, go);
        }
        #endregion

        #region 生命周期
        //初始化
        public async UniTask Init()
        {
            vfxPoolManager = gameObject.AddComponent<VFXPoolManager>();
            vfxParticleManager = gameObject.AddComponent<VFXParticleManager>();

            vfxPrefabDict = new Dictionary<string, GameObject>();
            await GameAsset.LoadAssets(vfxDataCollection.GetDataCollection(), LoadVFXPrefab);
        }
        #endregion

        #region 战斗节点处理
        public void CleanUpBattle()
        {
            vfxPoolManager.ClearAllVFXPool();
            vfxParticleManager.CleanUpParticles();
        }
        #endregion

        #region 世界特效管理
        public GameObject PlayVFX(string vfxName, Vector2 createPos, float angle = 0, float scaleMultiplier = 1, Vector2[] controlPoints = null, GameObject[] controlObjects = null)
        {
            if (string.IsNullOrEmpty(vfxName))
                return null;

            if (!vfxPrefabDict.ContainsKey(vfxName))
            {
                Debug.LogError($"{vfxName} is not included in All VFX !!!");
                return null;
            }

            //创建VFX，并调整位置
            var vfxData = vfxDataCollection.GetDataByKey(vfxName);

            GameObject vfxObj = null;
            switch (vfxData.manageMode)
            {
                case VFXManageMode.Manual:
                    vfxObj = Instantiate(vfxPrefabDict[vfxName], transform);
                    ModifyVFXObjectTransform(vfxObj, vfxName, createPos, scaleMultiplier, angle);
                    break;
                case VFXManageMode.Pooled:
                    vfxObj = vfxPoolManager.GetVFXFromPool(vfxName);
                    ModifyVFXObjectTransform(vfxObj, vfxName, createPos, scaleMultiplier, angle);
                    break;
                case VFXManageMode.ManagedParticle:
                    vfxObj = vfxParticleManager.PlayParticle(vfxName, createPos, scaleMultiplier, angle);
                    break;
            }

            var vfxMono = vfxObj.GetComponent<VFXMono>();
            if (vfxMono != null)
            {
                vfxMono.InitVFX(vfxData.manageMode, controlPoints, controlObjects);
            }

            return vfxObj;
        }
        public void ReleaseVFX(VFXMono vfxMono)
        {
            if (vfxMono == null)
            {
                Debug.LogWarning("VFX Mono is already deleted");
                return;
            }
            switch (vfxMono.vfxManagedMode)
                {
                    case VFXManageMode.Manual:
                        Destroy(vfxMono.gameObject);
                        break;
                    case VFXManageMode.Pooled:
                        vfxPoolManager.ReleaseVFXInPool(vfxMono.gameObject);
                        break;
                    case VFXManageMode.ManagedParticle:
                        break;
                }
        }
        //修改VFX的Transform与属性
        void ModifyVFXObjectTransform(GameObject vfxObj, string vfxName, Vector2 targetPos, float scaleMultiplier, float angle)
        {
            //修改GameObject属性与Transform
            vfxObj.name = vfxName;
            vfxObj.transform.position = targetPos;
            vfxObj.transform.eulerAngles = Vector3.forward * angle;
            vfxObj.transform.localScale *= scaleMultiplier;
        }
        #endregion

        public void OnVFXFlayerBatchUI(List<RewardArgs> listRewardArgs)
        {
            UIVFX.Instance.OnVFXFlayerBatchUI(new UIVFXFlyerBatchArgs()
            {
                listReward = listRewardArgs,
            });
        }

        public void OnUIVFX(string targetName, Vector2 pos)
        {
            var args = AllVFX.dictData[targetName];
            UIVFX.Instance.OnVFXUI(new UIVFXArgs()
            {
            target = args.vfxName,
            posX = pos.x,
            posY = pos.y,
            life = args.life
            });
        }
    }
}
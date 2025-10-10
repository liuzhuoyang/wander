using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

namespace SimpleVFXSystem
{
    /// <summary>
    /// 管理战场中创建的添加到对象池的特效
    /// </summary>
    public class VFXPoolManager : MonoBehaviour
    {
        private HashSet<string> battleVFXPools;
        private Transform vfxPoolRoot;

        void Awake()
        {
            battleVFXPools = new HashSet<string>();
        }
        void OnDestroy()
        {
            battleVFXPools.Clear();
        }
        internal GameObject GetVFXFromPool(string vfxName)
        {
            if(vfxPoolRoot == null)
            {
                vfxPoolRoot = new GameObject("[VFXPoolRoot]").transform;
            }
            if (!battleVFXPools.Contains(vfxName))
                battleVFXPools.Add(vfxName);

            var vfxObj = PoolManager.Instance.GetObject(vfxName, VFXManager.Instance.GetVFXPrefab(vfxName), vfxPoolRoot, false, 100);

            return vfxObj;
        }
        internal void ClearAllVFXPool()
        {
            foreach (var pool in battleVFXPools)
            {
                PoolManager.Instance.Dispose(pool);
            }
            battleVFXPools.Clear();
        }
        internal void ReleaseVFXInPool(GameObject vfxObj)
        {
            if (vfxObj == null)
            {
                Debug.LogError("VFX Object is already deleted");
                return;
            }

            //若已经进入回收池回收了
            if (vfxObj.name == PoolManager.POOL_KEYWORD)
            {
              Debug.LogError("VFX Object is already Recycled in pool");
              return;
            }
            PoolManager.Instance.Release(vfxObj.name, vfxObj);
        }
    }
}

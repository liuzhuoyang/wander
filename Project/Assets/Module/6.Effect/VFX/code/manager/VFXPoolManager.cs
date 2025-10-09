using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理战场中创建的添加到对象池的特效
/// 通过注册事件，在战斗结束后会销毁对象池与对象
/// 因此通过它创建的特效都只能存在于战斗中
/// </summary>
public class VFXPoolManager : MonoBehaviour
{
    private HashSet<string> battleVFXPools;

    void Awake()
    {
        battleVFXPools = new HashSet<string>();
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void OnDestroy()
    {
        battleVFXPools.Clear();
        EventManager.StopListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    public GameObject GetVFXFromPool(string vfxName)
    {
        if(!battleVFXPools.Contains(vfxName))
            battleVFXPools.Add(vfxName);

        return PoolManager.Instance.GetObject(vfxName, GameAssetGenericManager.Instance.GetVFXPrefab(vfxName), transform, false, 100);
    }
    void OnAction(ActionArgs args)
    {
        if(args.action == ActionType.ArenaEnd 
         ||args.action == ActionType.TowerEnd
         ||args.action == ActionType.DungeonEnd
         ||args.action == ActionType.LevelEnd)
        {
            ClearAllVFXPool();
        }
    }
    void ClearAllVFXPool()
    {
        foreach(var pool in battleVFXPools)
        {
            PoolManager.Instance.Dispose(pool);
        }
        battleVFXPools.Clear();
    }
    public void ReleaseVFXInPool(GameObject vfxObj)=>PoolManager.Instance.Release(vfxObj.name, vfxObj);
}

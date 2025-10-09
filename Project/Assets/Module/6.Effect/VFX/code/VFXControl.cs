using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXControl : Singleton<VFXControl>
{
  //记录与管理vfx对象池
  VFXPoolManager vfxPoolManager;
  //记录与管理Particles对象
  VFXParticleManager vfxParticleManager;
  //UI显示层，调用过多，避免去使用UI的事件框架，直接存引用来使用
  UIVFX uiViewVFX;
  // #if DEVELOPMENT_BUILD || UNITY_EDITOR
  public bool ForceNoVFX = false;
  // #endif


  //初始化
  public void Init()
  {
    uiViewVFX = UIVFX.Instance;
    vfxPoolManager = gameObject.AddComponent<VFXPoolManager>();
    vfxParticleManager = gameObject.AddComponent<VFXParticleManager>();
  }
/*
  #region 创建世界坐标VFX
  public GameObject OnCreateVFX(Vector2 startPos, string vfxName, float angle = 0)
    => OnCreateVFX(startPos, startPos, 1, vfxName, angle);
  public GameObject OnCreateVFX(Vector2 startPos, IBattleActor target, string vfxName, float angle = 0)
    => OnCreateVFX(startPos, target, 1, vfxName, angle);
  public GameObject OnCreateVFX(Vector2 startPos, float scaleMultiplier, string vfxName, float angle = 0) 
    => OnCreateVFX(startPos, startPos, scaleMultiplier, vfxName, angle);

  public GameObject OnCreateVFX(Vector2 startPos, Vector2 targetPos, float scaleMultiplier, string vfxName, float angle = 0)
    => BuildVFX((vfx, life)=>vfx.OnCreateVFX(startPos, targetPos, life), targetPos, scaleMultiplier, vfxName, angle);
  GameObject OnCreateVFX(Vector2 startPos, IBattleActor target, float scaleMultiplier, string vfxName, float angle)
    => BuildVFX((vfx, life)=>vfx.OnCreateVFX(startPos, target, life), target.aimCenter, scaleMultiplier, vfxName, angle);
  
  GameObject BuildVFX(Action<IVFX_Behavior, float> Builder, Vector2 targetPos, float scaleMultiplier, string vfxName, float angle)
  {
// #if DEVELOPMENT_BUILD || UNITY_EDITOR
    //强制关闭所有VFX的创建
    if (ForceNoVFX)
    {
      return null;
    }
// #endif
    //数据检查
    if (string.IsNullOrEmpty(vfxName)) return null;
    if (!GameData.allVFX.vfxDict.ContainsKey(vfxName))
    {
      Debug.LogError($"{vfxName} is not included in All VFX !!!");
      return null;
    }

    //创建VFX，并调整位置
    var args = GameData.allVFX.vfxDict[vfxName];
    GameObject go = null;

    switch (args.lifeMode)
    {
      case VFXLifeCycleMode.Manual:
        go = Instantiate(GameAssetsManager.Instance.GetVFXPrefab(vfxName), transform);
        ModifyVFXObjectTransform(go, vfxName, targetPos, scaleMultiplier, angle);
        break;
      case VFXLifeCycleMode.Pooled:
        go = vfxPoolManager.GetVFXFromPool(vfxName);
        ModifyVFXObjectTransform(go, vfxName, targetPos, scaleMultiplier, angle);
        break;
      case VFXLifeCycleMode.ManagedParticle:
        go = vfxParticleManager.PlayParticle(vfxName, targetPos, scaleMultiplier, angle);
        break;
    }

    var vfxBehaviors = go.GetComponents<IVFX_Behavior>();
    if (vfxBehaviors != null) 
    {
      foreach(var vfxOnCreate in vfxBehaviors)
        Builder(vfxOnCreate, args.life);
    }

    //计划VFX的回收
    if (args.lifeMode != VFXLifeCycleMode.ManagedParticle && args.life >= 0)
    {
      var delayRecycle = go.GetComponent<VFX_LifeControl>();
      if(delayRecycle==null)
      {
        delayRecycle = go.AddComponent<VFX_LifeControl>();
      }
      delayRecycle.StartLifeCounting(args.life, CleanUpVFX);
    }

    return go;
  }
  void ModifyVFXObjectTransform(GameObject vfxObj, string vfxName, Vector2 targetPos, float scaleMultiplier, float angle)
  {
    //修改GameObject属性与Transform
    vfxObj.name = vfxName;
    vfxObj.transform.position = targetPos;
    vfxObj.transform.eulerAngles = Vector3.forward*angle;

    //修改VFX的大小，若存在具体的修改方式，采用修改方式，若没有则直接缩放。
    var scaler = vfxObj.GetComponent<IVFX_ScaleModifier>();
    if(scaler!=null)
    {
      scaler.ScaleVFX(scaleMultiplier);
    }
    else
    {
      vfxObj.transform.localScale *= scaleMultiplier; 
    }
  }
  //将VFX循环利用到pool里面，注意，此方法一定要检查是否该vfx本身是可以回收的。否则可能会重复创建，或是pool物件丢失
  public void CleanUpVFX(GameObject vfxObj)
  {
    if (vfxObj == null)
    {
      Debug.LogError("VFX Object is already deleted");
      return;
    }
    var vfxBehaviors = vfxObj.GetComponents<IVFX_Behavior>();
    if (vfxBehaviors != null)
    {
      foreach (var vfxBehavior in vfxBehaviors)
        vfxBehavior.OnCleanUp();
    }
    //若已经进入回收池回收了
    if (vfxObj.name == Pool.POOL_KEYWORD)
    {
      Debug.LogWarning("VFX Object is already Recycled in pool");
      return;
    }
    switch (GameData.allVFX.vfxDict[vfxObj.name].lifeMode)
    {
      case VFXLifeCycleMode.Manual:
        Destroy(vfxObj);
        break;
      case VFXLifeCycleMode.Pooled:
        vfxPoolManager.ReleaseVFXInPool(vfxObj);
        break;
      case VFXLifeCycleMode.ManagedParticle:
        //通过Particle创建则有ParticleSystem直接管理，不进行单独清理
        break;
    }
  }
  //停止持续vfx的方法，此方法不一定会立刻删除vfx，由vfx自身管理
  public void StopLoopVFX(GameObject vfx)
  {
    var stopper = vfx.GetComponent<IVFX_Stopable>();
    if(stopper!=null)
        stopper.StopEffect();
    else
        CleanUpVFX(vfx);
  }
  #endregion

  #region UI 特效
  //创建UI飞行物，并使用reward名称寻找目标点
  public void OnUIFlyerVFX(string rewardName, Vector2 spawnPos, float scale = 1, bool isWorldSpace = false, float delay = 0.8f)
  {
    Vector2 targetPos = DynamicControl.Instance.GetDynamicTargetPosition(rewardName);
    OnUIFlyerVFX(rewardName, targetPos, spawnPos, scale, isWorldSpace, delay);
  }
  //单个创建UI飞行物VFX，但另外使用动态类型查找目标
  public void OnUIFlyerVFX(string rewardName, DynamicTriggerType triggerType, Vector2 spawnPos, float scale = 1, bool isWorldSpace = false, float delay = 0.8f)
  {
    Vector2 targetPos = DynamicControl.Instance.GetDynamicTargetPosition(triggerType);
    OnUIFlyerVFX(rewardName, targetPos, spawnPos, scale, isWorldSpace, delay);
  }

  public void OnUIFlyerVFX(string rewardName, Vector2 _targetPos, Vector2 spawnPos, float scale, bool isWorldSpace, float delay)
  {
    uiViewVFX.OnUIFlyerVFX(new UIVFXFlyerArgs()
    {
      rewardName = rewardName,
      spawmPos = spawnPos,
      isWorldSpaceSpawn = isWorldSpace,
      targetPos = _targetPos,
      scale = scale,
      delay = delay
    });
  }

  //跳跃消失
  public void OnUIJumpVFX(string targetName, Vector2 pos, float scale = 1, float force = 1, float life = 1, bool isWorldSpace = false)
  {
    uiViewVFX.OnUIJumpVFX(new UIVFXJumpArgs()
    {
      target = targetName,
      pos = pos,
      scale = scale,
      force = force,
      life = life,
      isWorldSpace = isWorldSpace
    });
  }

*/
  //批量创建UI飞行物VFX
  //使用场景：
  //1. 领取金币，因为金币数量通常比较多，会创建一堆金币散落到屏幕，然后金币飞到目标ui位置
  public void OnVFXFlayerBatchUI(List<RewardArgs> listRewardArgs)
  {
      uiViewVFX.OnVFXFlayerBatchUI(new UIVFXFlyerBatchArgs()
      {
          listReward = listRewardArgs,
      });
  }

  public void OnUIVFX(string targetName, Vector2 pos)
  {
    var args = AllVFX.dictData[targetName];
    uiViewVFX.OnVfxUI(new UIVFXArgs()
    {
      target = args.vfxName,
      posX = pos.x,
      posY = pos.y,
      life = args.life
    });
  }
}

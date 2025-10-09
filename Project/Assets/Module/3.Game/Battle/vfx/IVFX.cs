using System;
using UnityEngine;

//以下接口一般放在vfx prefab上
#region VFX逻辑接口
public interface IVFX_Behavior
{
    //一般VFX生成初始化
    void OnCreateVFX(Vector2 start, Vector2 target, float lifeTime);
    //只定义出生点的VFX初始化
    void OnCreateVFX(Vector2 start, float lifeTime)
        =>OnCreateVFX(start, start, lifeTime);
    //定义出生点，和目标的VFX初始化
    //void OnCreateVFX(Vector2 start, IBattleActor target, float lifeTime)
    //    =>OnCreateVFX(start, target.aimCenter, lifeTime);
    //清理VFX，执行在VFX被销毁或被回收前，由VFXControl统一调度
    void OnCleanUp();
}
//放缩某个VFX的方法
public interface IVFX_ScaleModifier
{
    public void ScaleVFX(float scaleMultiplier);
}
//停止某个VFX的方法
public interface IVFX_Stopable
{
    void StopEffect();
}
#endregion

//以下接口一般放在BattleActor的prefab上
#region VFX功能支持接口
//可支持VFX控制变色
public interface IVFX_ColorChangable
{
    void ChangeRendererColor(Color newColor);
    void ResetRendererColor();
}
//可支持将VFX锚定到BattleActor上
public interface IVFX_Mountable
{
    Vector2 GetMountOffset(Vector2 basicOffset); //获取到一个锚点的坐标，因为单位可能会有不同大小，因此需要一个调整值，例如添加在单位上的孢子，需要动态调整位置
    Vector2 GetMountScale(); //获取锚点的缩放值，因为单位大小可能不同，因此需要一个调整值。例如，一个添加在单位上的效果，需要根据单位大小去放缩
}
#endregion
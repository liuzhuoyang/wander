using UnityEngine;

namespace SimpleVFXSystem
{
    public abstract class VFXMono : BattleBehaviour, IVFX
    {
        public VFXManageMode vfxManagedMode { get; protected set; } = VFXManageMode.Pooled;
        protected Vector2[] controlPoints;
        protected GameObject[] controlObjects;
        public void InitVFX(VFXManageMode mode, Vector2[] points, GameObject[] controlObjects)
        {
            this.vfxManagedMode = mode;
            this.controlPoints = points;
            this.controlObjects = controlObjects;
            VFXBegin();
        }
        public override void BattleUpdate()
        {
            base.BattleUpdate();
            VFXUpdate();
        }
        protected virtual void VFXUpdate() { }
        protected virtual void VFXBegin() { }
        protected virtual void VFXEnd()
        {
            VFXManager.Instance?.ReleaseVFX(this);
        }
    }
    //以下接口一般放在vfx prefab上
    #region VFX逻辑接口
    public interface IVFX
    {
        VFXManageMode vfxManagedMode{ get; }
        //VFX初始化
        void InitVFX(VFXManageMode mode, Vector2[] points, GameObject[] controlObjects);
    }
    #endregion
}
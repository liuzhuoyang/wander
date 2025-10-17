using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo.Unit
{
    public enum UnitFaceType
    {
        Horizontal = 0,
        TopView = 1,
    }
    //单位视觉层面管理
    //处理包括动画、VFX、朝向、体积大小等等表现层的功能
    public class UnitViewBasic : MonoBehaviour
    {
        [Header("Basic Init")]
        [SerializeField] protected UnitFaceType unitFaceType;

        [Header("Animation")]
        public bool PlayDeathAnimation = false;
        [SerializeField] protected Transform animationRoot;

        [Header("发射点位")]
        [SerializeField] protected Transform launchPoint; //发射枪口，子弹与发射特效从这里发出

        [Header("Hit Flash")]
        [SerializeField] protected SpriteRenderer[] ignoreFlash; //不会因击中，替换材质的sprite，例如尾焰等等

        [Header("Audio")]
        [SerializeField] protected float minAttackPlayStep = 0.1f; //音效最短播放间隔
        [SerializeField] protected float overallVolume = 0.5f;

        [Header("HUD Info")]
        [SerializeField] protected Transform hud_Pivot;

        protected Animator animator;
        protected UnitBase self;
        protected SpriteRenderer[] bodySprites;
        protected Material[] originMat;
        protected AnimatorControl animatorControl;
        protected Material flashMat;
        protected float aimResetTimer = 0;
        protected float defaultAimAngle = 0;
        protected float attackTimer = 0;
        private float flashtimer;
        private bool isFaceRight = true;

        public bool bodyActive => animationRoot.gameObject.activeSelf;
        public Transform m_animatorRoot => animationRoot;

        private const float HIT_FEEDBACK_TIME = 0.15f;

        #region 生命周期
        //单位创建时初始化
        public virtual void Init(UnitBase unit)
        {
            self = unit;

            animator = GetComponent<Animator>();
            animatorControl = new AnimatorControl().Init(animator);
            List<SpriteRenderer> tempSprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
            foreach (var sprite in ignoreFlash)
            {
                tempSprites.Remove(sprite);
            }
            bodySprites = tempSprites.ToArray();

            originMat = new Material[bodySprites.Length];
            for (int i = 0; i < bodySprites.Length; i++)
            {
                originMat[i] = bodySprites[i].sharedMaterial;
            }

            flashMat = UnitManager.Instance.GetHitFeedbackMat();

            self.OnUnitGetHit += OnHitFeedback;
        }
        //单位销毁时清理
        public virtual void CleanUp()
        {
            self.OnUnitGetHit -= OnHitFeedback;
        }
        public virtual void UpdateView()
        {
            //更新单位的闪烁
            if (flashtimer > 0)
            {
                flashtimer -= Time.deltaTime;
                flashtimer = Mathf.Max(0, flashtimer);
                //还原材质
                if (flashtimer <= 0)
                {
                    RecoverFromFlash();
                }
            }
            //地面单位更新朝向
            Vector2 vel = self.unitMovement.GetVector();
            switch (unitFaceType)
            {
                case UnitFaceType.Horizontal:
                    UpdateFacing(vel);
                    break;
                case UnitFaceType.TopView:
                    AlignOrientationToVelocity(vel);
                    break;
            }
        }
        #endregion

        #region 单位事件处理
        //单位受到攻击时执行
        void OnHitFeedback()
        {
            //仅当单位未死亡时才会执行
            if (self.IsDead)
            {
                RecoverFromFlash();
                return;
            }

            RefreshFlash();
        }
        #endregion

        #region 朝向
        //单位朝速度转向
        private void AlignOrientationToVelocity(Vector2 vel)
        {
            if (vel == Vector2.zero) return;
            float yaw = Vector3.SignedAngle(vel, Vector3.up, Vector3.back);
            transform.rotation = Quaternion.Euler(0, 0, yaw);
        }
        public void UpdateFacing(Vector2 dir)
        {
            if (unitFaceType != UnitFaceType.Horizontal)
                return;
            if (isFaceRight)
            {
                if (dir.x <= -0.1f)
                {
                    isFaceRight = false;
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
            else
            {
                if (dir.x >= 0.1f)
                {
                    isFaceRight = true;
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
        }
        #endregion

        #region 动画
        public void PlayIdleAnimation() => animatorControl.PlayIdle();
        public void PlayRunAnimation() => animatorControl.PlayRun();
        public void PlayDieAnimation() => animatorControl.PlayDie();
        public void PlayAttackAnimation() => animatorControl.PlayAttack();
        public bool IsDeathAnimationFinish()
        {
            return animator.enabled ? animatorControl.IsDieDone() : true;
        }
        public bool IsSpawnAnimationFinish()
        {
            return animator.enabled ? animatorControl.IsSpawnDone() : true;
        }
        public void SetMoveAnimationSpeed(float speedMultiplier) => animatorControl.SetMoveSpeed(speedMultiplier);
        public void SetAttackAnimationSpeed(float speedMultiplier) => animatorControl.SetAttackSpeed(speedMultiplier);
        #endregion

        #region 特效支持
        //刷新命中闪烁特效，仅当从未闪烁状态到闪烁状态时，才替换材质
        void RefreshFlash()
        {
            //仅当从未闪烁状态到闪烁状态时，才替换材质
            if (flashtimer <= 0)
            {
                foreach (var spriteRenderer in bodySprites)
                {
                    spriteRenderer.sharedMaterial = flashMat;
                }
            }
            //刷新闪烁效果持续时间
            flashtimer += HIT_FEEDBACK_TIME;
        }
        //移除命中闪烁特效，还原材质
        void RecoverFromFlash()
        {
            flashtimer = 0;
            for (int i = 0; i < bodySprites.Length; i++)
            {
                bodySprites[i].sharedMaterial = originMat[i];
            }
        }
        public void ResetHitFeedback()
        {
            flashtimer = 0;
            for (int i = 0; i < bodySprites.Length; i++)
            {
                bodySprites[i].sharedMaterial = originMat[i];
            }
        }
        public void ChangeRendererColor(Color newColor)
        {
            for (int i = 0; i < bodySprites.Length; i++)
            {
                if (bodySprites[i] != null)
                    bodySprites[i].color = newColor;
            }
        }
        public void ResetRendererColor()
        {
            for (int i = 0; i < bodySprites.Length; i++)
            {
                if (bodySprites[i] != null)
                    bodySprites[i].color = Color.white;
            }
        }
        #endregion

        #region HUD支持
        public Vector2 GetHUDPivotPos() => hud_Pivot.position;
        public Transform GetHUDPivotTrans() => hud_Pivot;
        #endregion

        #region Launch支持
        public virtual void OnLaunchSend() { }
        public virtual void OnLaunchComplete() { }
        #endregion

        public void SwitchBody(bool isOn) => animationRoot.gameObject.SetActive(isOn); //用于切换单位的显示与隐藏，但不关闭单位行为
        public bool IsVisible()
        {
            if (!animationRoot.gameObject.activeSelf) return false;
            foreach (var renderer in bodySprites)
            {
                if (renderer.isVisible) return true;
            }
            return false;
        }

        #region 发射点位支持
        //根据弹道数量，和连射数量，获取到发射点
        public virtual Transform[] GetAbilityLaunchTranses(int totalCount, int launchLayer = 0) => new Transform[1] { GetLaunchTrans() };
        public virtual Transform GetAbilityLaunchTrans(int launchIndex, int launchLayer = 0) => GetLaunchTrans();
        public Transform GetLaunchTrans() => launchPoint == null ? transform : launchPoint;
        #endregion
    }
}
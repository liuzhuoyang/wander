using System.Collections.Generic;
using BattleActor;
using BattleLaunch;
using SimpleAudioSystem;
using SimpleVFXSystem;
using UnityEngine;

namespace BattleGear
{
    public class GearViewBasic : BattleBehaviour
    {
        [Header("发射设置")]
        [SerializeField] protected Transform turretRoot; //炮台身体
        [SerializeField] protected LaunchLayer mainLaunchLayer; //发射点位

        [Header("瞄准")]
        [SerializeField] protected bool aimTowardsTarget;
        [SerializeField] protected float aimResetTime = 4f;
        [SerializeField] protected float aimLerpSpeed = 10;

        [Header("动画设置")]
        [SerializeField] protected Animator animator;

        [Header("音效设置")]
        [SerializeField] private float audioStep;
        [SerializeField, Range(0, 1)] private float audioVolume = 1f;

        private Vector2 targetAimDir = Vector2.up;
        private Vector2 aimDir = Vector2.up;
        private float aimResetTimer = 0;
        private GearBase self;

        //音效设置
        private float audioPlayTime = 0;
        

        private static readonly int trigger_launch_id = Animator.StringToHash("fire"); //武器发射状态触发
        private static readonly int trigger_reset_id = Animator.StringToHash("reset"); //武器状态重置

        public void Init(GearBase gear)
        {
            self = gear;
            audioPlayTime = 0;
            ResetView();
        }
        public LaunchLayer GetLaunchLayer() => mainLaunchLayer;
        public void UpdateOrientation(IBattleActor aimTarget)
        {
            if (aimTowardsTarget)
            {
                if (!IBattleActor.IsInvalid(aimTarget))
                {
                    aimResetTimer = 0;
                    targetAimDir = aimTarget.position - (Vector2)turretRoot.position;
                }
                else
                {
                    if (aimResetTimer >= aimResetTime)
                    {
                        targetAimDir = Vector2.up;
                    }
                    else
                    {
                        aimResetTimer += Time.deltaTime;
                    }
                }
            }
            aimDir = Vector2.Lerp(aimDir, targetAimDir, Time.deltaTime * aimLerpSpeed);
            turretRoot.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(aimDir, Vector2.up));
        }
        public void OnGearBeginFire()
        {
            animator.SetTrigger(trigger_launch_id);

            //音效播放
            var audioKey = self.sfx_beginFire;
            if (Time.time - audioPlayTime >= audioStep)
            {
                audioPlayTime = Time.time;
                AudioManager.Instance.PlaySFX(audioKey, audioVolume);
            }
            //特效播放
            var vfxKey = self.vfx_beginFire;
            VFXManager.Instance.PlayVFX(vfxKey, mainLaunchLayer.launchTrans[0].position, -mainLaunchLayer.launchTrans[0].eulerAngles.z, 1);
        }
        public void ResetView()
        {
            animator.SetTrigger(trigger_reset_id);
            targetAimDir = aimDir = Vector2.up;
        }
    }
}

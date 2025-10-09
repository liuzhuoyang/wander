using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace SimpleVFXSystem
{
    /// <summary>
    /// 用于VFX自身进行生命周期的计算，时间到后自行清理
    /// 通过VFXPoolManager赋予
    /// </summary>
    [HideInMenu]
    public class VFX_LifeControl : BattleBehaviour
    {
        private float lifeTime;
        private float timer = 0;
        private Action<GameObject> onRecycle;
        public void StartLifeCounting(float lifeTime, Action<GameObject> onRecycle)
        {
            this.lifeTime = lifeTime;
            this.onRecycle = onRecycle;
            this.timer = 0;
            BattleBehaviourManager.Instance.RegisterBehaviour(this);
        }
        public override void BattleUpdate()
        {
            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                onRecycle?.Invoke(gameObject);
                BattleBehaviourManager.Instance.UnregisterBehaviour(this);
            }
        }
    }
}
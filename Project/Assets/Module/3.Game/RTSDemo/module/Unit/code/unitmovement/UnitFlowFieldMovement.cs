using UnityEngine;
using RTSDemo.Grid;

namespace BattleActor.Unit
{
    public class UnitFlowFieldMovement : BattleBehaviour
    {
        [SerializeField] private float slerpSpeed = 7;
        private UnitMovement unitMovement;
        private Rigidbody2D m_rigid;

        protected override void Start()
        {
            base.Start();
            m_rigid = GetComponent<Rigidbody2D>();
            unitMovement = GetComponent<UnitMovement>();
        }
        public override void BattleUpdate()
        {
            Vector2 direction = RTS_GridWorldSystem.Instance.GetNodeFromWorldPos(m_rigid.position).bestDirection.Vector;
            unitMovement.SlerpVelocity(direction, slerpSpeed);
        }
    }
}
using Unity.Mathematics;
using UnityEngine;

using SimpleRVO;

namespace RTSDemo.Unit
{
    public class UnitRTSMovement : UnitMovement
    {
        [SerializeField] private float slerpSpeed = 7;
        private int agentID;

        private const float STOP_DIST = 0.1f;

        public override void Init(Rigidbody2D _rigid)
        {
            base.Init(_rigid);
            InitRVO();
        }
        void OnDestroy()
        {
            if (!RVOSimManager.IsValid())
                return;
            RVOSimManager.EnsureCompleted();
            RVOSimManager.RemoveAgent(this.agentID);
            this.agentID = default;
        }
        void InitRVO()
        {
            RVOSimManager.EnsureCompleted();
            var position = new float2(this.transform.position.x, this.transform.position.y);
            this.agentID = RVOSimManager.AddAgent(position);
            RVOSimManager.SetAgentMaxSpeed(this.agentID, moveSpeed);
            RVOSimManager.SetAgentRadius(this.agentID, GetComponent<CircleCollider2D>().radius);
        }
        protected override Vector2 CalculateMovement(Vector2 currentPos)
        {
            float2 goalVector = velocityVector;
            if (math.lengthsq(goalVector) > STOP_DIST * STOP_DIST)
            {
                goalVector = math.normalize(goalVector);
                goalVector += (float2)UnityEngine.Random.insideUnitCircle * 0.001f;
            }
            RVOSimManager.SetAgentPrefVelocity(this.agentID, goalVector * 0.1f);
            return RVOSimManager.GetAgentPos(this.agentID);
        }
        public override void SetMoveSpeed(float speed)
        {
            base.SetMoveSpeed(speed);
            RVOSimManager.SetAgentMaxSpeed(this.agentID, speed);
        }
    }
}
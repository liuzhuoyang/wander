namespace SimpleRVO
{
    using Unity.Mathematics;
    using UnityEngine;
    
    internal class RVOAgent : MonoBehaviour
    {
        private float maxSpeed = 0.5f;
        private int agentID;

        private const float STOP_DIST = 5f;

        private void OnEnable()
        {
            var position = new float2(this.transform.position.x, this.transform.position.y);
            this.agentID = RVOSimManager.AddAgent(position);
            RVOSimManager.SetAgentMaxSpeed(this.agentID, this.maxSpeed);

            var radius = 0.5f * this.transform.localScale.x;
            RVOSimManager.SetAgentRadius(this.agentID, radius);
        }

        private void OnDisable()
        {
            if (!RVOSimManager.IsValid())
                return;
            RVOSimManager.RemoveAgent(this.agentID);
            this.agentID = default;
        }

        private void SetPreferredVelocities(float2 newGoal)
        {
            float2 goalVector = newGoal - RVOSimManager.GetAgentPos(this.agentID);

            if (math.lengthsq(goalVector) > STOP_DIST * STOP_DIST)
            {
                goalVector = math.normalize(goalVector);
                goalVector += (float2)UnityEngine.Random.insideUnitCircle * 0.001f;
            }

            RVOSimManager.SetAgentPrefVelocity(this.agentID, goalVector);
        }
    }
}
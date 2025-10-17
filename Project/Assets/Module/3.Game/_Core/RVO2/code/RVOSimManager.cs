namespace SimpleRVO
{
    using Unity.Mathematics;
    using UnityEngine;
    using RVO;
    using System.Collections.Generic;

    internal class RVOSimManager : Singleton<RVOSimManager>
    {
        [SerializeField] private float SimFreq = 60;
        [SerializeField] private float neighborDist = 5f;
        [SerializeField] private int maxNeighbors = 10;
        [SerializeField] private float timeHorizon = 7f;
        [SerializeField] private float timeHorizonObst = 2f;
        private Simulator simulator
        {
            get
            {
                if (_simulator == null)
                {
                    _simulator = new Simulator();
                    _simulator.SetTimeStep(1 / SimFreq);
                    _simulator.SetAgentDefaults(neighborDist, maxNeighbors, timeHorizon, timeHorizonObst, 1, 1, new float2(0.0f, 0.0f));
                }
                return _simulator;
            }
        }
        private Simulator _simulator;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            simulator.Clear();
            simulator.Dispose();
        }
        private void Update()
        {
            simulator.DoStep();
        }
        private void LateUpdate()
        {
            simulator.EnsureCompleted();
        }

        public static bool IsValid() => Instance != null && Instance.simulator != null;

        #region Agent Operation
        public static void EnsureCompleted() => Instance.simulator.EnsureCompleted();
        public static int AddAgent(float2 agentPos) => Instance.simulator.AddAgent(agentPos);
        public static void RemoveAgent(int agentID) => Instance.simulator.RemoveAgent(agentID);
        public static void SetAgentMaxSpeed(int agentID, float maxSpeed) => Instance.simulator.SetAgentMaxSpeed(agentID, maxSpeed);
        public static void SetAgentRadius(int agentID, float radius) => Instance.simulator.SetAgentRadius(agentID, radius);
        public static void SetAgentPrefVelocity(int agentID, float2 prefVel) => Instance.simulator.SetAgentPrefVelocity(agentID, prefVel);
        public static float2 GetAgentPos(int agentID) => Instance.simulator.GetAgentPosition(agentID);
        #endregion

        #region Obstacle Operation
        public static void RemoveObstacle(int obstacleID) => Instance.simulator.RemoveObstacle(obstacleID);
        public static int AddObstacle(float2[] vertices) => Instance.simulator.AddObstacle(vertices); 
        public static int AddObstacle(List<float2> vertices) => Instance.simulator.AddObstacle(vertices);
        #endregion

        void OnGUI()
        {
            if (simulator == null)
                return;
            GUILayout.Label($"Agents:{this.simulator.GetNumAgents()}");
            GUILayout.Label($"FPS:{1f / Time.deltaTime}");
        }
    }
}
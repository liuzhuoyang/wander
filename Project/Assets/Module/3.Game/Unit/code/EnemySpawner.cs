using System.Collections.Generic;
using RTSDemo.Unit;
using UnityEngine;

namespace RTSDemo.Spawn
{
    [System.Flags]
    public enum SpawnArea
    {
        None = 0,
        Top = 1 << 0,
        Bottom = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        All = Top | Bottom | Left | Right
    }
    public enum SpawnRate
    {
        Slow = 1,
        Normal = 2,
        Fast = 3
    }
    [System.Serializable]
    public class EnemySpawnHandler
    {
        private EnemySpawnData enemySpawnData;
        private EnemySpawner parent;
        [SerializeField] private float spawnCycle = 0f;
        [SerializeField] private int spawnedCount = 0;
        [SerializeField] private float spawnTimer = 0;

        internal bool spawnComplete{ get; private set; } = false;

        internal void Initialize(EnemySpawner parent, EnemySpawnData spawnData)
        {
            this.parent = parent;
            enemySpawnData = spawnData;
            if (enemySpawnData.baseCount > 1)
            {
                spawnedCount = enemySpawnData.baseCount + (parent.currentWave - 1) + (Random.value > 0.5f ? 1 : -1);
            }
            else
            {
                spawnedCount = enemySpawnData.baseCount;
            }
            spawnCycle = spawnData.delay;
            spawnTimer = 0;
        }
        internal void UpdateHandler(float dt)
        {
            if (spawnComplete)
                return;
            spawnTimer += dt;
            if (spawnTimer >= spawnCycle)
            {
                spawnTimer = 0;
                spawnCycle = GetSpawnCycle(enemySpawnData.spawnRate);
                UnitManager.Instance.CreateUnit(enemySpawnData.unitName, parent.GetSpawnPosition(enemySpawnData.spawnArea), true, 1);
                spawnedCount--;
                if(spawnedCount <= 0)
                {
                    spawnComplete = true;
                }
            }
        }
        static float GetSpawnCycle(SpawnRate spawnRate)
        {
            switch (spawnRate)
            {
                case SpawnRate.Slow: return Random.Range(0.5f, 1f);
                case SpawnRate.Normal: return Random.Range(0.2f, 0.5f);
                case SpawnRate.Fast: return Random.Range(0f, 0.1f);
                default: return 1f;
            }
        }
    }
    public class EnemySpawner : BattleBehaviour
    {
        [SerializeField] private List<EnemySpawnHandler> listCurrentHandlers;
        [SerializeField] private float spawnRadius = 8f;
        [SerializeField] private float ellipseFactor = 1.67f;
        private bool isSpawning = false;
        public int currentWave{get; private set;}
        public int maxWave{get; private set;}

        //在载入关卡数据后执行一次初始化
        public void StartBattle(LevelData levelData)
        {
            currentWave = 0;
            maxWave = levelData.totalWave;
            listCurrentHandlers = new List<EnemySpawnHandler>();
            foreach (var spawnData in levelData.enemyUnitAssetList)
            {
                var handler = new EnemySpawnHandler();
                handler.Initialize(this, spawnData);
                listCurrentHandlers.Add(handler);
            }
        }
        public void EndBattle()
        {
            currentWave = 0;
            maxWave = 0;
            listCurrentHandlers.Clear();
        }
        //在每一波开始时打开敌人生成器
        public void StartFight(int wave)
        {
            currentWave = wave;
            isSpawning = true;
        }
        //在每一波结束时关闭敌人生成器
        public void EndFight()
        {
            isSpawning = false;
        }
        //在每一帧更新中调用
        public override void BattleUpdate()
        {
            if(!isSpawning)
                return;            
            foreach (var handler in listCurrentHandlers)
            {
                if (handler.spawnComplete)
                    continue;
                handler.UpdateHandler(Time.deltaTime);
            }
        }
        public Vector2 GetSpawnPosition(SpawnArea spawnArea)
        {
            return GeometryUtil.GetEllipsePointPos(Vector2.zero, Random.Range(0, 360), spawnRadius, spawnRadius*ellipseFactor);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var matrix = new Matrix4x4();
            matrix.SetTRS(Vector3.zero, Quaternion.identity, new Vector3(1, ellipseFactor, 1));
            Gizmos.matrix = matrix;
            Gizmos.DrawWireSphere(Vector2.zero, spawnRadius);
        }
    }
}

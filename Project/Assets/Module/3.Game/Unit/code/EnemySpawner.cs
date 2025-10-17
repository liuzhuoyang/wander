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

    public class EnemySpawner : BattleBehaviour
    {
        [SerializeField] private List<EnemySpawnHandler> listCurrentHandlers;
        [SerializeField] private float spawnRadius = 8f;
        [SerializeField] private float ellipseFactor = 1.67f;
        private bool isSpawning = false;
        public int currentWave{get; private set;}
        public int maxWave { get; private set; }

        //在载入关卡数据后执行一次初始化
        public void InitSpawner(LevelData levelData)
        {
            currentWave = 0;
            maxWave = levelData.totalWave;
            listCurrentHandlers = new List<EnemySpawnHandler>();
            foreach (var spawnData in levelData.enemyUnitAssetList)
            {
                var handler = new EnemySpawnHandler(this, spawnData);
                listCurrentHandlers.Add(handler);
            }
        }
        public void CleanUpSpawner()
        {
            currentWave = 0;
            maxWave = 0;
            listCurrentHandlers.Clear();
        }
        //在每一波开始时打开敌人生成器
        public void StartSpawning(int wave)
        {
            currentWave = wave;
            isSpawning = true;
            foreach (var handler in listCurrentHandlers)
            {
                handler.ResetHandler(currentWave);
                //如果是最后一波，单独处理
                if(currentWave==maxWave)
                    handler.HandleLastWave();
            }
        }
        //在每一波结束时关闭敌人生成器
        public void StopSpawning()
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
            switch (spawnArea)
            {
                case SpawnArea.Top:
                    return GeometryUtil.GetEllipsePointPos(Vector2.zero, Random.Range(180, 360) * Mathf.Deg2Rad, spawnRadius, spawnRadius * ellipseFactor);
                case SpawnArea.Bottom:
                    return GeometryUtil.GetEllipsePointPos(Vector2.zero, Random.Range(0, 180) *Mathf.Deg2Rad, spawnRadius, spawnRadius * ellipseFactor);
                case SpawnArea.Left:
                    return GeometryUtil.GetEllipsePointPos(Vector2.zero, Random.Range(90, 270) *Mathf.Deg2Rad, spawnRadius, spawnRadius * ellipseFactor);
                case SpawnArea.Right:
                    return GeometryUtil.GetEllipsePointPos(Vector2.zero, Random.Range(-90, 90) * Mathf.Deg2Rad, spawnRadius, spawnRadius * ellipseFactor);
                default:
                    return GeometryUtil.GetEllipsePointPos(Vector2.zero, Random.Range(0, 360)*Mathf.Deg2Rad, spawnRadius, spawnRadius*ellipseFactor);
            }            
        }
        public static float GetSpawnCycle(SpawnRate spawnRate)
        {
            switch (spawnRate)
            {
                case SpawnRate.Slow: return Random.Range(0.5f, 1f);
                case SpawnRate.Normal: return Random.Range(0.2f, 0.5f);
                case SpawnRate.Fast: return Random.Range(0f, 0.1f);
                default: return 1f;
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var matrix = new Matrix4x4();
            matrix.SetTRS(Vector3.zero, Quaternion.identity, new Vector3(1, ellipseFactor, 1));
            Gizmos.matrix = matrix;
            Gizmos.DrawWireSphere(Vector2.zero, spawnRadius);
        }
        [System.Serializable]
        public class EnemySpawnHandler
        {
            private readonly EnemySpawnData enemySpawnData;
            private readonly EnemySpawner parent;
            private readonly Dictionary<SpawnArea, Vector2> validAreas;
            private readonly float totalWeight = 0f;
            [SerializeField] private int nextSpawnWave = 0;
            [SerializeField] private float spawnCycle = 0f;
            [SerializeField] private int spawnedCount = 0;
            [SerializeField] private float spawnTimer = 0;

            internal bool spawnComplete{ get; private set; } = false;

            public EnemySpawnHandler(EnemySpawner parent, EnemySpawnData spawnData)
            {
                this.parent = parent;
                enemySpawnData = spawnData;
                nextSpawnWave = spawnData.startWave;
                //初始化加权spawnArea
                var spawnArea = enemySpawnData.spawnArea;
                validAreas = new Dictionary<SpawnArea, Vector2>();
                totalWeight = 0f;
                if ((spawnArea & SpawnArea.Top) != 0)
                {
                    validAreas.Add(SpawnArea.Top, new Vector2(totalWeight, totalWeight+1f));
                    totalWeight += 1f;
                }
                if ((spawnArea & SpawnArea.Bottom) != 0)
                {
                    validAreas.Add(SpawnArea.Bottom, new Vector2(totalWeight, totalWeight+1f));
                    totalWeight += 1f;
                }
                if ((spawnArea & SpawnArea.Left) != 0)
                {
                    validAreas.Add(SpawnArea.Left, new Vector2(totalWeight, totalWeight+0.5f));
                    totalWeight += 0.5f;
                }
                if ((spawnArea & SpawnArea.Right) != 0)
                {
                    validAreas.Add(SpawnArea.Right, new Vector2(totalWeight, totalWeight+0.5f));
                    totalWeight += 0.5f;
                }

                ResetHandler(0);
            }
            internal void ResetHandler(int wave)
            {
                if (enemySpawnData.baseCount > 1)
                    spawnedCount = enemySpawnData.baseCount + (parent.currentWave - 1) + (Random.value > 0.5f ? 1 : -1);
                else
                    spawnedCount = enemySpawnData.baseCount;
                spawnCycle = enemySpawnData.delay;

                if (wave != nextSpawnWave)
                    spawnComplete = true;
                else
                {
                    spawnComplete = false;
                    nextSpawnWave += Random.Range(enemySpawnData.waveIntersect.x, enemySpawnData.waveIntersect.y + 1);
                }
                spawnTimer = 0;
            }
            internal void HandleLastWave()
            {
                if(enemySpawnData.forceLastWaveSpawn)
                {
                    ResetHandler(nextSpawnWave);
                }
            }
            internal void UpdateHandler(float dt)
            {
                if (spawnComplete)
                    return;
                spawnTimer += dt;
                if (spawnTimer >= spawnCycle)
                {
                    //找出加权后落在区间内的生成区域
                    var spawnFactor = Random.value * totalWeight;
                    var selectedArea = SpawnArea.None;
                    foreach (var area in validAreas)
                    {
                        if (spawnFactor >= area.Value.x && spawnFactor < area.Value.y)
                        {
                            selectedArea = area.Key;
                            break;
                        }
                    }
                    //生成单位
                    UnitManager.Instance.CreateUnit(enemySpawnData.unitName, parent.GetSpawnPosition(selectedArea), true, 1);
                    //重置计数
                    spawnTimer = 0;
                    spawnedCount--;
                    spawnCycle = EnemySpawner.GetSpawnCycle(enemySpawnData.spawnRate);
                    //判断是否结束
                    if(spawnedCount <= 0)
                    {
                        spawnComplete = true;
                    }
                }
            }
        }
    }
}

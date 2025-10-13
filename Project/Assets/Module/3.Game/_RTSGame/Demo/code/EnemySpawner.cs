using RTSDemo.Unit;
using UnityEngine;

namespace RTSDemo.Level
{ 
    public class EnemySpawner : BattleBehaviour
    {
        [SerializeField] private UnitData spawnUnitData;
        [SerializeField] private Vector2Int countPerSpawn;
        [SerializeField] private int maxCount = 50;
        [SerializeField] private float spawnFreq = 1f;
        [SerializeField] private float spawnRadius = 1f;

        private int count = 0;
        private float timer = 0;

        void OnEnable()
        {
            count = 0;
            timer = 0;
        }
        // Update is called once per frame
        public override void BattleUpdate()
        {
            timer += Time.deltaTime * spawnFreq;
            if (timer >= 1)
            {
                timer = 0;
                int spawnCount = countPerSpawn.GetRndValueInVector2Range();
                spawnCount = Mathf.Min(spawnCount, maxCount - count);
                count += spawnCount;
                for (int i = 0; i < spawnCount; i++)
                {
                    Vector2 rndPos = Random.insideUnitCircle * spawnRadius;
                    UnitManager.Instance.CreateUnit(spawnUnitData.m_actorKey, transform.position + new Vector3(rndPos.x, rndPos.y, 0), true, 1);
                }
                if (count >= maxCount)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

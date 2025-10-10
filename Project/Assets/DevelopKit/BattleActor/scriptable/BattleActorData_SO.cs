using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BattleActor
{
    public abstract class BattleActorData_SO : ScriptableObject
    {
        public abstract BattleActorType actorType{ get; }
        [BoxGroup("基础数值")] public int maxLevel = 1000;
        [BoxGroup("基础数值")] public Vector2 healthRange = new Vector2(100, 1000);
        [BoxGroup("基础数值")] public Vector2 shieldRange = new Vector2(0, 0);

        [TabGroup("战斗参数")] public bool penetrateArmor = false;
        [TabGroup("战斗参数"), ShowIf("IsRange")] public float attackRange = 2f;
        [TabGroup("战斗参数")] public ElementType damageType = ElementType.Physical;
        [TabGroup("战斗参数")] public Vector2 attackDamage = new Vector2(10, 100); //伤害范围
        [TabGroup("战斗参数")] public float damageMultiToBuilding = 0;
        [TabGroup("战斗参数")] public float damageMultiToShield = 0;
        [TabGroup("战斗参数")] public float criticRate; //暴击率
        [TabGroup("战斗参数")] public float criticDamageMultiplier; //暴击伤害倍率
        [TabGroup("战斗参数")] public float attackSpeed = 1;//攻击速度
        public string m_actorKey => this.name;

        protected virtual bool IsRange() => true;
    }
}
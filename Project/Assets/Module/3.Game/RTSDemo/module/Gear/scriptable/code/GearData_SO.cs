using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using BattleMap.Grid;
using SimpleVFXSystem;
using SimpleAudioSystem;

namespace BattleGear
{
    public abstract class GearData_SO : ScriptableObject
    {
        public abstract GearType gearType{get;}

        [BoxGroup("武器形状参数")] public GridShape gearShape;
        [BoxGroup("武器形状参数")] public Vector2 placeOffset = Vector2.zero;

        [BoxGroup("武器参数"), PropertyTooltip("1系代表发射速度，2系代表召唤速度")] public float attackSpeed = 1;
        [BoxGroup("武器参数"), PropertyTooltip("1系代表索敌范围，2系代表召唤半径")] public float attackRange = 10;

        [TabGroup("攻击参数")] public ElementType damageElement;
        [TabGroup("攻击参数")] public bool penetrateArmor;
        [TabGroup("攻击参数")] public float baseDamage;
        [TabGroup("攻击参数")] public float damageMultiToBuilding;
        [TabGroup("攻击参数")] public float damageMultiToShield;
        [TabGroup("攻击参数")] public float criticRate;
        [TabGroup("攻击参数")] public float criticDamageMultiplier;

        [TabGroup("武器素材")] public AssetReference gearPrefab;
        [BoxGroup("武器技能")] public GearAbilityData_SO[] gearAbilites;
        
        [TabGroup("武器表现")] public VFXData_SO vfx_gearBeginFire;
        [TabGroup("武器表现")] public AudioData_SO sfx_gearBeginFire;
        public string m_gearKey => this.name;
    }
}
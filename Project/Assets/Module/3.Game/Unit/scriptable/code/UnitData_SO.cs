using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;
using BattleLaunch.Bullet;

namespace BattleActor.Unit
{
    [CreateAssetMenu(fileName = "UnitData_SO", menuName = "RTS_Demo/Actor/Unit/UnitData_SO")]
    public class UnitData_SO : BattleActorData_SO
    {
        public override BattleActorType actorType => BattleActorType.Unit;
        #region 基础数值
        [BoxGroup("基础数值")] public UnitRace unitRace = UnitRace.None;
        [BoxGroup("基础数值")] public Vector2 armorRange = new Vector2(0, 0);
        #endregion

        [TabGroup("移动参数")] public BattleActorMotionLayer motionLayer = BattleActorMotionLayer.Ground;
        [TabGroup("移动参数")] public float moveSpeed = 3;

        #region 战斗参数
        [TabGroup("战斗参数")] public BattleActorMotionLayerMask attackLayer; //攻击的层级
        [TabGroup("战斗参数")] public bool isRange = true;
        [TabGroup("战斗参数")] public int maxAmmo;//最大弹药量
        [TabGroup("战斗参数")] public float reloadSpeed;//重新装填的速度
        #endregion

        #region 单位配置
        [TabGroup("单位素材"), SerializeField] private AssetReference bodyRef;
        [TabGroup("单位素材"), SerializeField] private BulletData_SO bulletData;
        #endregion

        #region 技能配置
        [BoxGroup("技能配置"), SerializeField] private UnitAbilityData_SO[] unitAbilites;
        #endregion

        public AssetReference m_bodyRef => bodyRef;
        public BulletData_SO m_bulletData => bulletData;
        public UnitAbilityData_SO[] m_unitAbilites => unitAbilites;
        protected override bool IsRange() => isRange;
    }
}
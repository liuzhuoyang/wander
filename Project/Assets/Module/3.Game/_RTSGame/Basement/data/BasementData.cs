using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BattleActor.Basement
{
    using Skill;
    [CreateAssetMenu(fileName = "BasementData", menuName = "RTS_Demo/Actor/Basement/BasementData", order = 1)]
    public class BasementData : ScriptableObject
    {
        [BoxGroup("基地基础数值")] public int maxHealth = 1000;
        [BoxGroup("基地基础数值")] public int maxShield = 1000;
        [BoxGroup("基地基础数值")] public int maxMana = 100;
        [TabGroup("墙体素材")] public AssetReference wallStraightRef;
        [TabGroup("墙体素材")] public AssetReference wallCornerConnectSpriteRef;
        [TabGroup("墙体素材")] public AssetReference wallCornerRef;
        [TabGroup("基地技能")] public BasementSkillData[] basementAbilities;
        public string m_basementKey => this.name;
    }
}
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RTSDemo.Basement
{
    using Skill;
    [CreateAssetMenu(fileName = "BasementData", menuName = "RTS_Demo/Actor/Basement/BasementData", order = 1)]
    public class BasementData : ScriptableObject
    {
        [BoxGroup("基地基础数值")] public int maxHealth = 1000;
        [BoxGroup("基地基础数值")] public int maxShield = 1000;
        [TabGroup("基地素材")] public AssetReferenceGameObject basementPrefab;
        [TabGroup("基地技能")] public BasementSkillData[] basementAbilities;
        public string m_basementKey => this.name;
    }
}
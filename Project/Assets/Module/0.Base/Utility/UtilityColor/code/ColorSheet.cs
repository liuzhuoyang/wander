using UnityEngine;

[CreateAssetMenu(fileName = "new ColorSheet", menuName = "Asset/ColorSheet")]
public class ColorSheet : ScriptableObject
{

[Header("文本强调颜色")]
    [SerializeField] private Color textEmphasisColorGreen;

[Header("稀有度")]
    [SerializeField] private RarityColors rariyColors;
[Header("伤害")]
    [SerializeField] private DamageColors damageColors;
    [SerializeField] private Color criticDamageColor;
[Header("血条")]
    [SerializeField] private HealthBarColors healthBarColors;
[Header("队伍颜色")]
    [SerializeField] private Color FriendlyColor;
    [SerializeField] private Color EnemyColor;
    [SerializeField] private Color FriendlyTrailColor;
    [SerializeField] private Color EnemyTrailColor;
    
[Header("战力颜色")]
    [SerializeField] private Color powerOver;
    [SerializeField] private Color powerUnder;

[Header("属性颜色")]
    //未知颜色表示很厉害的敌人
    [SerializeField] private Color attributeEnemyUnknown;
    [SerializeField] private Color attributeEnemyVeryLow;
    [SerializeField] private Color attributeEnemyLow;
    [SerializeField] private Color attributeEnemyNormal;
    [SerializeField] private Color attrubuteEnemyHigh;
    [SerializeField] private Color attributeEnemyVeryHigh;

    public Color GetTextEmphasisColorGreen() => textEmphasisColorGreen;
    public Color GetDamageColor(DamageType damageType) => damageColors.GetDamageColor(damageType);
    public Color GetRarityColor(Rarity rarity) => rariyColors.GetRarityColor(rarity);
    public Color GetCriticDamageColor()=>criticDamageColor;
    public Color GetHealthBarColor(int healthBarCount) => healthBarColors.GetHealthBarColor(healthBarCount);
    public Color GetTeamColor(bool isPlayerSide) => isPlayerSide?FriendlyColor:EnemyColor;
    public Color GetTeamTrailColor(bool isPlayerSide) => isPlayerSide?FriendlyTrailColor:EnemyTrailColor;
    public Color GetAttributeValueColor(AttributeValueType attributeValueType)
    {
        return attributeValueType switch
        {
            AttributeValueType.Unknown=>attributeEnemyUnknown,
            AttributeValueType.VeryLow=>attributeEnemyVeryLow,
            AttributeValueType.Low=>attributeEnemyLow,
            AttributeValueType.Normal=>attributeEnemyNormal,
            AttributeValueType.High=>attrubuteEnemyHigh,
            AttributeValueType.VeryHigh=>attributeEnemyVeryHigh,
            _=>attributeEnemyUnknown,
        };
    }
    public int GetLoopIndexForHealth(int index) => healthBarColors.GetLoopedHealthIndex(index);

    public Color GetPowerColor(bool isOver) => isOver ? powerOver : powerUnder;

    [System.Serializable]
    public struct RarityColors
    {
        public Color colorRarityCommon;
        public Color colorRarityUncommon;
        public Color colorRarityRare;
        public Color colorRarityEpic;
        public Color colorRarityLegendary;
        public Color colorRarityMythic;
        public Color colorRarityArcane;
        public Color GetRarityColor(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common=>colorRarityCommon,
                Rarity.Rare=>colorRarityRare,
                Rarity.Epic=>colorRarityEpic,
                Rarity.Legendary=>colorRarityLegendary,
                Rarity.Mythic=>colorRarityMythic,
                Rarity.Arcane=>colorRarityArcane,
                _=>colorRarityCommon,
            };
        }
    }
    [System.Serializable]
    public struct DamageColors
    {
        public Color colorDamagePhysical;
        public Color colorDamageCryo;
        public Color colorDamageElectro;
        public Color colorDamageThermal;
        public Color colorDamageBiochemical;
        public Color GetDamageColor(DamageType damageType)
        {
            return damageType switch
            {
                DamageType.Physical => colorDamagePhysical,
                DamageType.Cryo => colorDamageCryo,
                DamageType.Electro => colorDamageElectro,
                DamageType.Thermal => colorDamageThermal,
                DamageType.Biochemical => colorDamageBiochemical,
                _ => colorDamagePhysical,
            };
        }
    }
    [System.Serializable]
    public struct HealthBarColors
    {
        public Color[] healthCountToColors;
        public Color GetHealthBarColor(int healthBarCount)
        {
            healthBarCount = Mathf.Max(0, healthBarCount);
            healthBarCount = Mathf.Min(healthBarCount, healthCountToColors.Length - 1);
            return healthCountToColors[healthBarCount];
        }
        public int GetLoopedHealthIndex(int index) => index % healthCountToColors.Length;
    }
}

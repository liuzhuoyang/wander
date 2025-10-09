using UnityEngine;
public class UtilityColor : Singleton<UtilityColor>
{
    [SerializeField] private ColorSheet colorSheet;

    //暴击伤害颜色
    public Color GetCriticDamageColor() => colorSheet.GetCriticDamageColor();
    //伤害跳字颜色
    public Color GetDamageColor(DamageType damageType) => colorSheet.GetDamageColor(damageType);
    //稀有度颜色
    public Color GetRarityColor(Rarity rarity) => colorSheet.GetRarityColor(rarity);
    //获取血量条颜色
    public Color GetHealthBarColor(int healthBarCount) => colorSheet.GetHealthBarColor(healthBarCount);
    //获取队伍颜色
    public Color GetTeamColor(bool isPlayerSide) => colorSheet.GetTeamColor(isPlayerSide);
    //获取队伍尾焰Trail颜色
    public Color GetTeamTrailColor(bool isPlayerSide) => colorSheet.GetTeamTrailColor(isPlayerSide);
    //获取属性值颜色
    public Color GetAttributeValueColor(AttributeValueType attributeValueType) => colorSheet.GetAttributeValueColor(attributeValueType);
    //获取战力颜色
    public Color GetPowerColor(bool isOver) => colorSheet.GetPowerColor(isOver);
    public int LoopHealthBarIndex(int index) => colorSheet.GetLoopIndexForHealth(index);
    //为rich text里字符添加颜色
    public string GetStringRarityColor(string content, Rarity rarity)
    {
        string htmlColor = ColorUtility.ToHtmlStringRGB(GetRarityColor(rarity));
        return $"<color=#{htmlColor}>{content}</color>";
    }

    //获取文本强调颜色
    public Color GetTextEmphasisColorGreen() => colorSheet.GetTextEmphasisColorGreen();
    
}

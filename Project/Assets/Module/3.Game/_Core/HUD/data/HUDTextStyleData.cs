using UnityEngine;

[CreateAssetMenu(fileName = "HUDTextStyle", menuName = "Assets/UI/HUDTextStyle")]
public class HUDTextStyleData : ScriptableObject
{
    [Space(20)]
    [SerializeField] private float textFlyDistance = -1f;
    [SerializeField] private float textFlyDuration = 0.8f;
    [SerializeField] private float textFadeDuration = 0.6f;
    [SerializeField] private float textFadeDelay = 0.4f;
    public HUD_TextStyle GetCurrentStyle()
    {
        HUD_TextStyle style = new HUD_TextStyle(
            textFlyDistance,
            textFlyDuration,
            textFadeDuration ,
            textFadeDelay,
            textFadeDuration
        );
        return style;
    }
}
public struct HUD_TextStyle
{
    public float textDistance;
    public float textBounceDuration;
    public float textFlyDuration;
    public float textFadeDuration;
    public float textFadeDelay;

    public static readonly HUD_TextStyle Default_Style = new HUD_TextStyle(-1, 0.8f, 0.3f, 0.4f, 0.6f);
    
    public HUD_TextStyle(float textDistance, float textFlyDuration, float textBounceDuration, float textFadeDuration, float textFadeDelay)
    {
        this.textDistance = textDistance;
        this.textFlyDuration = textFlyDuration;
        this.textBounceDuration = textBounceDuration;
        this.textFadeDuration = textFadeDuration;
        this.textFadeDelay = textFadeDelay;
    }
}

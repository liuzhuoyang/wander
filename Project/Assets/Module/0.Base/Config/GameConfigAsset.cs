using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Onicore/Config/GameConfig", order = 1)]
public class GameConfigAsset : ScriptableObject
{

    public ProductMode productMode;

    public DebugTool debugTool;

    public PlatformType targetPlatform;

    [Space(20)]
    public AnalyticsTool isAmpOn;
    public string amplitudeAppKey;

    [Space(20)]
    public string privacyPolicyUrl;
    public string termsOfServiceUrl;
    public string supportEmail;

    [Space(20)]
    public int screenshotSizeMultiplier = 1;

    void OnValidate()
    {
        if(productMode == ProductMode.Release)
        {
            debugTool = DebugTool.Off;
        }
    }
}

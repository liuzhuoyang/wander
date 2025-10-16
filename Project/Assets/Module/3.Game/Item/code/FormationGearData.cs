using UnityEngine;
using Sirenix.OdinInspector;
using BattleGear;
using SimpleVFXSystem;
using SimpleAudioSystem;
using BattleLaunch;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "formation_gear_", menuName = "Formation/FormationGearData")]
public class FormationGearData : FormationItemData
{
    FormationGearData()
    {
        itemType = FormationItemType.Gear;
    }

    public GearData gearData;

    [BoxGroup("参数")]
    [LabelText("金币消耗")]
    public int coinCost = 0;


    public Sprite itemIcon;


    [BoxGroup("武器技能")] public GearAbilityData[] gearAbilites;

    [BoxGroup("武器表现")] public VFXData vfx_gearBeginFire;
    [BoxGroup("武器表现")] public AudioData sfx_gearBeginFire;
    public string m_gearKey => this.name;

    [BoxGroup("发射参数"), PreviouslySerializedAs("launchConfig_SO")] public LaunchConfig launchConfig;
}

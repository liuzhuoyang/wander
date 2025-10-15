using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "formation_skill_", menuName = "Formation/FormationSkillData")]
public class FormationSkillData : FormationItemData
{
    FormationSkillData()
    {
        itemType = FormationItemType.Skill;
    }
    [BoxGroup("参数"), LabelText("是否允许升级")]
    public bool canUpgrade = false;

    [BoxGroup("参数"), LabelText("最高等级")]
    [ShowIf("canUpgrade")]
    public int maxLevel = 1;

    [BoxGroup("参数"), LabelText("稀有度")]
    public Rarity rarity = Rarity.Common;


    [BoxGroup("参数")]
    [LabelText("是否通过广告获取")]
    public bool AdGet = false;


    [BoxGroup("参数")]
    [LabelText("金币消耗")]
    [HideIf("AdGet")]
    public int coinCost = 0;



    [BoxGroup("参数")]
    [LabelText("显示名称（本地化，没整理完,之后写）")]
    [ReadOnly]
    public string displayName;

    [BoxGroup("参数")]
    [LabelText("描述（本地化，没整理完,之后写）")]
    [ReadOnly]
    public string info;


    [BoxGroup("技能参数")]
    public EffectType effectType;
    [BoxGroup("技能参数")]
    [LabelText("需要充能的次数")]
    public int requiredChargeCount = 1;

    [BoxGroup("技能参数")]
    [ReadOnly]
    [LabelText("消耗能量（大于0时，技能充能完毕后，会消耗能量，此概念暂时不需要管）")]
    public int energyConsumption = 0;

    [BoxGroup("技能参数")]
    [LabelText("是否需要冷却")]
    public bool hasCooldown = false;

    [BoxGroup("技能参数")]
    [LabelText("冷却时间")]
    [ShowIf("hasCooldown")]
    public float cooldownTime = 0f;

    [BoxGroup("技能参数")]
    [LabelText("值列表（(临时，暂时没想好怎么处理这块，先运行)）")]
    public List<float> value;

#if UNITY_EDITOR
    [Button("初始化数据")]
    public void InitData()
    {
        var underscoreIndex = this.name.IndexOf('_');
        if (underscoreIndex >= 0 && underscoreIndex < this.name.Length - 1)
        {
            itemName = this.name.Substring(underscoreIndex + 1);
        }
        else
        {
            itemName = this.name;
        }

    }
#endif
}

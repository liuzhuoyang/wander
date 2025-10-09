using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GearDataEditor
{
    [LabelText("等级")] public int level;

    [LabelText("属性")] public AttributeType attributeType;

    [LabelText("值")] public float value;
}

[CreateAssetMenu(fileName = "GearData", menuName = "OniData/System/Meta/Gear/GearData", order = 1)]
public class GearData : ScriptableObject
{
    [LabelText("装备序号")] public int gearIndex;

    [LabelText("品质")] public Rarity rarity;

    [LabelText("攻击力")] public int attack;

    [LabelText("触发圈速度")] public int triggerSpeed;

    [LabelText("属性解锁")] public List<GearDataEditor> listData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public void InitData()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable]
public class LootDataEditor
{
    [LabelText("品质")] public Rarity rarity;

    [LabelText("是否为碎片")] public bool isShard;

    [LabelText("概率")] public float probability;
}

[CreateAssetMenu(fileName = "LootData", menuName = "OniData/System/Meta/Loot/lootData", order = 1)]
public class LootData : ScriptableObject
{
    [LabelText("所属宝箱")] public int lootIndex;

    [LabelText("宝箱奖励")] public List<LootDataEditor> listData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public void InitData()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
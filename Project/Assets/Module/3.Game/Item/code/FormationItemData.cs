using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class FormationItemData : ScriptableObject
{
    [BoxGroup("参数")]
    [ReadOnly]
    public FormationItemType itemType;

    [BoxGroup("参数")]
    [LabelText("物品名称（自动生成，识别用）")]
    [ReadOnly]
    public string itemName;
}

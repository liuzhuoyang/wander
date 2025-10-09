using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "pin_node", menuName = "OniData/Generic/Pin/PinData", order = 1)]
public class PinData : ScriptableObject
{
    [OnValueChanged("OnInitData"), LabelText("父节点")] public PinData parentNode;
    [OnValueChanged("OnInitData"), LabelText("子节点")] public PinData[] childNodes;
    [LabelText("点击后解除")] public bool selfResolve;
    [ReadOnly, LabelText("根节点"), PropertyTooltip("当Pin节点是根节点，则只会根据子节点状况刷新自己")] public bool isRoot;
    [ReadOnly, LabelText("叶节点"), PropertyTooltip("当Pin节点是叶节点，则一定有判定条件")] public bool isLeaf;
    [Button("Init Data", ButtonSizes.Medium)]
    public void OnInitData()
    {
        isRoot = parentNode == null;
        isLeaf = childNodes == null || childNodes.Length == 0;
    }
}
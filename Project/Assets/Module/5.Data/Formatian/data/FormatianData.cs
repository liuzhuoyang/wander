using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using onicore.editor;
using System.Linq;
#endif


[CreateAssetMenu(fileName = "formatian_asset", menuName = "OniData/Data/Formatian/FormatianData", order = 1)]
public class FormatianData : ScriptableObject
{
    [ReadOnly]
    public string formatianName;

    [LabelText("法阵列表，先这样编辑，后续会写一个可视化编辑器，然后自动生成。（目前做完需要手动init一下，内部需要锁定下一个节点）")]
    public List<FormatianNodaData> listNodeData = new List<FormatianNodaData>();



#if UNITY_EDITOR
    [Button("初始化数据", ButtonSizes.Gigantic)]
    public void InitData()
    {
        formatianName = this.name;

        // 按照 index 从小到大排序
        if (listNodeData != null && listNodeData.Count > 0)
        {
            // 先排序
            listNodeData = listNodeData.OrderBy(node => node.index).ToList();
        }
    }
#endif

}


[Serializable]
public class FormatianNodaData
{
    public int index;
    public Vector2 position;
}
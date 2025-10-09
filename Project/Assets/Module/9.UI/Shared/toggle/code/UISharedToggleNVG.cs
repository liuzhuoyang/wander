using System.Collections.Generic;
using UnityEngine;

public class UISharedToggleNVG : MonoBehaviour
{
    [SerializeField] RectTransform rectToggle;
    [SerializeField] GameObject prefabToggle;

    List<SharedToggleNVGNode> listToggleNode;
    public void Init(int count)
    {
        foreach (Transform child in rectToggle)
        {
            Destroy(child.gameObject);
        }
        listToggleNode = new List<SharedToggleNVGNode>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefabToggle, rectToggle);
            obj.GetComponent<SharedToggleNVGNode>().Init(i);
            listToggleNode.Add(obj.GetComponent<SharedToggleNVGNode>());
        }
    }

    public void Refresh(int index)
    {
        foreach (var item in listToggleNode)
        {
            item.Refresh(index);
        }
    }
}

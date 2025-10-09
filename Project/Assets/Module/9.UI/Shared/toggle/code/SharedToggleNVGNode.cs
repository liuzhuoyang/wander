using UnityEngine;

public class SharedToggleNVGNode : MonoBehaviour
{
    [SerializeField] GameObject objOn;

    int nodeIndex;
    public void Init(int index)
    {
        nodeIndex = index;
    }

    public void Refresh(int index)
    {
        objOn.SetActive(nodeIndex == index);
    }
}
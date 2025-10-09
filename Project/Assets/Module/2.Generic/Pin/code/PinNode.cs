using System;
using System.Collections.Generic;

//PinNode，用于记录各个PinNode之间的父子关系，形成PinNode Map
public class PinNode
{
    public bool isPined; //Pin状态
    public bool selfResolve; //点击后自动解除
    public Action<bool> PinUI_Update;
    public PinNode parentNode; //父节点
    public HashSet<PinNode> childNodes; //所有子节点

    //当子节点切换时，检查自身是否满足切换状态条件
    void OnChildNodeUpdate(PinNode childNode)
    {
        if(childNode.isPined)
        {
            SetNewPinState(true);
        }
        else
        {
            bool newPin = false;
            foreach(PinNode node in childNodes)
            {
                if(node!=childNode && node.isPined)
                {
                    newPin = true;
                    SetNewPinState(newPin);
                    return;
                }
            }
            SetNewPinState(newPin);
        }
    }
    public void SetNewPinState(bool newPined)
    {
        PinUI_Update?.Invoke(newPined);
    //仅当有状态更新时，才通知父节点
        if(isPined!=newPined)
        {
            isPined = newPined;
            if(parentNode!=null)
            {
                parentNode.OnChildNodeUpdate(this);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

// 动态管理器
// 使用场景1：管理特效里经常出现的ui特效飞行需要的目标对象, 存储起来便于特效飞行时使用
public class UIDynamicControl : Singleton<UIDynamicControl>
{
    public Dictionary<string, Transform> uiTargetDict = new Dictionary<string, Transform>();

    // 添加动态管理对象，如特效里经常出现的ui特效飞行需要的目标对象
    public void AddUIDynamicTarget(string targetName, Transform target){
        if(uiTargetDict.ContainsKey(targetName)){
            uiTargetDict[targetName] = target;
        }else{
            uiTargetDict.Add(targetName, target);
        }
    }

    //通过动态管理类型获取目标点
    public Transform GetDynamicTarget(string targetName)
    {
        if(uiTargetDict.ContainsKey(targetName)){
            return uiTargetDict[targetName];
        }
        return null;
    }
}

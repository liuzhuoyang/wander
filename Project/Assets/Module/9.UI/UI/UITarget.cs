using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用户存储特效飞行目标，辅助播放飞行特效显示层和数据层获取目标位置
public class UITarget : Singleton<UITarget>
{
    public Transform targetSilver;

    public Transform targetCurrentPageFlyer;
}

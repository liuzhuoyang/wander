using System;
using System.Collections.Generic;
using UnityEngine;

public class EventNameVFX
{
    public const string EVENT_ON_VFX_UI = "OnVFXUI";
    public const string EVENT_ON_VFX_UI_FLYER = "OnUIVFXFlyerUI";
}

//批量创建UI飞行物VFX参数
public class UIVFXFlyerBatchArgs
{
    public Vector2 spawmPoint;
    public Vector3 targetPoint;
    public List<RewardArgs> listReward;
}

public class UIVFXFlyerArgs
{
    //物品名字
    public string rewardName;
    //生成位置
    public Vector2 spawmPos;
    //目标位置
    public Vector2 targetPos;
    //缩放
    public float scale;
    //延迟
    public float delay;
}

// public class UIVFXJumpArgs
// {
//     public string target;
//     public Vector2 pos;
//     public float scale;
//     public float force;
//     public float life;
//     public bool isWorldSpace;
// }

public class UIVFXArgs
{
    public string target;
    public float life;
    public float posX;
    public float posY;
}
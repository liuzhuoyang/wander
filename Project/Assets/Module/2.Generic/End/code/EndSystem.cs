using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EndSystem : Singleton<EndSystem>
{
    public void Init()
    {

    }

    public async void OnOpen(EndArgs args)
    {
        //恢复速度
        UtilityGameSpeed.OnDefaultGameSpeed();
        //TODO 处理战斗奖励，道具奖励，章节进度等等
    }

    public void OnClaim(int multiplier = 1, Action callback = null)
    {
        
    }
}

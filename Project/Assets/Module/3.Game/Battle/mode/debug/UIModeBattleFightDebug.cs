using System.Collections.Generic;
using RTSDemo.Unit;
using TMPro;
using UnityEngine;

public class UIModeBattleFightDebug : DebuggerSharedMenu
{
    public TMP_Dropdown dropGear;
    public void Init()
    {

    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnDebugFightEnd()
    {
        BattleSystem.Instance.OnChangeBattleState(BattleStates.FightEnd);
    }

    public void OnDebugBattleEndVictory()
    {
        BattleData.isVictory = true; //Debug假设满足胜利条件
        BattleSystem.Instance.OnBattleEnd();
    }

    public void OnDebugBattleEndDefeat()
    {
        BattleSystem.Instance.OnBattleEnd();
    }

    public void OnDebugSpawnEnemy()
    {
        UnitManager.Instance.CreateUnit("unit_zreep_melee_001", Random.insideUnitCircle * 5, true, 1);
    }
    public void OnDebugInvincible()
    {
        
    }
}

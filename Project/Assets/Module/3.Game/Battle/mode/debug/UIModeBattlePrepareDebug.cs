using UnityEngine;
using TMPro;
using RTSDemo.Basement;
using RTSDemo.Grid;

public class UIModeBattlePrepareDebug : DebuggerSharedMenu
{
    public TMP_InputField inputFieldWave;
    public GameObject map_node_target;
    public GameObject objDebugMenu;

    public void Init()
    {
       
    }

    public void OnDebugPrepareEnd()
    {
        BattleSystem.Instance.OnChangeBattleState(BattleStates.PrepareEnd);
    }

    public void OnDebugOpen()
    {
        objDebugMenu.SetActive(true);
    }

    public void OnDebugClose()
    {
        objDebugMenu.SetActive(false);
    }
    public void OnDebugCreateBasement(BasementData basementData)
    {
        // Vector2Int grid_zero = RTSGridWorldSystem.Instance.GetGridPointFromWorld(Vector2.zero);
        //创建基地
        // BasementControl.Instance.CreateBasement(basementData.m_basementKey, grid_zero, Vector2Int.one * 3);
        //创建目的点
        for(int i=0; i<9; i++)
        {
            RTSGridWorldSystem.Instance.GetNodeFromWorldPos(Vector2.zero+new Vector2(i%3,i/3)).SetCost(1);
        };
        Instantiate(map_node_target, Vector3.zero, Quaternion.identity);
        RTSGridWorldSystem.Instance.GetNodeFromWorldPos(Vector2.zero).SetCost(0);
        //刷新世界节点
        FindAnyObjectByType<RTSGridWorldBuilder>().RefreshGridWorld();
    }
}



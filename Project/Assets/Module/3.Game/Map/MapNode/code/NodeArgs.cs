using UnityEngine;
 
public class NodeArgs
{
    public Vector3 worldPos;
    public Vector2Int gridXY;
    public byte cost;
    public ushort bestCost;
    public NodeDirectionUtility bestDirection;
    
    public NodeArgs(Vector3 worldPos, Vector2Int gridXY)
    {
        this.worldPos = worldPos;
        this.gridXY = gridXY;
        cost = (byte)NodeDirectionUtility.NodeCostType.Normal;
        bestCost = ushort.MaxValue;
        bestDirection = NodeDirectionUtility.None;
    }
 
 /*
    public void IncreaseCost(int amnt)
    {
        if (cost == byte.MaxValue) { return; }
        if (amnt + cost >= 255) { cost = byte.MaxValue; }
        else { cost += (byte)amnt; }
    }*/

    public void SetCost(byte cost)
    {
        this.cost = cost;
    }

    public void ResetCost()
    {
        cost = (byte)NodeDirectionUtility.NodeCostType.Normal;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
//节点方向工具
public class NodeDirectionUtility
{
    public enum NodeCostType
    {
        None = -1,
        Target = 0, 
        Base = 1,   //基地
        Normal = 10, //普通
        Block = 255, //阻挡
    }

    public readonly Vector2Int Vector;
 
    private NodeDirectionUtility(int x, int y)
    {
        Vector = new Vector2Int(x, y);
    }
 
    public static implicit operator Vector2Int(NodeDirectionUtility direction)
    {
        return direction.Vector;
    }
    
    //根据Vector2Int获取方向
    public static NodeDirectionUtility GetDirectionFromVectorXY(Vector2Int vector)
    {
        return CardinalAndIntercardinalDirections.DefaultIfEmpty(None).FirstOrDefault(direction => direction == vector);
    }
 
    public static readonly NodeDirectionUtility None = new NodeDirectionUtility(0, 0);
    public static readonly NodeDirectionUtility North = new NodeDirectionUtility(0, 1);
    public static readonly NodeDirectionUtility South = new NodeDirectionUtility(0, -1);
    public static readonly NodeDirectionUtility East = new NodeDirectionUtility(1, 0);
    public static readonly NodeDirectionUtility West = new NodeDirectionUtility(-1, 0);
    public static readonly NodeDirectionUtility NorthEast = new NodeDirectionUtility(1, 1);
    public static readonly NodeDirectionUtility NorthWest = new NodeDirectionUtility(-1, 1);
    public static readonly NodeDirectionUtility SouthEast = new NodeDirectionUtility(1, -1);
    public static readonly NodeDirectionUtility SouthWest = new NodeDirectionUtility(-1, -1);
 
    public static readonly List<NodeDirectionUtility> CardinalDirections = new List<NodeDirectionUtility>
    {
        North,
        East,
        South,
        West
    };
 
    public static readonly List<NodeDirectionUtility> CardinalAndIntercardinalDirections = new List<NodeDirectionUtility>
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };
 
    public static readonly List<NodeDirectionUtility> AllDirections = new List<NodeDirectionUtility>
    {
        None,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };
    public static bool IsAdjacent(NodeDirectionUtility direction)
    {
        return direction == NorthEast || direction == SouthEast || direction == SouthWest || direction == NorthWest;
    }
}
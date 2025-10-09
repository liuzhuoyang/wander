using UnityEngine;
using TMPro;

public class MapNodeDebugView : MonoBehaviour
{
    public GameObject objArrow;
    public GameObject objStop;
    public GameObject objTarget;
    public GameObject objBlock;
    public GameObject objBase;
    
    public GameObject objGridSelector;

    public TextMeshProUGUI textCost;

    public TextMeshProUGUI textIntegration;

    public int x;
    public int y;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;

        objGridSelector.SetActive(false);
    }

    public void SetCost(int cost)
    {
        textCost.text = cost.ToString();
    }

    public void SetIntegration(int integration)
    {
        textIntegration.text = integration.ToString();
    }

    public void OnRefresh()
    {   
        NodeArgs args = FlowFieldControl.Instance.flowField.nodeArray[x, y];
        //MapNodeArgs args = MapNodeControl.Instance.GetNodeByXY(x, y);
        SetIntegration(args.bestCost);

        objArrow.SetActive(true);
        objStop.SetActive(false);
        objTarget.SetActive(false);
        objBlock.SetActive(false);
        objBase.SetActive(false);

        NodeArgs nodeArgs = FlowFieldControl.Instance.flowField.nodeArray[x,y]; //(this.transform.position);
        Vector3 moveDirection = new Vector3(nodeArgs.bestDirection.Vector.x, 0, nodeArgs.bestDirection.Vector.y);
        
        // 找到箭头对象
        Transform arrowTransform = transform.Find("arrow");
        
        Debug.Log("nodeArgs.cost: " + nodeArgs.cost);   
        if (nodeArgs.cost == (byte)NodeDirectionUtility.NodeCostType.Target)
        {
            objArrow.SetActive(false);
            objStop.SetActive(false);
            objTarget.SetActive(true);
            objBase.SetActive(false);
        }
        else if (nodeArgs.cost == (byte)NodeDirectionUtility.NodeCostType.Base)
        {
            //基地
            objArrow.SetActive(false);
            objStop.SetActive(false);
            objTarget.SetActive(false);
            objBase.SetActive(true);
        }
        else if (nodeArgs.cost == byte.MaxValue)
        {
            objArrow.SetActive(false);
            objBlock.SetActive(true);
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.North)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 0); // North: z=0
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.South)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 180); // South: z=180
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.East)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 270); // East: z=270
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.West)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 90); // West: z=90
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.NorthEast)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 315); // NorthEast: z=315
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.NorthWest)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 45); // NorthWest: z=45
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.SouthEast)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 225); // SouthEast: z=225
            arrowTransform.rotation = newRot;
        }
        else if (nodeArgs.bestDirection == NodeDirectionUtility.SouthWest)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, 135); // SouthWest: z=135
            arrowTransform.rotation = newRot;
        }
        else
        {
            //停止
            objArrow.SetActive(false);
            objStop.SetActive(true);
            objBase.SetActive(false);
        }
    }
    
    #region 辅助显示
    public void OnMouseOver()
    {
        objGridSelector.SetActive(true);
    }

    public void OnMouseExit()
    {
        objGridSelector.SetActive(false);
    }
    #endregion

}

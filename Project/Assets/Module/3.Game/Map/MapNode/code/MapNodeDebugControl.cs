using UnityEngine;

public class MapNodeDebugControl : Singleton<MapNodeDebugControl>
{
    MapNodeDebugView[,] nodeViewArray;

    MapNodeDebugView selectedNodeView;

    public void Init(int row, int col)
    {
        nodeViewArray = new MapNodeDebugView[row, col];
    }

    public void OnRefreshNode()
    {
        foreach (var nodeView in nodeViewArray)
        {
            nodeView.OnRefresh();
        }
    }

    public void AddNodeView(int x, int y, GameObject nodeView)
    {
        nodeViewArray[x, y] = nodeView.GetComponent<MapNodeDebugView>();
        nodeViewArray[x, y].Init(x, y);
    }

    public void OnClear()
    {
        for(int x = 0; x < nodeViewArray.GetLength(0); x++)
        {
            for(int y = 0; y < nodeViewArray.GetLength(1); y++) 
            {
                Destroy(nodeViewArray[x, y].gameObject);
            }
        }
        nodeViewArray = null;
    }

    //鼠标悬停提示
    public void OnMouse(int x, int y)
    {
        if(selectedNodeView == null)
        {
            nodeViewArray[x, y].GetComponent<MapNodeDebugView>().OnMouseOver();
            selectedNodeView = nodeViewArray[x, y];
        }
        else
        {
            if(selectedNodeView != nodeViewArray[x, y])
            {
                selectedNodeView.OnMouseExit();
                selectedNodeView = nodeViewArray[x, y];
                selectedNodeView.OnMouseOver();
            }
        }
    }

    public void OnReset()
    {
        foreach (var nodeView in nodeViewArray)
        {
            nodeView.OnMouseExit();
        }
        selectedNodeView = null;
    }
}

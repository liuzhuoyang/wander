using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class DebuggerGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        //Canvas.ForceUpdateCanvases();
        //GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    public void OnClose()
    {
        Debugger.Instance.OnCloseDebug();
    }    
}

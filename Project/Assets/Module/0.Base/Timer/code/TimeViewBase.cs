using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TimeViewBase : MonoBehaviour
{
    [SerializeField] TextTimerHandler textTimer;
    [SerializeField] RectTransform rectTransform;

    public void Refresh(int time,Action action = null)
    {
        if (!gameObject || !gameObject.activeInHierarchy)
        {
            return;
        }
        textTimer.OnCount(time,0,"",action);
        CancelInvoke("IEOnCount");
        InvokeRepeating("IEOnCount", 1, 1);
        StartCoroutine(RefreshLayoutNextFrame());
    }

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    private void OnDisable()
    {
        CancelInvoke("IEOnCount");
    }

    void IEOnCount()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
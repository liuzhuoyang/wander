using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextTimerHandler : MonoBehaviour
{
    int sec;
    int tragetSec;
    string describe;
    public TextMeshProUGUI textTimer;
    Action action;

    public void OnCount(int sec, int tragetSec = 0, string describe = "", Action action = null)
    {
        this.sec = sec;
        this.tragetSec = tragetSec;
        this.describe = describe;
        this.action = action;

        textTimer.text = describe + TimeUtility.GetTimeFormat(sec);

        CancelInvoke("IEOnCount");
        InvokeRepeating("IEOnCount", 1, 1);
    }

    private void OnDisable()
    {
        CancelInvoke("IEOnCount");
    }

    void IEOnCount()
    {
        if (sec > tragetSec)
        {
            sec--;
            textTimer.text = describe + TimeUtility.GetTimeFormat(sec);
        }
        else
        {
            CancelInvoke("IEOnCount");
            action?.Invoke();
            textTimer.text = UtilityLocalization.GetLocalization("generic/expired");
        }
    }

    public void OnCountUp(int sec, int tragetSec = 0, string describe = "")
    {
        this.sec = sec;
        this.tragetSec = tragetSec;
        this.describe = describe;

        textTimer.text = describe + TimeUtility.GetTimeFormat(sec);

        CancelInvoke("IEOnCountUp");
        InvokeRepeating("IEOnCountUp", 1, 1);
    }

    void IEOnCountUp()
    {
        if (sec < tragetSec)
        {
            sec++;
            textTimer.text = describe + TimeUtility.GetTimeFormat(sec);
        }
        else
        {
            CancelInvoke("IEOnCountUp");
        }
    }
}

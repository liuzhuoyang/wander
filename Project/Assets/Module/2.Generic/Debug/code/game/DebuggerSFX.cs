using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DebuggerSFX : MonoBehaviour
{
    public TMP_InputField inputField;

    public void OnDebugPlay()
    {        
        AudioControl.Instance.PlaySFX(inputField.text);
    }

    public void OnDebugStop()
    {
        AudioControl.Instance.StopSFX(inputField.text);
    }
}

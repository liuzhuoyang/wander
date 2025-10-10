using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using SimpleAudioSystem;

public class DebuggerSFX : MonoBehaviour
{
    public TMP_InputField inputField;

    public void OnDebugPlay()
    {        
        AudioManager.Instance.PlaySFXLoop(inputField.text);
    }

    public void OnDebugStop()
    {
        AudioManager.Instance.StopSFXLoop(inputField.text);
    }
}

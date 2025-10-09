
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
[CreateAssetMenu(fileName = "ThemeData", menuName = "OniData/Data/Theme/ThemeData", order = 1)]
public class ThemeData : ScriptableObject
{
    [ReadOnly]
    public string themeName;
    public string displayName;

   // public BattleAudio_Config_SO audioConfig;

    [ReadOnly] public string audioConfigName;

    [Button("InitData")]
    public void InitData()
    {
        themeName = this.name;

        //audioConfigName = audioConfig ?audioConfig.name:string.Empty;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}


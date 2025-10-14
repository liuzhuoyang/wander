using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using onicore.editor;
using System.Linq;
#endif


[CreateAssetMenu(fileName = "formatian_asset", menuName = "OniData/Data/Formatian/FormatianData", order = 1)]
public class FormatianData : ScriptableObject
{
    [ReadOnly]
    public string formatianName;

    
}

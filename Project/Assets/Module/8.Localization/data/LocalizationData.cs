
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
[CreateAssetMenu(fileName = "LocalizationData", menuName = "OniData/Localization/LocalizationData", order = 1)]
public class LocalizationData : ScriptableObject
{
    [BoxGroup("Mapping")]
    //是否mapping类型的本地化资源
    public LocalizationType type;

    [BoxGroup("注意事项")]
    public bool isEdit;

    [BoxGroup("注意事项")]
    [EnableIf("isEdit")]
    public string hint;
    
    public List<LocalizationSerializedItem> list;
}


using UnityEngine;
using Sirenix.OdinInspector;
using System;

[Serializable]
[CreateAssetMenu(fileName = "race_asset", menuName = "OniData/Data/Race/RaceData", order = 1)]
public class RaceData : ScriptableObject
{
    [BoxGroup("Info")]
    [ReadOnly]
    public string raceName;
    
    [Button("Init Data", ButtonSizes.Gigantic)]
    void OnInitData()
    {
        raceName = this.name;
    }
}

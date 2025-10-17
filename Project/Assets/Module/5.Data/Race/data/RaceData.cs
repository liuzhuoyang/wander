using UnityEngine;
using System;
using RTSDemo.Unit;

[Serializable]
[CreateAssetMenu(fileName = "race_asset", menuName = "OniData/Data/Race/RaceData", order = 1)]
public class RaceData : ScriptableObject
{
    public UnitRace race;
}

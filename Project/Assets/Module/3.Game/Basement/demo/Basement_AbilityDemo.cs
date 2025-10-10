using BattleActor.Basement;
using UnityEngine;

public class Basement_AbilityDemo : MonoBehaviour
{
    public void TriggerAbility(int abilityIndex)
    {
        BasementControl.Instance.m_currentBasement.TriggerAbility(abilityIndex);
    }
}

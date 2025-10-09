using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Utils;

public class DebuggerHaptic : MonoBehaviour
{
    public void OnVibrate()
    {
        UtilityHaptic.Haptic(HapticTypes.None);
    }

    public void OnHapticSelection()
    {
        UtilityHaptic.Haptic(HapticTypes.Selection);
    }

    public void OnHapticHeavyImpact()
    {
        UtilityHaptic.Haptic(HapticTypes.HeavyImpact);
    }

    public void OnHapticLightImpact()
    {
        UtilityHaptic.Haptic(HapticTypes.LightImpact);
    }

    public void OnHapticMediumImpact()
    {
        UtilityHaptic.Haptic(HapticTypes.MediumImpact);
    }

    public void OnHapticSuccess()
    {
        UtilityHaptic.Haptic(HapticTypes.Success);
    }

    public void OnHapticWarning()
    {
        UtilityHaptic.Haptic(HapticTypes.Warning);
    }

    public void OnHapticFailure()
    {
        UtilityHaptic.Haptic(HapticTypes.Failure);
    }
}

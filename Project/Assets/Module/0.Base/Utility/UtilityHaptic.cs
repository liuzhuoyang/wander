using UnityEngine;
using Voodoo.Utils;

public enum HapticTypes { Selection, Success, Warning, Failure, LightImpact, MediumImpact, HeavyImpact, None }

public static class UtilityHaptic
{
    //通用按钮震动，不要太多选项弄复杂，公用一个
    public static void OnHapticButton()
    {
        Haptic(HapticTypes.MediumImpact);
    }

    public static void Haptic(HapticTypes type = HapticTypes.None)
    {
        if (SettingManager.Instance.isHapticOn == 1)
        {
            if (type == HapticTypes.None)
            {
                Vibrations.Vibrate();
            }
            else
            {
                Vibrations.Haptic((Voodoo.Utils.HapticTypes)type);
            }
        }
    }
}

using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AdData", menuName = "OniData/Monetization/Ad/AdData", order = 1)]
public class AdData : ScriptableObject
{
    //[ReadOnly]
    public AdType adType;

    //spublic TGCOPSNode tgNode;

    [LabelText("是否每日重置")] public bool isDailyReset;
    [LabelText("每日上限(-1无上限)"),ShowIf("isDailyReset")] public int dailyLimit;

    /*
    #if UNITY_EDITOR
        [Button("Init Data")]
        void InitData()
        {
            string targetName = this.name.Replace("ad_", "").Replace("_", "");
            adType = (AdType)Enum.Parse(typeof(AdType), targetName, true);

            if (adType == AdType.None)
            {
                Debug.LogError("AdType is None");
            }
        }
    #endif*/
}

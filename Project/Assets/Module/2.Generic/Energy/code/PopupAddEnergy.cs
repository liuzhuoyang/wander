using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupAddEnergyArgs : PopupArgs
{

}

public class PopupAddEnergy : PopupBase
{
    [SerializeField] TextMeshProUGUI textAdEnergyRemain;
    [SerializeField] TextMeshProUGUI textGemEnergyRemain;

    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);

        PopupAddEnergyArgs popupAddEnergyArgs = args as PopupAddEnergyArgs;

        int adEnergyRemain = EnergySystem.AD_ENERGY_DAILY_LIMIT - GameData.userData.userEnergy.dailyAdEnergy;
        int gemEnergyRemain = EnergySystem.GEM_ENERGY_DAILY_LIMIT - GameData.userData.userEnergy.dailyGemEnergy;

        textAdEnergyRemain.text = string.Format(UtilityLocalization.GetLocalization("dynamic/remaining_x"), adEnergyRemain);
        textGemEnergyRemain.text = string.Format(UtilityLocalization.GetLocalization("dynamic/remaining_x"), gemEnergyRemain);
    }

    public void OnAdEnergy()
    {
        if (GameData.userData.userEnergy.dailyAdEnergy >= EnergySystem.AD_ENERGY_DAILY_LIMIT)
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("tip/generic/daily_limit"));
            return;
        }

/*
        AdControl.Instance.OnVideoAdSkippable(GameData.allAd.dictAdData[AdType.MetaEnergyGet], () =>
        {
            List<RewardShowArgs> rewardArgs = new List<RewardShowArgs>();
            rewardArgs.Add(new RewardShowArgs
            {
                name = ConstantItem.ENERGY,
                num = 10,
            });
            RewardSystem.Instance.OnRewardDisplay(rewardArgs);
            EnergySystem.Instance.AddEnergy(10, false);
            GameData.userData.userEnergy.dailyAdEnergy++;
            EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
            {
                action = ActionType.EnergyBuy,
            });
            OnClose();
        }, null);*/
    }

    public void OnGemEnergy()
    {
        if (GameData.userData.userEnergy.dailyGemEnergy >= EnergySystem.GEM_ENERGY_DAILY_LIMIT)
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("tip/tip_purchase_limit"));
            return;
        }

        ItemSystem.Instance.UseItem(ConstantItem.GEM, 90, () =>
        {
            List<RewardShowArgs> rewardArgs = new List<RewardShowArgs>();
            rewardArgs.Add(new RewardShowArgs
            {
                name = ConstantItem.ENERGY,
                num = 15,
            });
            RewardSystem.Instance.OnRewardDisplay(rewardArgs);
            EnergySystem.Instance.AddEnergy(15, false);
            GameData.userData.userEnergy.dailyGemEnergy++;
            EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
            {
                action = ActionType.EnergyBuy,
            });
            OnClose();
        }, null);
    }
}

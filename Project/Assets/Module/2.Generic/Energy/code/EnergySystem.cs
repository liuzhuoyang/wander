using UnityEngine;
using System;
public class EnergySystem : Singleton<EnergySystem>
{
    //体力恢复时间
    // public const int ENERGY_RECOVER_TIME = 300;
    public int ENERGY_RECOVER_TIME = 300; //测试用
    //广告体力每日限制
    public const int AD_ENERGY_DAILY_LIMIT = 2;
    //宝石体力每日限制
    public const int GEM_ENERGY_DAILY_LIMIT = 4;

    public void Init()
    {
        //首次登陆用户，体力拉满
        if (GameData.userData.userProgress.isFirstGame)
        {
            AddEnergy(GetUserMaxEnergy());
        }

        //注册读秒事件
        EventManager.StartListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);

        int offlineDuration = 0;
        if (GetCurrentEnergy() < GetUserMaxEnergy())
        {
            //获得当前时间和用户记录里保存时间的差值，也就是离线时间
            offlineDuration = (int)(TimeManager.Instance.GetCurrentTimeSpan() - GameData.userData.userAccount.saveTime);
            Debug.Log($"=== EnergySystem: 离线时间 {offlineDuration} ===");
            //计算体力
            OnApplyEnergy(offlineDuration);
        }
        ;
    }

    private void OnDestroy()
    {
        EventManager.StopListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
    }

    //后台回来计算离线时间
    public void OnApplyBackendTime()
    {
        OnApplyEnergy(TimeManager.Instance.secTempAFK);
    }

    //离线时间
    //AFK = Away From Keyboard
    //Duration 时长
    void OnApplyEnergy(long afkDuration)
    {
        int energyRemain = GameData.userData.userEnergy.energyRecoverTimer; //恢复剩余时间
        int energyAdd = 0; //获得体力数量
        if (energyRemain <= afkDuration)
        {
            Debug.Log($"=== EnergySystem: 离线时间 {afkDuration} ===");
            //离线时间大于恢复剩余时间，需要计算获得体力数量
            energyAdd++; //获得1体力
            afkDuration -= energyRemain; //离线时间减去恢复剩余时间，剩下的时间继续计算获得体力数量
            energyAdd += (int)afkDuration / ENERGY_RECOVER_TIME; //获得体力数量，注意这里会计算超过最大体力值，在后面AddEnergy里会再处理这种情况，这里不用管，只管时间间隔加了多少
            Debug.Log($"=== EnergySystem: 增加的体力数量: {energyAdd} ===");
            energyRemain = ENERGY_RECOVER_TIME - ((int)afkDuration % ENERGY_RECOVER_TIME);  //剩余时间，用于下一次体力恢复
            Debug.Log($"=== EnergySystem: 剩余增加体力的时间: {energyRemain} ===");
        }
        else
        {
            //离线时间小于恢复剩余时间，直接减少时间即可
            energyRemain -= (int)afkDuration;
        }

        if (energyAdd > 0)
        {
            AddEnergy(energyAdd);
        }

        //赋值剩余时间
        GameData.userData.userEnergy.energyRecoverTimer = energyRemain;

        //刷新体力
        EventManager.TriggerEvent(EventNameHeader.EVENT_HEADER_UPDATE_ITEM_NUM_UI, new UIHeaderItemNumArgs()
        {
            itemName = ConstantItem.ENERGY,
        });
    }

    //读秒，恢复体力
    void OnTick(TimeArgs args)
    {
        if (GetCurrentEnergy() >= GetUserMaxEnergy())
        {
            return;
        }

        //倒计时，为0怎增加一点体力
        GameData.userData.userEnergy.energyRecoverTimer--;

        //读秒超过体力恢复的阈值，恢复体力
        if (GameData.userData.userEnergy.energyRecoverTimer <= 0)
        {
            AddEnergy(1);
        }
    }

    //添加体力
    public void AddEnergy(int num, bool needCheckMax = true)
    {
        //如果体力超过最大体力值，则只添加到最大体力值
        if (needCheckMax)
        {
            int energyMax = GetUserMaxEnergy();
            num = Math.Min(num, energyMax - GetCurrentEnergy());
        }

        //得到体力Item
        ItemSystem.Instance.GainItem(ConstantItem.ENERGY, num);
        //恢复体力时间设置为配置的恢复时间
        GameData.userData.userEnergy.energyRecoverTimer = ENERGY_RECOVER_TIME;

        //触发体力恢复事件
        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
        {
            action = ActionType.EnergyRecover,
        });
    }

    //使用体力
    public void UseEnergy(int num, Action callbackSucceed, Action callbackFailed = null)
    {
        ItemSystem.Instance.UseItem(ConstantItem.ENERGY, num, () =>
        {
            callbackSucceed?.Invoke();
        },
        () =>
        {
            callbackFailed?.Invoke();
        },
        "tip/tip_lack_energy");
    }

    #region 内部辅助方法
    int GetCurrentEnergy()
    {
        return ItemSystem.Instance.GetItemNum(ConstantItem.ENERGY);
    }
    #endregion

    #region 对外方法
    public int GetUserMaxEnergy()
    {
        return 30;
    }

    public void OnPopupAddEnergy()
    {
        PopupAddEnergyArgs args = new PopupAddEnergyArgs();
        args.popupName = "popup_add_energy";
        PopupManager.Instance.OnPopup(args);
    }
    #endregion
}

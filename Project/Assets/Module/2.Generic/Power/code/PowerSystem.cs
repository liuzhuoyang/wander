using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerSystem : Singleton<PowerSystem>
{
    //每个武器战力
    Dictionary<string, int> dictGearPower;
    //上阵武器战力总和
    int gearPower;

    //天赋战力总和
    int talentPower;

    //总战力
    int totalPower;

    #region 初始化
    public void Init()
    {
        dictGearPower = new Dictionary<string, int>();
        totalPower = 0;
        //计算武器战力
        //CalculateWeaponPower();
        //计算天赋战力
        // CalculateTalentPower();
        //计算总战力
        OnUpdatePower(false); //初始化更新战力不出tip
    }
    #endregion

    #region 武器战力
    public void CalculateWeaponPower()
    {
        //计算拥有武器战力
        foreach (var gearData in GameData.userData.userGear.dictGear)
        {
            //获取武器属性
            CalculateGearPower(gearData.Key);

        }
        //计算上阵武器战力总和
        ChangeGearTeamPower();
    }

    void CalculateGearPower(string gearName)
    {
        
    }
    //获取指定武器战力
    public int GetGearPower(string gearName)
    {
        if (!dictGearPower.ContainsKey(gearName))
        {
            return 0;
        }
        return dictGearPower[gearName];
    }
    //更新指定武器战力
    public void ChangeGearPower(string gearName)
    {
        //计算武器战力
        CalculateGearPower(gearName);
        //判断是否上阵
        if (GameData.userData.userGear.listEquipGear.Contains(gearName))
        {
            //更新战力
            ChangeGearTeamPower();
        }
    }
    //更新阵容战力
    public void ChangeGearTeamPower()
    {
        //计算上阵武器战力总和
        gearPower = 0;
        foreach (var equipName in GameData.userData.userGear.listEquipGear)
        {
            gearPower += dictGearPower[equipName];
        }
        //计算武器总星级
        // int totalStar = GetGearTotalStar();
        // totalGearPower += (int)(totalGearPower * (totalStar * 0.05f));
        //天赋总战力需要武器总战力，武器战力更新后需要重新计算天赋总战力
        CalculateTalentPower();
        // OnTotalPowerChanged();
    }

    public int GetTotalGearPower()
    {
        return gearPower;
    }

    public float GetGearTotalStar()
    {
        int result = 0;
        foreach (var gear in GameData.userData.userGear.dictGear)
        {
            result += gear.Value.star;
        }
        return result * 0.05f;
    }
    #endregion

    #region 天赋战力
    public void CalculateTalentPower()
    {

    }
    #endregion

    #region 宠物战力
    public void CalculatePetPower(string gearName)
    {
        ChangeGearPower(gearName);
    }
    #endregion

    #region 觉醒战力
    public void CalculateEvolvePower(string gearName)
    {
        ChangeGearPower(gearName);
    }
    #endregion


    #region 总战力
    public int GetTotalPower()
    {
        return totalPower;
    }

    public void OnUpdatePower(bool isShowTip = true)
    {
        int oldPower = totalPower;
        totalPower = gearPower + talentPower;

        if(isShowTip)
        {
            ShowPowerTip(oldPower, totalPower);
        }

        EventManager.TriggerEvent<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_REFRESH_POWER, new UIHeaderArgs()
        {
            power = totalPower
        });

        //更新用户历史最高战力
        if (totalPower > GameData.userData.userStats.highestPower)
        {
            GameData.userData.userStats.highestPower = totalPower;
        }
    }
    #endregion

    #region 战力变化
    async void ShowPowerTip(int oldPower, int newPower)
    {
        GameObject customTipPrefab = await GameAsset.GetPrefabAsync("tip_power");

        TipManager.Instance.OnCustomTip(new UITipPowerArgs
        {
            oldPower = oldPower,
            newPower = newPower,
            customTipPrefab = customTipPrefab
        });
    }
    #endregion

    #region 调试
    public void OnDebugTipPower()
    {
        ShowPowerTip(0, 1000);
    }
    #endregion
}
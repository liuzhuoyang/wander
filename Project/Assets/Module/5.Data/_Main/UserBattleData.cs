using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
//用户临时战斗数据
public class UserBattleData
{
    public bool isReady;  //这个战斗数据已经刷新,可以被恢复
    //关卡数据
    public LevelType levelType;
    public int chapterID;
    public int levelID;
    public string eliteName;
    public int currentWave;
    public int randomSpawnPointWave;
    public LevelModeType chapterMode;
    public float baseHealthPercent;
    public float baseShield;
    public float baseMana;

    public float expandedBrickRate;
    public int expandedBrickStack;
    public int refreshCountPerWave;
    public int refreshCountTotal;
    public int inGameExp;
    public int inGameLevel;
    public int battleToken;
    public int expandCount;
    public int reviveCount;
    public int forgeValue;
    public int forgeCount;

    //装备权重
    public Dictionary<string, float> dictBattleGearWeight;
    // public Dictionary<string, int> dictADGear;

    //技能列表
    public Dictionary<string, List<string>> dictGearSkillPool;
    public List<string> listBasicSkillPool;
    public List<string> listSkillNameToSelect;
    public Dictionary<string, List<string>> dictGearSkill;
    public Dictionary<string, List<string>> dictSelectSkill;
    public int selectedSkillCount;
    public Dictionary<string, List<string>> dictADGearSkill;
    public int GearIIAdSkillCount;
    public int BasicAdSkillCount;

    //防御塔解锁状态
    public Dictionary<string, bool> dictDefenseTowerLockState;

    //战斗统计数据
    public int enemyKilled;
    public int eliteKilled;
    public int bossKilled;
    public int mergeCount;
    public int battleTokenEarned;

    //奖励数据
    public Dictionary<string, int> itemRewardDict;

    public void Save()
    {
        Debug.Log("=== UserBattleData: 保存用户战斗数据 ===");
        isReady = true;
        ReadWrite.WriteUserBattleData(this);
    }

    public static UserBattleData Load()
    {
        // 读取用户战斗数据
        string dataStream = ReadWrite.ReadUserBattleData();

        // 如果用户临时战斗数据为空
        if (string.IsNullOrEmpty(dataStream))
        {
            UserBattleData userBattleData = new UserBattleData();
            return userBattleData;
        }

        // 读取用户临时战斗数据
        UserBattleData savedUserBattleData = JsonConvert.DeserializeObject<UserBattleData>(dataStream);
        return savedUserBattleData;
    }

    public void Clear()
    {
        Debug.Log("=== UserBattleData: 清除用户战斗数据 ===");
        isReady = false;
        ReadWrite.WriteUserBattleData(this);
    }
}

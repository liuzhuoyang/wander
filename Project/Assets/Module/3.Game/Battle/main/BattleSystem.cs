using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


public class BattleSystem : Singleton<BattleSystem>
{
    public bool isSandboxBattle = false;

    LevelData currentLevelData; //当前关卡数据

    public async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("prepare", "battle");
        await UIMain.Instance.CreateModeSubPage("fight", "battle");

        //添加功能事件，点击按钮后开启战斗功能
        FeatureUtility.OnAddFeature(FeatureType.Battle, () => {
            Game.Instance.OnChangeState(GameStates.Battle);
        });
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnBattleAction(BattleActionArgs args)
    {
        switch (args.battleAction)
        {
            case BattleActionType.EnemyKilled:

                break;
            case BattleActionType.WaveStart:
               
                break;
            case BattleActionType.WaveEnd:
                GameData.userData.userStats.OnWavePassed();
                break;
            case BattleActionType.LevelUp:
                
                break;
            case BattleActionType.PlayerDead:
              
                break;
        }
    }

    #region 战斗系统的开始结束唯一出入口
    public async void BattleStart()
    {   
        //禁止用户输入
        ActingSystem.Instance.OnActing(this.name); 

        await LevelControl.OnLoadLevel(LevelType.Main, 1, 1);

        OnPrepare();

        //这个允许用户输入的位置，还需要考虑后续添加的动画
        ActingSystem.Instance.StopActing(this.name);
    }

    void OnPrepare()
    {
         //播放准备音乐
        PlayBattlePrepareBGM();

        //打开准备界面
        ModeBattleControl.OnOpen("prepare");
    }

    public void OnBattleWin()
    {
       
    }

    public void BattleEnd()
    {
       
    }
    #endregion

    #region 战斗波次流程管理

    //进入到合成页面
    void OnWavePrepare()
    {
       
    }

    //波段战斗开始
    //点击战斗按钮切换成战斗界面
    public bool OnWaveFight()
    {
        
        return true;
    }


    //当玩家成功赢下当前波次时触发
    void OnWinWave()
    {
        
    }
    void OnWaveEnd()
    {
        
        //进入下一波次
        OnWavePrepare();
    }

    public void OnDebugWaveEnd()
    {
       
        OnWaveEnd();
    }

    public void OnDebugWaveJump(int targetWave)
    {
        OnWavePrepare();
    }

    public void OnRevive()
    {
        OnWaveEnd();

        //AnalyticsControl.Instance.OnLogLevelRevive(BattleData.levelType, BattleData.chapterID, BattleData.levelID, BattleData.currentWave);
    }
    #endregion

    public void OnRefreshBrick(bool isFree = false)
    {

    }

    public void OnPause()
    {
        
    }

    public void OnResume()
    {

    }

    #region 音乐
    protected void PlayBattlePrepareBGM()
    {
        //播放准备音乐
       // AudioControl.Instance.PlayBGM(battleAudioConfig.GetPrepareBGM());
    }
    #endregion
}
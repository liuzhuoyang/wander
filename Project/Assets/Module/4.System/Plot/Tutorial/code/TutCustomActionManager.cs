using Cysharp.Threading.Tasks;
using UnityEngine;

//教程自定义事件管理器，处理特殊的教程任务
public class TutCustomActionManager : MonoBehaviour
{
    public void Init()
    {

    }

    //对于需要特殊预处理的教程事件，写在里面
    public void InitCustomAction(TutCustomAction tutCustomAction)
    {
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
        EventManager.StartListening<BattleActionArgs>(EventNameBattleAction.EVENT_ON_BATTLE_ACTION, OnBattleAction);

        switch (tutCustomAction)
        {
            case TutCustomAction.TUT_BATTLE_GEAR_MOVE:
                //启用手指动画
                // Vector2 startPos = BattleBrickShopControl.Instance.GetBrickPos(0);
                // startPos += new Vector2(100, 100);
                // //默认基地的世界坐标为0
                // Vector2 endPos = Camera.main.WorldToScreenPoint(Vector2.zero);
                // endPos += new Vector2(100, 100);
                // StartFingerAnimation(startPos, endPos);
                //BattleUtil.OnEnableInput();
                break;
            case TutCustomAction.TUT_BATTLE_SUPPORT_SKILL_SHOW_UP:
                //tut_c001_01_b04
                //ModeBattleControl.OnShowSupportSkill();
                //立马进入下一步
                EventManager.TriggerEvent<TutCustomActionArgs>(TutEventName.EVENT_ON_TUT_CUSTOM_ACTION, new TutCustomActionArgs
                {
                    tutCustomAction = TutCustomAction.TUT_BATTLE_SUPPORT_SKILL_SHOW_UP,
                    isSuccess = true,
                });
                //开启基地技能
                break;
            default:
                break;
        }
    }

    public void FinalizeCustomAction(TutCustomAction tutCustomAction)
    {
        EventManager.StopListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
        EventManager.StopListening<BattleActionArgs>(EventNameBattleAction.EVENT_ON_BATTLE_ACTION, OnBattleAction);

        switch (tutCustomAction)
        {
            case TutCustomAction.TUT_BATTLE_GEAR_MOVE:
                // StopFingerAnimation();
                //BattleUtil.OnDisableInput();
                break;
        }
    }

    void OnAction(ActionArgs args)
    {
        switch (args.action)
        {
            case ActionType.SummonGearSuccess:
                EventManager.TriggerEvent<TutCustomActionArgs>(TutEventName.EVENT_ON_TUT_CUSTOM_ACTION, new TutCustomActionArgs
                {
                    tutCustomAction = TutCustomAction.TUT_TAVERN_SUMMON_GEAR,
                    isSuccess = true,
                });
                break;
            default:
                break;
        }
    }

    void OnBattleAction(BattleActionArgs args)
    {
        //监听游戏内事件触发教程完成的事件
        switch (args.battleAction)
        {
            case BattleActionType.GearMoveSuccess:
                
                break;
            default:
                break;
        }
    }
}

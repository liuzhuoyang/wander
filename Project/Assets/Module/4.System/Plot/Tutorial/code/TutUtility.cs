using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
//教程自定义工具类，根据当前项目修改
public static class TutUtility
{
    //需要手动查找按钮的列表
    public static List<string> btnEventKeyListToSearch = new List<string>(){
        "btn_gear_slot_01",
        "btn_gear_slot_06",
        "btn_gear_slot_07",
    };

    public static bool IsBtnEventKeyNeedSearch(string btnEventKey)
    {
        return btnEventKeyListToSearch.Contains(btnEventKey);
    }

    public static GameObject SearchBtnObjInScene(string btnEventKey)
    {
        return null;
    }

    //是否创建手指
    public static bool IsCreatePointer(string btnEventKey)
    {
        return true;
    }

    #region 特殊步骤预处理
    public static async UniTask InitTutCustomStep(TutData currentTutArgs, int currentStepIndex)
    {
        switch (currentTutArgs.tutName)
        {
            /*
            case "tut_c000_00_b00_fight":
                if (currentStepIndex == 1)
                {
                    BattleGridControl.Instance.SetGridCustomLock(1, 2);
                    BattleInputControl.Instance.SetGridUnDragable(true);

                    //手指动画
                    Vector2 startPos = BattleBrickShopControl.Instance.transform.position + new Vector3(150, 0, 0);
                    Vector2 endPos = BattleGridControl.Instance.GetWorldPosFromAxis(2, 2);

                    StartFingerAnimation(startPos, CameraManager.Instance.WorldToScreenPos(endPos));

                    //helper
                    TutUtilityHelper.Instance.OnForceShowGridHelper(1, 2);
                }
                break;
            case "tut_c001_01_m01_deploy_gear":
                if (currentStepIndex == 1)
                {
                    BattleInputControl.Instance.SetTutorialHeplerOn(true);

                    //手指动画
                    Vector2 startPos = BattleBrickShopControl.Instance.transform.position + new Vector3(150, 0, 0);
                    Vector2 endPos = BattleGridControl.Instance.GetWorldPosFromAxis(2, 3);

                    StartFingerAnimation(startPos, CameraManager.Instance.WorldToScreenPos(endPos));
                }
                break;
            case "tut_t1006":
                // 需要等待ui动画结束
                if (currentStepIndex == 2)
                {
                    ActingSystem.Instance.OnActing();
                    await UniTask.Delay(600);
                    ActingSystem.Instance.StopActing();
                }
                break;
            case "tut_t1007":
                // 第7步时，需要等待ui动画结束
                if (currentStepIndex == 7)
                {
                    ActingSystem.Instance.OnActing();
                    await UniTask.Delay(800);
                    ActingSystem.Instance.StopActing();
                }
                break;
            case "tut_t1010":
                // 第7步时，需要等待ui动画结束
                if (currentStepIndex == 2)
                {
                    ActingSystem.Instance.OnActing();
                    await UniTask.Delay(600);
                    ActingSystem.Instance.StopActing();
                }
                break;
            case "plot_t1015":
                if (currentStepIndex == 2)
                {
                    ActingSystem.Instance.OnActing();
                    await UniTask.Delay(250);
                    ActingSystem.Instance.StopActing();
                }
                break;*/
            default:
                break;
        }
    }

    public static void FinalizeTutCustomStep(TutData targetTutData, int currentStepIndex)
    {
        
    }

    //整个教程结束时的额外处理
    public static void FinalizeTutCustom(TutData targetTutData)
    {
        /*
        switch (targetTutData.tutName)
        {
            case "tut_t1004":
                BattleBehaviourManager.Instance.SetPause(true);
                break;
            default:
                break;
        }*/
    }
    #endregion

    #region Helper
    static void StartFingerAnimation(Vector2 startPos, Vector2 endPos)
    {
        EventManager.TriggerEvent<UITutFingerArgs>(TutEventName.EVENT_TUT_FINGER_DRAG_UI, new UITutFingerArgs
        {
            startPos = startPos,
            endPos = endPos,
        });
    }

    static void StopFingerAnimation()
    {
        EventManager.TriggerEvent<UITutFingerArgs>(TutEventName.EVENT_TUT_FINGER_STOP_UI, new UITutFingerArgs
        {
        });
    }
    #endregion
}

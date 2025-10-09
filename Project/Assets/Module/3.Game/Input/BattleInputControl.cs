using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;

public class BattleInputControl : Singleton<BattleInputControl>
{
    #region 字段
    //PlayerInputAction inputAction;

    Camera mainCamera;                // 主相机，在Update里调用Camera.main性能不好，所以定义一个变量
    Vector2 initialFingerPosition;    // 初始触摸位置
    float moveThreshold = 15f;        // 移动阈值，单位为像素，超过了阈值才算移动
    bool isMoving;                    // 标记是否已经移动超过阈值

    Vector2 pointerUpWorldPosition;     //手指抬起时候的世界坐标
    bool isBrickMoved = false;   //手指抓取的物件开始移动

    #endregion

    #region 初始化
    public void Init()
    {
        EnhancedTouchSupport.Enable();
        mainCamera = Camera.main;
        Input.multiTouchEnabled = false;
    }

    void OnDestroy()
    {
        OnDisableInput();
        EnhancedTouchSupport.Disable();
    }
    #endregion

    #region 输入控制
    //开启输入控制
    public void OnEnableInput()
    {
        mainCamera = Camera.main;
        //inputAction = new PlayerInputAction();
        //inputAction.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnFingerMove;
    }

    //关闭输入控制
    public void OnDisableInput()
    {
        /*
        if (inputAction != null)
        {
            inputAction.Disable();
            inputAction = null;
        }*/
        // 先检查是否启用，再移除事件监听器
        if (EnhancedTouchSupport.enabled)
        {
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
        }
    }
    #endregion

    #region 交互事件
    //按下
    void OnFingerDown(Finger finger)
    {
        if (finger.index != 0) return; //若不是primary touch，则不操作

        //如果已经打开武器详情，则不响应
        initialFingerPosition = finger.screenPosition;         // 记录初始触摸位置
        isMoving = false;

        Debug.Log($"=== BattleInputControl: OnFingerDown: {finger.screenPosition} ===");
        Debug.Log($"=== BattleInputControl: OnFingerDown: {mainCamera.ScreenToWorldPoint(finger.screenPosition)} ===");
        
    }

    // 抬手
    void OnFingerUp(Finger finger)
    {
        if (finger.index != 0) return; //若不是primary touch，则不操作

        isMoving = false;
        isBrickMoved = false;

        Debug.Log("=== BattleInputControl: OnFingerUp ===");
    }

    // 移动
    void OnFingerMove(Finger finger)
    {
        if (finger.index != 0) return; //若不是primary touch，则不操作
        
        //如果已经打开武器详情，则不响应
        if (!isMoving)
        {
            //计算初始位置和当前位置的距离，超过阈值才算移动
            if (Vector2.Distance(initialFingerPosition, finger.screenPosition) > moveThreshold)
            {
                isMoving = true; // 标记为已移动
            }
        }
        else
        {
            
        }
    }
    #endregion

    #region 拖拽物件交互
    /// 开始抓取
    public void OnDrag(Transform targetSlot)
    {
        
    }

    // 停止抓取
    public void StopDrag(
        Action onMoveSuccess,
        Action onMoveFailure,
        Action onTileExtendSuccess,
        Action onTileExtendFailure,
        Action onMergeSuccess
    ){

    }
    #endregion

    #region 辅助方法
    //检查是否触摸到UI层

    #endregion    

    #region 对外方法
   
    #endregion

}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputControl : Singleton<InputControl>
{
    PlayerInputAction inputAction;
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouchEvent;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouchEvent;
    public void Init()
    {
        
        inputAction = new PlayerInputAction();

        inputAction.Enable();
        //TouchSimulation.Enable();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;

        inputAction.Debug.Screenshot.performed += CaptureScreenshot;
    }


    void OnDestroy()
    {
        inputAction.Disable();
        //TouchSimulation.Disable();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;

        inputAction.Debug.Screenshot.performed -= CaptureScreenshot;
    }


    void OnFingerDown(Finger finger)
    {
        if (OnStartTouchEvent != null) OnStartTouchEvent(finger.screenPosition, Time.time);
        Debug.Log("Touch position: " + finger.screenPosition); // 输出点击位置
    }

    void CaptureScreenshot(InputAction.CallbackContext context)
    {
        ScreenCapture.CaptureScreenshot("screenshot.png", GameConfig.main.screenshotSizeMultiplier);
        Debug.Log("=== InputControl: take a screenshot ===");
    }

    void Update()
    {
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            //Debug.Log("touch: " + touch.phase);
            //Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
        }
    }
}


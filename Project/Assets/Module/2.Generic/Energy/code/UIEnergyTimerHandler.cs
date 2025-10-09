using UnityEngine;  
using TMPro;

//体力读秒计时控制器
public class UIEnergyTimerHandler : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textEnergyTimer;

    void OnEnable()
    {
        //Enable要更新一次文本，因为下面计时器是按秒计算，可能启动在下一秒没到来之前，导致文本没更新
        OnRefreshEnergyTimer();
        //先停止一次，避免反复注册，一般情况不会出现，仅作为保护
        EventManager.StopListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
        EventManager.StartListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
    }

    void OnDisable()
    {
        EventManager.StopListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
    }

    //读秒计时
    //读秒是每秒都会发送的事件，并统一在TimeManager里发送
    //这里只需要监听到这个时间，并且更新剩余时间即可，不参与逻辑运算
    void OnTick(TimeArgs args)
    {
        OnRefreshEnergyTimer();
    }

    //更新文本
    void OnRefreshEnergyTimer()
    {
        textEnergyTimer.text = TimeUtility.GetTimeFormat(GameData.userData.userEnergy.energyRecoverTimer);
    }

/*
    //销毁
    void OnDestroy()
    {
        EventManager.StopListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
    }*/
}

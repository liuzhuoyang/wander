using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGroupRight : MonoBehaviour
{
    public GameObject prefabBtnTimeEvent;
    public Transform containerTimeEvent;

    public void Init()
    {
       //EventManager.StartListening<UILobbyTimeEventArgs>(EventName.EVENT_LOBBY_ADD_TIME_EVENT_UI, OnAddTimeEvent);
    }

    private void OnDestroy()
    {
        //EventManager.StopListening<UILobbyTimeEventArgs>(EventName.EVENT_LOBBY_ADD_TIME_EVENT_UI, OnAddTimeEvent);
    }

    /*
    //刷新入口，在这之前应该要完成了活动时间刷新，已经过期的活动不再创建入口
    void OnAddTimeEvent(UILobbyTimeEventArgs args)
    {
        foreach (UserTimeEventArgs data in args.listUserTimeEventData.Values)
        {
            GameObject go = Instantiate(prefabBtnTimeEvent, containerTimeEvent);
            go.GetComponent<LobbyBtnTimeEvent>().Init(data.eventType, TimeManager.Instance.GetSecondUntilResetDate(data.durationType));
        }
    }*/
}

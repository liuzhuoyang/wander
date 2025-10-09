using UnityEngine;

//Loot Pin的检测条件
public class PrivilegePinHandler : MonoBehaviour
{
    private const string PRIVILEGE_PIN_ID = "pin_privilege";
    public void Init()
    {
        CheckPrivilegePin();
    }
    public void CheckPrivilegePin()
    {
        //判断是否有可领取奖励
        foreach (var item in GameData.userData.userPrivilege.dictPrivilege)
        {
            if (item.Value.claimed)
            {
                continue;
            }
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(PRIVILEGE_PIN_ID, true));
            return;
        }
        EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(PRIVILEGE_PIN_ID, false));
    }
}

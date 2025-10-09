using UnityEngine;

//Loot Pin的检测条件
public class RelicPinHandler : MonoBehaviour
{
    private const string RELIC_PIN_ID = "pin_relic";
    public void Init()
    {
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void OnDestroy()
    {
        EventManager.StopListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    public void CheckRelicPin()
    {
        foreach (var data in AllRelic.dictData)
        {
            int star = -1;
            if (GameData.userData.userRelic.dictRelic.ContainsKey(data.Key))
            {
                star = GameData.userData.userRelic.dictRelic[data.Key];
            }
            //判断是否解锁
            int count = star == -1 ? RelicFomular.GetRelicUnlockNeedCount(data.Value.rarity) : RelicFomular.GetRelicUpgradeNeedCount(star + 1, data.Value.rarity);
            if (count <= ItemSystem.Instance.GetItemNum("item_shard_" + data.Key))
            {
                EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(RELIC_PIN_ID, true));
                return;
            }
        }
        EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(RELIC_PIN_ID, false));
    }

    void OnAction(ActionArgs args)
    {
        if (args.action == ActionType.OnBackToLobby)
        {
            CheckRelicPin();
        }
    }
}

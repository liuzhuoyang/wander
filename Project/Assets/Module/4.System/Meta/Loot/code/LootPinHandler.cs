using UnityEngine;

//Loot Pin的检测条件
public class LootPinHandler : MonoBehaviour
{
    private const string LOOT_BOX_PIN_ID = "pin_loot_box";
    public void Init()
    {
        CheckLootPin();
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void OnDestroy()
    {
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void CheckLootPin()
    {
        if (!FeatureUtility.CheckIsUnlock(FeatureType.Loot))
        {
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(LOOT_BOX_PIN_ID, false));
            return;
        }
        //判断进度
        if (GameData.userData.userLoot.point >= LootUtility.GetChestLockArgs(GameData.userData.userLoot.lockIndex).needPoint)
        {
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(LOOT_BOX_PIN_ID, true));
            return;
        }
        //判断宝箱数量
        for (int i = 0; i < 5; i++)
        {
            if (ItemSystem.Instance.GetItemNum("item_loot_box_" + (i + 1)) > 0)
            {
                EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(LOOT_BOX_PIN_ID, true));
                return;
            }
        }
        EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(LOOT_BOX_PIN_ID, false));
    }
    void OnAction(ActionArgs args)
    {
        if (args.action == ActionType.ItemReward || args.action == ActionType.OnBackToLobby)
        {
            CheckLootPin();
        }
    }
}

using UnityEngine;
public class MsgRanking : MsgBase
{
    [SerializeField] GameObject objGet, objCommit;

    MsgRankingArgs rankingArgs;
    public override void Init(MsgArgs args)
    {
        base.Init(args);
        rankingArgs = args as MsgRankingArgs;
        objGet.SetActive(rankingArgs.isGetData);
        objCommit.SetActive(!rankingArgs.isGetData);
    }

    public void OnRetry()
    {
        rankingArgs.onRetry?.Invoke();
        OnClose();
    }

    public void OnClickClose()
    {
        MessageManager.Instance.CloseLoading();
        //RankingSDK.Instance.ClearCallback();
        OnClose();
        EventManager.TriggerEvent<MsgArgs>(EventNameMsg.EVENT_MESSAGE_CLOSE_RANKING_UI, null);
    }

    public void OnReconnect()
    {
        //RankingSDK.Instance.ClearCallback();
        Game.Instance.Restart();
    }
}

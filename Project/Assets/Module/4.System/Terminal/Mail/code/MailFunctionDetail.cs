using UnityEngine;

public class MailFunctionDetail : MonoBehaviour
{
    [SerializeField] GameObject objOpenDebug;

    public void Open(MailArgs args)
    {
        if (args.content == "Open Debugger")
        {
            objOpenDebug.SetActive(true);
        }
    }

    #region 按钮事件
    public void OnOpenDebugger()
    {
        UIMain.Instance.OnOpenDebugger();
        //RankingSDK.Instance.JoinCheatRanking();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
using UnityEngine;
using TMPro;

public class MsgUpdate : MsgBase
{
    public TextMeshProUGUI textContent;
    [SerializeField] GameObject btnClose;

    public override void Init(MsgArgs args)
    {
        string content = args.content;

        MsgUpdateArgs updateArgs = args as MsgUpdateArgs;
        if (!string.IsNullOrEmpty(updateArgs.targetVersion))
        {
            content = string.Format(args.content, updateArgs.currentVersion, updateArgs.targetVersion);
        }

        base.Init(args);
        textContent.text = content;
        if (VersionManager.Instance.checkVersionArgs.isUpdateNeeded)
        {
            btnClose.SetActive(false);
            return;
        }
        btnClose.SetActive(true);
    }

    public void OnStore()
    {

    }

    public void OnClickClose()
    {
        if (VersionManager.Instance.checkVersionArgs.isUpdateNeeded)
        {
            return;
        }
        OnClose();
    }
}

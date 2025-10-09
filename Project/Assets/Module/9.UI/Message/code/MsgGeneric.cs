using UnityEngine;
using TMPro;

public class MsgGeneric : MsgBase
{
    public TextMeshProUGUI textContent;

    public override void Init(MsgArgs args)
    {
        base.Init(args);
        textContent.text = args.content;
    }
}

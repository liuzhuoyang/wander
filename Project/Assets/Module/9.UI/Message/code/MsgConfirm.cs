using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MsgConfirm : MsgBase
{
    public TextMeshProUGUI textContent;

    public override void Init(MsgArgs args)
    {
        base.Init(args);
        textContent.text = args.content;
    }
}

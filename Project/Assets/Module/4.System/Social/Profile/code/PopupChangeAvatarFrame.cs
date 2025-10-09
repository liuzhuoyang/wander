using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupChangeAvatarFrameArgs : PopupArgs
{

}

public class PopupChangeAvatarFrame : PopupBase
{
    [SerializeField] Transform container;
    [SerializeField] GameObject prefabAvatarFrameSlot;

    PopupChangeAvatarFrameArgs args;

    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        this.args = args as PopupChangeAvatarFrameArgs;

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        List<AvatarFrameData> avatarFrameDataList = AllAvatarFrame.dictData.Values.ToList();

        foreach (var avatarFrameData in avatarFrameDataList)
        {
            GameObject go = Instantiate(prefabAvatarFrameSlot, container);
            go.GetComponent<AvatarFrameSlotView>().Init(avatarFrameData, this);
        }
    }

    public void OnResetSelected()
    {
        foreach (Transform child in container)
        {
            child.GetComponent<AvatarFrameSlotView>().ResetSelected();
        }
    }

    public void OnConfirm()
    {
        ProfileSystem.Instance.SetUserAvatarFrame(OnClose);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupChangeAvatarArgs : PopupArgs
{

}

public class PopupChangeAvatar : PopupBase
{
    [SerializeField] Transform container;
    [SerializeField] GameObject prefabAvatarSlot;

    PopupChangeAvatarArgs args;

    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        this.args = args as PopupChangeAvatarArgs;

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        List<AvatarData> avatarList = AllAvatar.dictData.Values.Where(avatar => avatar.avatarType == AvatarType.Player).ToList();

        foreach (var avatar in avatarList)
        {
            GameObject go = Instantiate(prefabAvatarSlot, container);
            go.GetComponent<AvatarSlotView>().Init(avatar);
        }
    }

    public void OnResetSelected()
    {
        foreach (Transform child in container)
        {
            child.GetComponent<AvatarSlotView>().ResetSelected();
        }
    }

    public void OnConfirm()
    {
        ProfileSystem.Instance.SetUserAvatar(OnClose);
    }
}

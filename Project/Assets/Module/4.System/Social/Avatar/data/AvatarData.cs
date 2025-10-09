using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AvatarData", menuName = "OniData/Social/Avatar/AvatarData", order = 1)]
public class AvatarData : ScriptableObject
{   
    [BoxGroup("Info", LabelText = "基础信息")]  
    [ReadOnly]
    public string avatarName;

    [BoxGroup("Edit", LabelText = "可编辑内容")]
    [ValueDropdown("GetLocalizationKey")]
    [OnValueChanged("OnDisplayNameChanged")]
    public string displayName;

    [BoxGroup("Edit")]
    public AvatarType avatarType;

/*
    [BoxGroup("Edit", LabelText = "可编辑内容")]
    [ShowIf("avatarType", AvatarType.Player)]
    public bool isDefault;
*/

#if UNITY_EDITOR

    [BoxGroup("Preview", LabelText = "预览信息")]
    [ReadOnly]
    public string previewDisplayName;

    [BoxGroup("Preview")]
    [PreviewField]
    [ReadOnly]
    public Texture2D previewIcon;

    [BoxGroup("Action", LabelText = "初始化")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public void InitData()
    {
        avatarName = this.name;

        previewIcon = GameAsset.GetAssetEditor<Texture2D>("icon_" + avatarName);
    }

    public List<string> GetLocalizationKey()
    {
        List<string> listKey = new List<string>();
        string path = GameDataControl.GetLocPath("all_avatar");
        List<LocalizationData> listAssets = AssetsFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData data in listAssets)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }

    void OnDisplayNameChanged()
    {
        previewDisplayName = LocalizationDataCollection.GetValue(displayName);
    }
#endif
}

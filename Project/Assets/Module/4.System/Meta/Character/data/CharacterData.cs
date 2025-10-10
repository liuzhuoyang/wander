using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "CharacterData", menuName = "OniData/System/Meta/Character/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [LabelText("角色名")] public string characterName;

    [LabelText("显示名称"), ValueDropdown("GetLocalizationKeyList")] public string displayName;

    [LabelText("描述"), ValueDropdown("GetLocalizationKeyList")] public string info;

    [LabelText("品质")] public Rarity rarity;


#if UNITY_EDITOR

    [Button("Init Data")]
    public void InitData()
    {
        EditorUtility.SetDirty(this);
    }

    private List<string> GetLocalizationKeyList()
    {
        List<string> list = new List<string>();
        list.Add("");
        string path = GameDataControl.GetLocPath("all_character");
        List<LocalizationData> listLocalizationAsset = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData data in listLocalizationAsset)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                list.Add(item.key);
            }
        }
        return list;
    }
#endif
}


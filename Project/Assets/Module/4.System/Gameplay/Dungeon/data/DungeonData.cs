using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using onicore.editor;

[CreateAssetMenu(fileName = "DungeonData", menuName = "OniData/System/Gameplay/Dungeon/DungeonData", order = 1)]
public class DungeonData : ScriptableObject
{
    [ReadOnly]
    public string dungeonName;

    [ValueDropdown("GetLocalizationLevelDisplayNameList")]
    public string displayName;

    #if UNITY_EDITOR
    public ItemData costItem;
    #endif
    
    [ReadOnly]
    public string costItemName;
    
    #if UNITY_EDITOR
    public LevelData levelAsset;
    #endif

    [ReadOnly]
    public string levelName;

    public bool isLocked;

    #if UNITY_EDITOR
    public List<string> GetThemeList()
    {
        List<string> list = new List<string>();
        string path = GameDataControl.GetAssetPath("all_theme");
        List<ThemeData> listAsset = FileFinder.FindAllAssetsOfAllSubFolders<ThemeData>(path);
        foreach (ThemeData asset in listAsset)
        {
            list.Add(asset.themeName);
        }
        return list;
    }

    public List<string> GetLocalizationLevelDisplayNameList()
    {
        List<string> listKey = new List<string>();
        LocalizationData asset = FileFinder.FindAssetByName<LocalizationData>(EditorPathUtility.localizationPath + "/ui", "loc_page_dungeon");
        foreach (var item in asset.list)
        {
            listKey.Add(item.key);
        }
        return listKey;
    }


    [Button("初始化基础数据")]
    public void InitData()
    {
        dungeonName = this.name;
        costItemName = costItem ? costItem.itemName : "";
        levelName = levelAsset ? levelAsset.levelName : "";
    }
    #endif
}

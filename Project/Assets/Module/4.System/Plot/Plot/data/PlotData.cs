
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "plot_data", menuName = "OniData/System/Plot/Plot/PlotData", order = 1)]

public class PlotData : ScriptableObject
{
    [ReadOnly]
    [BoxGroup("Info")]
    public string plotName;

    [ReadOnly]
    [BoxGroup("Info")]
    public int totalStep;

    [BoxGroup("Info")]
    public PlotSceneType sceneType;

    [BoxGroup("Info")]
    [TableList]
    public List<PlotItem> listPlotItem;

#if UNITY_EDITOR
    [ShowIf("sceneType", PlotSceneType.Lobby)]
    [BoxGroup("Reward")]
    public ItemData reward;
#endif

    [ShowIf("sceneType", PlotSceneType.Lobby)]
    [BoxGroup("Reward")]
    [ReadOnly]
    public string rewardName;

    [ShowIf("sceneType", PlotSceneType.Lobby)]
    [BoxGroup("Reward")]
    public int rewardNum;

#if UNITY_EDITOR
    [Button("Init Data", ButtonSizes.Gigantic)]
    void OnInitData()
    {
        plotName = this.name;

        rewardName = reward == null ? string.Empty : reward.itemName;

        string path = GameDataControl.GetLocPath("all_plot");
        LocalizationData target = FileFinder.FindAssetByName<LocalizationData>(path, "loc_" + this.name);
        if (target == null)
        {
            Debug.LogError("PlotAsset: 没有找到对应的本地化资源");
            return;
        }

        #region 初始化剧情条目
        int totalItemNum = 0;
        for (int i = 0; i < target.list.Count; i++)
        {
            if (!CheckIsSkipDialogItem(target.list[i]))
            {
                totalItemNum++;
            }
        }
        if (listPlotItem == null)
        {
            listPlotItem = new List<PlotItem>();
            for (int i = 0; i < totalItemNum; i++)
            {
                PlotItem plotItem = new PlotItem();
                listPlotItem.Add(plotItem);
            }
        }
        else
        {
            if (listPlotItem.Count < totalItemNum)
            {
                int addCount = totalItemNum - listPlotItem.Count;
                for (int i = 0; i < addCount; i++)
                {
                    PlotItem plotItem = new PlotItem();
                    listPlotItem.Add(plotItem);
                }
            }
            else if (listPlotItem.Count >= totalItemNum)
            {
                int removeCount = listPlotItem.Count - totalItemNum;
                for (int i = 0; i < removeCount; i++)
                {
                    listPlotItem.RemoveAt(listPlotItem.Count - 1);
                }
            }
        }
        #endregion

        #region 初始化剧情条目数据
        int optionIndex = 0;
        for (int i = 0; i < target.list.Count; i++)
        {
            PlotItem plotItem;
            LocalizationSerializedItem locItem = target.list[i];

            if (CheckIsOptionDialogItem(locItem))
            {
                if (CheckIsSkipDialogItem(locItem))
                {
                    optionIndex++;
                    plotItem = listPlotItem[i - optionIndex];
                    plotItem.optionKey2 = locItem.key;
                    plotItem.optionConcent2 = locItem.textEnglish;
                    continue;
                }
                else
                {
                    plotItem = listPlotItem[i - optionIndex];
                    plotItem.dialogType = PlotLobbyDialogType.Option;
                    plotItem.optionKey1 = locItem.key;
                    plotItem.optionContent1 = locItem.textEnglish;
                    continue;
                }
            }

            plotItem = listPlotItem[i - optionIndex];
            plotItem.dialogKey = locItem.key;
            plotItem.content = locItem.textEnglish;
        }
        #endregion


        totalStep = listPlotItem.Count;
        for (int i = 0; i < totalStep; i++)
        {
            listPlotItem[i].OnInitData();
        }
    }

    bool CheckIsOptionDialogItem(LocalizationSerializedItem item)
    {
        string[] parsed = item.key.Split('_');
        string keyOption = parsed[parsed.Length - 2];
        return keyOption == "option";
    }
    bool CheckIsSkipDialogItem(LocalizationSerializedItem item)
    {
        //解析Key，最后两位数为index和subIndex
        string[] parsed = item.key.Split('_');
        string keyOption = parsed[parsed.Length - 2];
        string lastKey = parsed[parsed.Length - 1];

        //如果倒数第二个字符串为option，则认为是玩家选项对话
        bool isOptionDialog = keyOption == "option";
        //如果倒数第一个字符串为2，则认为是玩家选项对话的第二个选项
        bool isOptionTwo = lastKey == "2";

        //如果是对话，同时是第二个选项
        if (isOptionDialog && isOptionTwo)
        {
            return true;
        }
        return false;
    }
#endif
}

[Serializable]
public class PlotItem
{
#if UNITY_EDITOR
    [ReadOnly]
    [VerticalGroup("AvatarLeft")]
    [TableColumnWidth(100, Resizable = false)]
    [HideLabel]
    [PreviewField(48)]
    public Sprite previewAvatarNPC;
    [VerticalGroup("AvatarLeft")]
    [HideLabel]
    [ValueDropdown("GetAvatarNameList")]
    [OnValueChanged("OnUpdateAvatar")]
#endif

    public string avatarNPC = "";

    [VerticalGroup("Key")]
    [HideLabel]
    [TableColumnWidth(180, Resizable = false)]
    [ReadOnly]
    public PlotLobbyDialogType dialogType;
    [VerticalGroup("Key")]
    [ShowIf("dialogType", PlotLobbyDialogType.Dialog)]
    [HideLabel]
    [ReadOnly]
    public string dialogKey;
    [VerticalGroup("Key")]
    [HideLabel]
    [ReadOnly]
    [ShowIf("dialogType", PlotLobbyDialogType.Option)]
    public string optionKey1;

    [VerticalGroup("Key")]
    [HideLabel]
    [ReadOnly]
    [ShowIf("dialogType", PlotLobbyDialogType.Option)]
    public string optionKey2;

#if UNITY_EDITOR
    [ReadOnly]
    [HideLabel]
    [VerticalGroup("Content")]
    [ShowIf("dialogType", PlotLobbyDialogType.Dialog)]
    public string content;

    [ReadOnly]
    [HideLabel]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [ShowIf("dialogType", PlotLobbyDialogType.Option)]
    [VerticalGroup("Content")]
    public string optionContent1;

    [ReadOnly]
    [HideLabel]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [ShowIf("dialogType", PlotLobbyDialogType.Option)]
    [VerticalGroup("Content")]
    public string optionConcent2;
#endif

    public void OnInitData()
    {

    }
#if UNITY_EDITOR
    void OnUpdateAvatar()
    {
        if (!string.IsNullOrEmpty(avatarNPC))
        {
            previewAvatarNPC = GameAsset.GetAssetEditor<Sprite>("icon_" + avatarNPC);
        }
        else
        {
            previewAvatarNPC = null;
        }
        /*
                if (!string.IsNullOrEmpty(avatarNameRight))
                {
                    avatarRight = GameAssets.GetAssetsEditor<Sprite>("icon_" + avatarNameRight);
                }
                else
                {
                    avatarRight = null;
                }*/
    }

    List<string> GetAvatarNameList()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        string path = GameDataControl.GetAssetPath("all_avatar");
        List<AvatarData> listAssets = FileFinder.FindAllAssets<AvatarData>(path);

        foreach (AvatarData item in listAssets)
        {
            listKey.Add(item.avatarName);
        }
        return listKey;
    }

    List<string> GetLocalizationKey()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        string path = GameDataControl.GetLocPath("all_loc");
        List<LocalizationData> listAssets = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData item in listAssets)
        {
            foreach (LocalizationSerializedItem child in item.list)
            {
                listKey.Add(child.key);
            }
        }
        return listKey;
    }
    #endif
}


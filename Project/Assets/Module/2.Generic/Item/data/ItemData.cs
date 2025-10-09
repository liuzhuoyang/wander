using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ItemDataSlot
{
    public ItemData itemData;

    [DisableIf("isRandom")]
    public int num;

    public bool isRandom;

    [ShowIf("isRandom")]
    public int min;
    [ShowIf("isRandom")]
    public int max;
}


[CreateAssetMenu(fileName = "ItemData", menuName = "OniData/Generic/Item/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [ReadOnly, BoxGroup("Info", LabelText = "信息")]
    public string itemName;

    [BoxGroup("Info")]
    public Rarity rarity;

    [LabelText("显示名称"), BoxGroup("Info"), TableColumnWidth(100, Resizable = false), ValueDropdown("GetLocalizationKeyListName")]
    [OnValueChanged("OnDisplayNameChanged")]
    public string displayName;
    
#if UNITY_EDITOR
    [ReadOnly]
    [LabelText("显示名称预览"), BoxGroup("Info"), TableColumnWidth(100, Resizable = false)]
    public string previewDisplayname;
#endif

    [LabelText("描述"), BoxGroup("Info"), TableColumnWidth(100, Resizable = false), ValueDropdown("GetLocalizationKeyListInfo")]
    [OnValueChanged("OnInfoChanged")]
    public string infoKey;

#if UNITY_EDITOR

    [ReadOnly]
    [LabelText("描述预览"), BoxGroup("Info"), TableColumnWidth(100, Resizable = false)]
    public string previewInfo;

    [LabelText("显示图片"), BoxGroup("Info"), HideLabel, PreviewField(55)]
    public Texture2D icon;
#endif
    [LabelText("显示图片名称"), BoxGroup("Info"), ReadOnly]
    public string iconName;
    [LabelText("是否为碎片"), BoxGroup("Info")]
    public bool isShard;

    [LabelText("是否不可见"), BoxGroup("Info")]
    [Tooltip("比如战斗代币，EXP等，不会在背包中显示，也不会获得，只是一个临时的战斗资源，要设置为不可见")]
    public bool isInvisible;

    [BoxGroup("Custom", LabelText = "自定义"), HideIf("isCustomResetTime")]
    public bool isReset;

    [EnableIf("isReset"), BoxGroup("Custom"), HideIf("isCustomResetTime")]
    public TimeResetType timeResetType;
    [BoxGroup("Custom"), LabelText("自定义过期时间"), HideIf("isReset")]
    public bool isCustomResetTime;
    [BoxGroup("Custom"), LabelText("自定义过期时间(天)"), EnableIf("isCustomResetTime"), HideIf("isReset")]
    public int customResetTime;

    [BoxGroup("Effect", LabelText = "效果"), ValueDropdown("GetSFXNameList")]
    public string sfxDrop;

    [BoxGroup("Effect"), ValueDropdown("GetSFXNameList")]
    public string sfxCollect;

    [BoxGroup("Obtain", LabelText = "获取方法")]
    [ValueDropdown("GetNavigatorNameList")]
    public List<string> listNavigatorName;
    

#if UNITY_EDITOR

    #region 初始化数据
    [Button("Init Data", ButtonSizes.Gigantic)]
    void OnInitData()
    {
        itemName = this.name;
        EditorUtility.SetDirty(this);
        iconName = icon == null ? string.Empty : icon.name;
    }
    #endregion

    public bool CheckIsConfigCorrect()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            EditorUtility.DisplayDialog("错误", this.name + " 名字配置错误", "ok");
            return false;
        }

        return true;
    }

    #region 本地化
      // 显示名称列表
    List<string> GetLocalizationKeyListName()
    {
        return GetLocalizationKeyList();
    }

    // 描述列表
    List<string> GetLocalizationKeyListInfo()
    {
        return GetLocalizationKeyList();
    }

    // 获取本地化键列表的通用方法
    List<string> GetLocalizationKeyList()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        string path = GameDataControl.GetLocPath("all_item");
        List<LocalizationData> listAssets = AssetsFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData asset in listAssets)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
    #endregion

    #region 音效
    public List<string> GetSFXNameList()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        string path = GameDataControl.GetAssetPath("all_audio");
        List<AudioData> asset = AssetsFinder.FindAllAssetsOfAllSubFolders<AudioData>(path);
        foreach (AudioData item in asset)
        {
            if (item.clipName.Contains("bgm")) continue;
            listKey.Add(item.clipName);
        }
        return listKey;
    }
    #endregion

    #region Odin辅助
    void OnDisplayNameChanged()
    {
        //更新预览名字
        string path = GameDataControl.GetLocPath("all_item");
        previewDisplayname = LocalizationDataCollection.GetValue(displayName);
    }

    void OnInfoChanged()
    {
        //更新预览描述
        string path = GameDataControl.GetLocPath("all_item");
        previewInfo = LocalizationDataCollection.GetValue(infoKey);
        Debug.Log("更新预览描述: " + previewInfo);
    }

    List<string> GetNavigatorNameList()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        string path = GameDataControl.GetAssetPath("all_navigator");
        List<NavigatorData> listAssets = AssetsFinder.FindAllAssetsOfAllSubFolders<NavigatorData>(path);
        foreach (NavigatorData item in listAssets)
        {
            listKey.Add(item.navigatorName);
        }
        return listKey;
    }
    #endregion
#endif
}
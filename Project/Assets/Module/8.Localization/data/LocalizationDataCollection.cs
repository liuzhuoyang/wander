using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllAchievement来使用
[Serializable]
[CreateAssetMenu(fileName = "all_loc", menuName = "OniData/Localization/LocDataCollection", order = 1)]
public class LocalizationDataCollection : GameDataCollectionBase
{
    public List<LocalizationData> listLocalizationDataGeneric;

    public List<LocalizationData> listLocalizationDataPlot;

    public List<LocalizationData> listLocalizationDataMapping;

    public List<LocalizationData> listLocalizationModule;        //放落到各个模块的本地化数据

    #if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listLocalizationDataGeneric = new List<LocalizationData>();
        listLocalizationDataPlot    = new List<LocalizationData>();
        listLocalizationDataMapping = new List<LocalizationData>();

        listLocalizationModule = new List<LocalizationData>();

        List<LocalizationData> list = AssetsFinder.FindAllAssetsOfAllSubFolders<LocalizationData>("Assets");
        
        foreach (LocalizationData data in list)
        {
            switch(data.type)
            {
                case LocalizationType.Generic:
                    listLocalizationDataGeneric.Add(data);
                    break;
                case LocalizationType.Plot:
                    listLocalizationDataPlot.Add(data);
                    break;
                case LocalizationType.Mapping:
                    listLocalizationDataMapping.Add(data);
                    break;
            }
        }
    }

    #region 编辑器里获取资源使用，比如某个物件需要在编辑器里预览翻译后的名字
    const string ALL_ASSETS_PATH = "Assets/Module/8.Localization/asset/";
    public static LocalizationDataCollection GetData()
    {
        return AssetsFinder.FindAssetByName<LocalizationDataCollection>(ALL_ASSETS_PATH, "all_localization");
    }

    // 获取本地化值, 在编辑器中使用
    public static string GetValue(string localizationKey)
    {
        LocalizationDataCollection localizationDataCollection = GetData();
        List<LocalizationData> listData = localizationDataCollection.listLocalizationDataGeneric
        .Concat(localizationDataCollection.listLocalizationDataPlot)
        .Concat(localizationDataCollection.listLocalizationDataMapping)
        .Concat(localizationDataCollection.listLocalizationModule)
        .ToList();
        
        foreach (LocalizationData data in listData)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                if (item.key == localizationKey)
                {
                    string content = "";
                    if(item.isMapping)
                    {
                        List<string> listMappingContent = new List<string>();
                        for(int i = 0; i < item.mappingList.Count; i++)
                        {
                            foreach (LocalizationSerializedItem mappingItem in GetData().listLocalizationDataMapping.SelectMany(x => x.list))
                            {
                                if(mappingItem.key == item.mappingList[i])
                                {
                                    listMappingContent.Add(mappingItem.textEnglish);
                                    break;
                                }
                            }
                        }

                        string result = string.Format(item.textEnglish, listMappingContent.ToArray());

                        content += $" {item.textEnglish} ({result}) ";
                    }else
                    {
                        content = item.textEnglish;
                    }
                    return content;
                }
            }
        }
        return "";
    }
    #endregion

    #endif
}

public class AllLocalization
{
    public static Dictionary<string, LocalizationArgs> dictLocalizationData; //所有非剧情和Mapping的本地化数据
    public static Dictionary<string, LocalizationArgs> dictLocalizationDataPlot; //剧情本地化数据
    public static Dictionary<string, LocalizationArgs> dictLocalizationDataMapping; //Mapping本地化数据

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        string language = PlayerPrefs.GetString("loc", "en");
        LanguageType languageType = (LanguageType)Enum.Parse(typeof(LanguageType), language);

        dictLocalizationData = new Dictionary<string, LocalizationArgs>();
        dictLocalizationDataPlot = new Dictionary<string, LocalizationArgs>();
        dictLocalizationDataMapping = new Dictionary<string, LocalizationArgs>();

        LocalizationDataCollection localizationDataAll = GameDataControl.Instance.Get("all_localization") as LocalizationDataCollection;
        //GameAsset.GetAssetAsync<LocalizationDataCollection>("all_localization");
            
        
        //初始化非剧情和Mapping的本地化数据，不包含模组分类
        foreach (LocalizationData data in localizationDataAll.listLocalizationDataGeneric)
        {
                foreach (LocalizationSerializedItem item in data.list)
            {
                string content = item.GetContent(languageType);
                dictLocalizationData.Add(item.key, new LocalizationArgs { key = item.key, content = content, mappingList = item.mappingList });
            }
        }

        //初始化模组分类的本地化数据
        //注意这个也是加入到dictLocalizationData中
        foreach (LocalizationData data in localizationDataAll.listLocalizationModule)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                string content = item.GetContent(languageType);
                dictLocalizationData.Add(item.key, new LocalizationArgs { key = item.key, content = content, mappingList = item.mappingList });
            }
        }

        
        try
        {
             //初始化剧情本地化数据
            foreach (LocalizationData data in localizationDataAll.listLocalizationDataPlot)
            {
                foreach (LocalizationSerializedItem item in data.list)
                {
                    string content = item.GetContent(languageType);
                    dictLocalizationDataPlot.Add(item.key, new LocalizationArgs { key = item.key, content = content, mappingList = item.mappingList });
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"=== LocalizationDataCollection: Error processing localization data: {ex.Message} ===");
        }
       

        //初始化Mapping本地化数据
        foreach (LocalizationData data in localizationDataAll.listLocalizationDataMapping)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                string content = item.GetContent(languageType);
                dictLocalizationDataMapping.Add(item.key, new LocalizationArgs { key = item.key, content = content, mappingList = item.mappingList });
            }
        }

        
    }
}

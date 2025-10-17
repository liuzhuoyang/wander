using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using RTSDemo.Basement;
using RTSDemo.Spawn;


#if UNITY_EDITOR
using System.Linq;
using RTSDemo.Unit;
#endif

[Serializable]
public class EnemySpawnData
{
    private UnitRace unitRace;
    public void InitUnitRace(UnitRace unitRace)
    {
        this.unitRace = unitRace;
    }

#if UNITY_EDITOR
    [HideLabel]
    [PreviewField(55)]
    public Sprite previewIcon;
#endif
    [ValueDropdown("GetUnitNameList")]
    public string unitName;

    [TabGroup("波次配置"), LabelText("起始波次")]
    public int startWave = 0;
    [TabGroup("波次配置"), LabelText("波次间隔")]
    public Vector2Int waveIntersect = Vector2Int.zero;
    [TabGroup("波次配置"), LabelText("强制最后一波出现")]
    public bool forceLastWaveSpawn = true;

    [TabGroup("数量配置"), LabelText("基础数量"), PropertyTooltip("实际数量=基础数量+波次+正负1")]
    public int baseCount = 10;

    [TabGroup("生成时间配置"), LabelText("生成延迟")]
    public float delay = 0f;
    [TabGroup("生成时间配置"), LabelText("生成频率")]
    public SpawnRate spawnRate = SpawnRate.Normal;

    [LabelText("生成位置")]
    public SpawnArea spawnArea = SpawnArea.All;

#if UNITY_EDITOR
    void OnUpdateIcon()
    {
        previewIcon = GameAsset.GetAssetEditor<Sprite>("icon_" + unitName);
    }

    public List<string> GetUnitNameList()
    {
        List<string> list = new List<string>();
        list.Add("");
        string path = GameDataControl.GetAssetPath("all_unit");
        List<UnitData> listAsset = FileFinder.FindAllAssetsOfAllSubFolders<UnitData>(path);
        foreach (UnitData asset in listAsset)
        {
            //获取同种族的单位
            UnitRace selectedUnitRace = unitRace;
            if (asset.unitRace == selectedUnitRace)
                list.Add(asset.name);
        }
        return list;
    }
#endif
}

[CreateAssetMenu(fileName = "level_asset", menuName = "OniData/Data/Level/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [ReadOnly]
    [BoxGroup("Info", LabelText = "基础信息")]
    public string levelName;

    [ValueDropdown("GetLocalizationKeyList")]
    [BoxGroup("Info")]
    public string displayName;

    [ReadOnly]
    [BoxGroup("Info")]
    public int levelIndex;

    [BoxGroup("Info")]
    public LevelType levelType;

    [BoxGroup("Info")]
    [ReadOnly]
    [ShowIf("levelType", Value = LevelType.Main)]
    public int chapterID;

    [BoxGroup("Info")]
    [ReadOnly]
    [ShowIf("levelType", Value = LevelType.Main)]
    public int levelID;

    [BoxGroup("Info")]
    [ValueDropdown("GetThemeList")]
    public string themeName;

    [BoxGroup("Info")]
    [ValueDropdown("GetMapNameList")]
    public string mapName;


    #region 外围选关编辑
    [BoxGroup("Meta", LabelText = "外围选关内容编辑")]
    [ValueDropdown("GetThemeVarientList")]
    [OnValueChanged("OnThemeVarientChanged")]
    public int themeVarient;

    public List<int> GetThemeVarientList()
    {
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        return list;
    }

    //这个preview是用来给编辑器更直观地预览当前关卡是哪个主题变种
    [BoxGroup("Meta")]
    [PreviewField(64)]
    [ReadOnly]
    public Sprite previewPic;

    void OnThemeVarientChanged()
    {
        previewPic = GameAsset.GetAssetEditor<Sprite>("pic_" + themeName + "_" + themeVarient);
    }
    #endregion

    // [BoxGroup("Camera", LabelText = "镜头配置")]
    // public LevelCameraPos cameraPos;

    #region 场景配置
    [ValueDropdown("GetFormationList")]
    [BoxGroup("Scene", LabelText = "场景配置")]
    public string formationName;
    [BoxGroup("Scene", LabelText = "场景配置")]
    public BasementData basementData;
    [BoxGroup("Scene")]
    public AssetReferenceGameObject mapPrefab;
    #endregion
    

    #region 难度配置
    [BoxGroup("Difficulity")]
    [ShowIf("levelType", Value = LevelType.Main)]
    public int totalWave = 15;

    [BoxGroup("Difficulity", LabelText = "难度配置")]
    [Tooltip("定义初始单位等级, 难度的基础值，定义关卡的难度基础")]
    public int unitLevel = 1;

    [Tooltip("定义难度系数，每波敌人能力值增长的幅度，定义关卡的难度增幅")]
    [BoxGroup("Difficulity")]
    [ShowIf("levelType", Value = LevelType.Main)]
    public Difficulty difficulty;
    #endregion

    #region 敌人编辑
    [TabGroup("敌人")]
    public UnitRace unitRace;
    [TabGroup("敌人")]
    public List<EnemySpawnData> enemyUnitAssetList;
    #endregion

#region 剧情编辑
#if UNITY_EDITOR
    [TabGroup("剧情")]
    [Title("[通关]后回到大厅的剧情-[序列]-")]
    public PlotSequenceData plotSeqData;
#endif
    
    [TabGroup("剧情")]
    [ReadOnly]
    public string plotSeqID;

#if UNITY_EDITOR
    [TabGroup("剧情")]
    public PlotData plotData;
#endif
    [TabGroup("剧情")]
    [ReadOnly]
    public string plotName;


#if UNITY_EDITOR
    [TabGroup("剧情")]
    [Title("[合成]阶段剧情-[序列]-")]
    [LabelText("剧情序列资源列表")]
    [SerializeField]
    List<PlotSequenceData> listWaveMergePlotSeqData; //触发的序列数据
#endif
    [TabGroup("剧情")]
    //[BoxGroup("[合成]阶段剧情-[序列]-")]
    [LabelText("剧情序列ID列表")]
    [SerializeField]
    [ReadOnly]
    List<string> listWaveMergeSeqID; //触发的序列名字
    
    [TabGroup("剧情")]
    //[BoxGroup("[合成]阶段剧情-[序列]-")]
    [LabelText("触发波段列表")]
    [SerializeField]
    List<int> listWaveMergeSeqIndex; //触发的波段
    
    //
    public Dictionary<int, string> dictWaveMergeSeqID; //这个是游戏中使用的字典，上面是编辑器使用
   

 #if UNITY_EDITOR

    [TabGroup("剧情")]
    [Title("[合成]阶段播放[剧情]")]
    [LabelText("剧情资源列表")]
    [SerializeField]
    List<PlotData> listWaveMergePlotData;

    [TabGroup("剧情")]
    [ReadOnly]
    [LabelText("剧情资源名字列表")]
    [SerializeField]
#endif

    [TabGroup("剧情")]
    List<string> listWaveMergePlotName;

    [TabGroup("剧情")]
    [LabelText("触发波段列表")]
    [SerializeField]
    List<int> listWaveMergePlotIndex;
    public Dictionary<int, string> dictWaveMergePlotName; //这个是游戏中使用的字典，上面是编辑器使用


#if UNITY_EDITOR
    [TabGroup("剧情")]
    [Title("剧情/[战斗]波段剧情-[序列]-")]
    [LabelText("剧情序列资源列表")]
    [SerializeField]
    List<PlotSequenceData> listWaveFightPlotSeqData; //触发的序列数据
#endif
    [TabGroup("剧情")]
    [LabelText("剧情序列ID列表")]
    [ReadOnly]
    [SerializeField]
    List<string> listWaveFightSeqID; //触发的序列名字

    [TabGroup("剧情")]
    [LabelText("触发波段列表")]
    [SerializeField]
    List<int> listWaveFightSeqIndex; //触发的波段
    public Dictionary<int, string> dictWaveFightSeqID; //这个是游戏中使用的字典，上面是编辑器使用
   

#if UNITY_EDITOR

    [TabGroup("剧情")]
    [Title("剧情/[战斗]波段播放[剧情]")]
    [LabelText("剧情资源列表")]
    [SerializeField]
    List<PlotData> listWaveFightPlotData;
#endif

    [TabGroup("剧情")]
    [ReadOnly]
    [LabelText("剧情资源名字列表")]
    [SerializeField]
    List<string> listWaveFightPlotName;

    [TabGroup("剧情")]
    [LabelText("触发波段列表")]
    [SerializeField]
    List<int> listWaveFightPlotIndex;
    public Dictionary<int, string> dictWaveFightPlotName; //这个是游戏中使用的字典，上面是编辑器使用
   
#endregion

    //初始化运行数据
    //字典无法序列化，要根据列表初始化才能正确使用
    public void Init()
    {
        dictWaveFightSeqID = ConvertListToDictionary(listWaveFightSeqIndex, listWaveFightSeqID);
        dictWaveMergeSeqID = ConvertListToDictionary(listWaveMergeSeqIndex, listWaveMergeSeqID);
        dictWaveFightPlotName = ConvertListToDictionary(listWaveFightPlotIndex, listWaveFightPlotName);
        dictWaveMergePlotName = ConvertListToDictionary(listWaveMergePlotIndex, listWaveMergePlotName);
    }
    
     //将列表转换为字典
    private Dictionary<int, string> ConvertListToDictionary(List<int> keys, List<string> values)
    {
        var dictionary = new Dictionary<int, string>();
        if (keys == null || values == null || keys.Count != values.Count)
        {
            return dictionary;
        }

        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }

        return dictionary;
    }


#if UNITY_EDITOR
    public List<string> GetFormationList()
    {
        List<string> list = new List<string>();
        string path = GameDataControl.GetAssetPath("all_formatian");
        List<FormatianData> listAsset = FileFinder.FindAllAssetsOfAllSubFolders<FormatianData>(path);
        foreach (FormatianData asset in listAsset)
        {
            list.Add(asset.formatianName);
        }
        return list;
    }
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


    [Button("初始化数据", ButtonSizes.Gigantic)]
    public void InitData()
    {
        GameDataCollection gameDataCollection = GameDataControl.GetGameDataCollectionEDITOR();
        LevelDataCollection levelDataCollection = gameDataCollection.dictGameData["all_level"] as LevelDataCollection;
        levelDataCollection.OnInitLevelIndex();
        
        levelName = this.name;
        if (levelName.Contains("000_00"))
        {
            //新手关卡
            levelID = 0;
            chapterID = 0;
        }
        else
        {
            levelID = int.Parse(this.name.Split("_")[3].TrimStart('0'));
            chapterID = int.Parse(levelName.Split("_")[2].TrimStart('0'));
        }

        //合成阶段剧情
        InitializePlotData(ref listWaveMergePlotIndex, ref listWaveMergePlotName, listWaveMergePlotData, data => data.plotName);
        //战斗阶段剧情
        InitializePlotData(ref listWaveFightPlotIndex, ref listWaveFightPlotName, listWaveFightPlotData, data => data.plotName);
        //战斗阶段剧情序列
        InitializePlotData(ref listWaveFightSeqIndex, ref listWaveFightSeqID, listWaveFightPlotSeqData, data => data.plotSeqID);
        //合成阶段剧情序列
        InitializePlotData(ref listWaveMergeSeqIndex, ref listWaveMergeSeqID, listWaveMergePlotSeqData, data => data.plotSeqID);

        //给每个资源加入种族数据，方便在选择中过滤单位，只能选择当前种族单位
        foreach (EnemySpawnData data in enemyUnitAssetList)
        {
            data.InitUnitRace(unitRace);
        }
    }

    //初始化剧情数据
    private void InitializePlotData<T>(ref List<int> listPlotIndex, 
    ref List<string> listPlotName, 
    List<T> listPlotData, 
    Func<T, string> getNameFunc)
    {
        //如果剧情索引列表为空，则初始化剧情索引列表
        if (listPlotIndex == null)
        {
            listPlotIndex = new List<int>();
        }

        //如果剧情数据列表不为空，则初始化剧情名字列表
        if (listPlotData != null && listPlotData.Count > 0)
        {
            //如果剧情索引列表比剧情数据列表少，则添加剧情索引
            for (int i = listPlotIndex.Count; i < listPlotData.Count; i++)
            {
                listPlotIndex.Add(i);
            }

            //初始化剧情名字列表
            listPlotName = listPlotData.Select(getNameFunc).ToList();
        }

        //如果剧情名字列表比剧情数据列表多，则删除多余的剧情名字
        if(listPlotName.Count > listPlotData.Count)
        {
            listPlotName.RemoveRange(listPlotData.Count, listPlotName.Count - listPlotData.Count);
        }
    }

    public List<string> GetMapNameList()
    {
        List<string> list = new List<string>();
        list.Add("");
        string path = GameDataControl.GetLocPath("all_level");
        List<string> listFileName = FileFinder.FindAllFilesOfAllSubFolders(path);
        foreach (string fileNme in listFileName)
        {
            list.Add(fileNme);
        }
        return list;
    }

     // 获取本地化键列表的通用方法
    List<string> GetLocalizationKeyList()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        string path = GameDataControl.GetLocPath("all_level");
        List<LocalizationData> listAssets = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData asset in listAssets)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
#endif
}


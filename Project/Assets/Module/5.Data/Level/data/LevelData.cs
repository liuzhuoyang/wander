using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using onicore.editor;
using System.Linq;
#endif

[Serializable]
public class EnemyUnitData
{
    string unitRace;
    public void InitUnitRace(string unitRace)
    {
        this.unitRace = unitRace;
    }

#if UNITY_EDITOR
    //[ReadOnly]
    [PreviewField(55)]
    //[OnValueChanged("OnUpdateIcon")]
    //[HorizontalGroup("Split", 55)]
    [HideLabel]
    public Sprite previewIcon;
#endif

    //[VerticalGroup("Split/Ver")]
    public int unlockWave;

    //[OnValueChanged("OnUpdateIcon")]
    //[VerticalGroup("Split/Ver")]
    public string unitName;

#if UNITY_EDITOR
    void OnUpdateIcon()
    {
        previewIcon = GameAsset.GetAssetEditor<Sprite>("icon_" + unitName);
    }
#endif

}

[CreateAssetMenu(fileName = "level_asset", menuName = "OniData/Data/Level/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [ReadOnly]
    [BoxGroup("Info", LabelText = "基础信息")]
    public string levelName;

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

    [BoxGroup("Camera", LabelText = "镜头配置")]
    public LevelCameraPos cameraPos;

    #region 难度配置
    [BoxGroup("Difficulity")]
    [ShowIf("levelType", Value = LevelType.Main)]
    public int totalWave;

    [BoxGroup("Difficulity", LabelText = "难度配置")]
    [Tooltip("定义初始单位等级, 难度的基础值，定义关卡的难度基础")]
    public int unitLevel;

    /*
        [BoxGroup("Difficulity")]
        [Tooltip("单位能力值变化")]
        public float unitLevelGrowth;
    */

    [Tooltip("定义难度系数，每波敌人能力值增长的幅度，定义关卡的难度增幅")]
    [BoxGroup("Difficulity")]
    [ShowIf("levelType", Value = LevelType.Main)]
    public Difficulty difficulty;


    [BoxGroup("Scene")]
    public GameObject mapPrefab;

    //未来写地图相关的参数设置
    


    [BoxGroup("Scene")]
    [ValueDropdown("GetFormatianList")]
    public string formatianName;

    #endregion

    #region 敌人编辑


    [TabGroup("敌人")]
    [ValueDropdown("GetRaceList")]
    [ShowIf("levelType", Value = LevelType.Main)]

    [TabGroup("敌人")]
    public string unitRace;
    [ShowIf("levelType", Value = LevelType.Main)]



    [TabGroup("敌人")]
    [ValueDropdown("")]
    [OnValueChanged("OnUpdateBossIcon")]
    public string bossName;

#if UNITY_EDITOR    
    [TabGroup("敌人")]
    [ReadOnly]
    [PreviewField(55)]
    public Sprite previewBossIcon;

    void OnUpdateBossIcon()
    {
        if (!string.IsNullOrEmpty(bossName))
        {
            previewBossIcon = GameAsset.GetAssetEditor<Sprite>("icon_" + bossName);
        }
    }

    [TabGroup("敌人")]
    public List<EnemyUnitData> enemyUnitAssetList;

    public List<string> GetBossNameList()
    {
        //等有单位后添加，只加入Boss
        List<string> list = new List<string>();
        list.Add("");
        return list;
    }

#endif

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

    public List<string> GetFormatianList()
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
        foreach (EnemyUnitData data in enemyUnitAssetList)
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
        if (listPlotName.Count > listPlotData.Count)
        {
            listPlotName.RemoveRange(listPlotData.Count, listPlotName.Count - listPlotData.Count);
        }
    }


    public List<string> GetRaceList()
    {
        List<string> list = new List<string>();

        string path = GameDataControl.GetAssetPath("all_race");
        List<ThemeData> listAsset = FileFinder.FindAllAssetsOfAllSubFolders<ThemeData>(path);
        foreach (ThemeData asset in listAsset)
        {
            list.Add(asset.themeName);
        }
        return list;
    }
#endif
}



using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.IO;

[Serializable]
[CreateAssetMenu(fileName = "ChapterData", menuName = "OniData/Data/Chapter/ChapterData", order = 1)]
public class ChapterData : ScriptableObject
    {
        [ReadOnly]
        public int chapterID;

        [ReadOnly]
        public string chapterName;

        [ValueDropdown("GetChapterDisplayNameList")]
        public string displayName;

        [ValueDropdown("GetThemeList")]
        public string themeName;
        
        [ReadOnly]
        public int totalLevel;

        [ReadOnly]
        public List<LevelData> listLevelAssets = new List<LevelData>();

#if UNITY_EDITOR
        [Button("初始化数据", ButtonSizes.Large)]
        public void InitData()
        {
            chapterName = this.name;
            planetName = $"planet_{themeName}_{planetVarient}";

            if(chapterName == "chapter_000")
            {
                chapterID = 0;
            }else
            {
                chapterID = int.Parse(chapterName.Split("_")[1].TrimStart('0'));
            }
           

            string folderName = "normal/" + chapterID.ToString("D3");
            string path = GameDataControl.GetAssetPath("all_level");
            totalLevel = OniEditorUtility.GetFileCount($"{path}_{folderName}");

            //剧情数据
            if(plotData != null)
            {
                plotName = plotData.plotName;
            }else
            {
                plotName = "";
            }
            if(plotSequenceData != null)
            {
                plotSequenceID = plotSequenceData.plotSeqID;
            }else
            {
                plotSequenceID = "";
            }

            listLevelAssets = FileFinder.FindAllAssets<LevelData>(path);
        }
#endif

        [BoxGroup("Display")]
        [ReadOnly]
        public string planetName;

        [BoxGroup("Display")]
        [ValueDropdown("listPlanetVarient")]
        [OnValueChanged("OnPlanetVarientChanged")]
        public int planetVarient;

        #if UNITY_EDITOR
        List<int> listPlanetVarient = new List<int>() { 1, 2, 3 };
        private void OnPlanetVarientChanged()
        {
            planetName = $"planet_{themeName}_{planetVarient}";
            previewPlanet = GameAsset.GetAssetEditor<Sprite>("pic_" + planetName);
        }
        
        [ReadOnly]
        [BoxGroup("Display")]
        [PreviewField(100)]
        public Sprite previewPlanet;
        #endif

        #if UNITY_EDITOR
        [BoxGroup("Plot")]
        public PlotData plotData;
        #endif

        [BoxGroup("Plot")]
        [ReadOnly]
        public string plotName;

        #if UNITY_EDITOR
        [BoxGroup("Plot")]
        public PlotSequenceData plotSequenceData;
        #endif

        [BoxGroup("Plot")]
        [ReadOnly]
        public string plotSequenceID;

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

        public List<string> GetChapterDisplayNameList()
        {
            List<string> list = new List<string>();
            list.Add("");
            string path = GameDataControl.GetLocPath("all_chapter");
            List<LocalizationData> listAsset = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
            foreach (LocalizationData asset in listAsset)
            {
                foreach (LocalizationSerializedItem item in asset.list)
                {
                    list.Add(item.key);
                }
            }
            return list;
        }
#endif
    }



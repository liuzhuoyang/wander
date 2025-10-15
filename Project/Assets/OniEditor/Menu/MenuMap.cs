#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

namespace onicore.editor
{
    public class MenuMap
    {
        static bool toggleInit = false;
        static bool toggleOpen = false;

        [InfoBox("点击开启编辑器，游戏会进入运行状态，再次点击关闭运行状态。编辑地图时，用这个按钮来开启/关闭游戏。不要使用自带的启动按钮开关游戏")]
        [HideIf("toggleInit")]
        [Button("开启地图编辑器", ButtonSizes.Gigantic)]
        public void InitMapEditor()
        {
            if (toggleInit)
            {
                Debug.LogError("=== MenuMap: 编辑器已经启动 ===");
                return;
            }

            if (SceneManager.GetActiveScene().name != "map_editor")
            {
                Debug.LogError("=== MenuMap: 切换到map_editor场景打开编辑器 ===");
                return;
            }

            //启动编辑器
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = true;
                
                ResetData();
                //运行游戏会重置参数，这里要在运行后初始化一次
                EditorData.Reset();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            if (SceneManager.GetActiveScene().name != "map_editor")
            {
                return;
            }

            toggleInit = true;

            if (EditorControl.Instance != null) return;
            GameObject editControl = new GameObject("[Edit Control]");
            editControl.AddComponent<EditorControl>().InitEditorControl();
        }


        [Button("关闭地图编辑器", ButtonSizes.Large)]
        [ShowIf("toggleInit")]
        public void OnCloseEditor()
        {
            if (EditorControl.Instance != null)
                EditorControl.Instance.OnKill();

            toggleInit = false;
            toggleOpen = false;
            levelData = null;
            //关闭编辑器
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
        }


        #region 重置数据
        void ResetData()
        {

        }
        #endregion

        [ShowIf("toggleInit")]
        [BoxGroup("BoxSelect", Order = 1, ShowLabel = false)]
        [TitleGroup("BoxSelect/选择地图")]
        [ValueDropdown("GetLevelDataList")]
        [OnValueChanged("OnValueChangedLevelData")]
        public LevelData levelData;

        void OnValueChangedLevelData()
        {
            EditorData.currentLevelData = levelData;
            selectedMap = levelData.mapName;
            mapType = levelData.levelType;
        }

        public List<LevelData> GetLevelDataList()
        {
            string path = GameDataControl.GetAssetPath("all_level");
            return FileFinder.FindAllAssetsOfAllSubFolders<LevelData>(path);
        }

        [BoxGroup("BoxSelect")]
        [ShowIf("toggleInit")]
        [TitleGroup("BoxSelect/选择地图")]
        [ReadOnly]
        public string selectedMap = "";

        [ReadOnly]
        [ShowIf("toggleInit")]
        [BoxGroup("BoxSelect")]
        [TitleGroup("BoxSelect/选择地图")]
        public LevelType mapType = LevelType.Main;

        [ShowIf("toggleInit")]
        [DisableIf("toggleOpen")]
        [BoxGroup("BoxSelect")]
        [HorizontalGroup("BoxSelect/Split")]
        [Button("打开地图", ButtonSizes.Large)]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 5), PropertyOrder(0)]
        public async void OpenMap()
        {
            toggleOpen = true;
            Debug.Log("=== MapAssertMenu: 打开地图:" + selectedMap + " ===");
            
            await MapControl.Instance.OpenLevel(levelData, true);
        }

        [ShowIf("toggleInit")]
        [EnableIf("toggleOpen")]
        [BoxGroup("BoxSelect")]
        [HorizontalGroup("BoxSelect/Split")]
        [Button("关闭地图", ButtonSizes.Large)]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 5), PropertyOrder(0)]
        public void OnCloseMap()
        {
            toggleOpen = false;
        }

        [ShowIf("toggleInit")]
        [EnableIf("toggleOpen")]
        [BoxGroup("BoxSelect")]
        [Button("保存地图", ButtonSizes.Large)]
        public void OnSaveMap()
        {
            ReadWrite.OnWriteMapJsonFile(levelData);
        }

        [BoxGroup("BoxTool", Order = 10)]
        [Button("打开地图编辑工具页面", ButtonSizes.Large)]
        [ShowIf("toggleInit")]
        public void OnMapTool()
        {
            // EditMapTool.OpenWindow();
        }

        [BoxGroup("BoxTool", Order = 10)]
        [Button("打开地形编辑工具页面", ButtonSizes.Large)]
        [ShowIf("toggleInit")]
        public void OnTerrainTool()
        {
            // EditTerrainTool.OpenWindow();
        }

    }
}
#endif
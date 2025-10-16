#if UNITY_EDITOR

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace onicore.editor
{
    public class EditTerrainTool : OdinWindowBase
    {
        public static void OpenWindow()
        {
            GetWindow<EditTerrainTool>().Show();
        }

        public override void Reset()
        {
            base.Reset();
            listTerrainHoleAssetGroup.Clear();
            listTerrainSurfaceAssetGroup.Clear();
            listTerrainDetailAssetGroup.Clear();
            listTerrainCliffAssetGroup.Clear();
            listTerrainScatterAssetGroup.Clear();
            listTerrainDecorAssetGroup.Clear();
            listTerrainBlockAssetGroup.Clear();
            listTerrainSharedAssetGroup.Clear();
        }

        #region 资源列表

        [TabGroup("洞 Hole", Order = 0)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainHoleAssetGroup = new List<TerrainAssetGroup>();

        [TabGroup("表层 Surface", Order = 1)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainSurfaceAssetGroup = new List<TerrainAssetGroup>();

        [TabGroup("细节 Detail", Order = 2)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainDetailAssetGroup = new List<TerrainAssetGroup>();

        [TabGroup("悬崖 Cliff", Order = 3)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainCliffAssetGroup = new List<TerrainAssetGroup>();

        [TabGroup("散落 Scatter", Order = 4)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainScatterAssetGroup = new List<TerrainAssetGroup>();

        [TabGroup("装饰 Decor", Order = 5)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainDecorAssetGroup = new List<TerrainAssetGroup>();

        [TabGroup("阻挡 Block", Order = 6)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainBlockAssetGroup = new List<TerrainAssetGroup>();

        [BoxGroup("共用 Shared", Order = 7)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> listTerrainSharedAssetGroup = new List<TerrainAssetGroup>();

        #endregion

        #region 初始化资源
        [BoxGroup("Init", Order = 0, ShowLabel = true, LabelText = "初始化当前场景资源")]
        [Button("Init Terrain Assets 初始化资源", ButtonHeight = 48), GUIColor(0.4f, 0.9f, 1f)]
        public void OnInit()
        {
            string currentTheme = EditorData.currentLevelData.themeName;

            if (string.IsNullOrEmpty(currentTheme))
            {
                EditorUtility.DisplayDialog("错误", "当前地图未启动，无法获取地图主题名字", "确定");
                return;
            }

            LoadThemeAssets(currentTheme, listTerrainHoleAssetGroup, TerrainLayer.Hole);
            LoadThemeAssets(currentTheme, listTerrainSurfaceAssetGroup, TerrainLayer.Surface);
            LoadThemeAssets(currentTheme, listTerrainDetailAssetGroup, TerrainLayer.Detail);
            LoadThemeAssets(currentTheme, listTerrainCliffAssetGroup, TerrainLayer.Cliff);
            LoadThemeAssets(currentTheme, listTerrainScatterAssetGroup, TerrainLayer.Scatter);
            LoadThemeAssets(currentTheme, listTerrainDecorAssetGroup, TerrainLayer.Decor);
            LoadThemeAssets(currentTheme, listTerrainBlockAssetGroup, TerrainLayer.Block);

            //读取共用资源
            LoadAssets(EditorPathUtility.editorTerrainSpritePath + "/shared", listTerrainSharedAssetGroup, TerrainLayer.Block);
        }

        void LoadThemeAssets(string currentTheme, List<TerrainAssetGroup> terrainItemList, TerrainLayer terrainType)
        {
            string blockPath = EditorPathUtility.editorTerrainSpritePath + currentTheme + "/" + (int)terrainType + "_" + terrainType.ToString().ToLower() + "/";
            LoadAssets(blockPath, terrainItemList, terrainType);
        }
        void LoadAssets(string blockPath, List<TerrainAssetGroup> terrainItemList, TerrainLayer terrainType)
        {
            terrainItemList.Clear();
            List<Sprite> listBlockSprite = FileFinder.FindAllSprites(blockPath);
            List<List<Sprite>> listGroup = OniEditorUtility.GroupItemGeneric<Sprite>(listBlockSprite);
            for (int i = 0; i < listGroup.Count; i++)
            {
                TerrainAssetGroup terrainAsset = new TerrainAssetGroup();
                terrainItemList.Add(terrainAsset);

                //点击回调
                List<Action> listAction = new List<Action>();
                foreach (Sprite sprite in listGroup[i])
                {
                    TerrainObject item = new TerrainObject();
                    item.itemName = sprite.name;
                    item.terrainLayer = terrainType;
                    listAction.Add(item.OnSelect);
                }

                terrainAsset.Init(listGroup[i], listAction);
            }
        }
        #endregion
    }
}
#endif

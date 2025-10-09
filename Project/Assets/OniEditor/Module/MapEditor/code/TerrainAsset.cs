#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace onicore.editor
{
    public class TerrainAsset : ScriptableObject
    {
        [BoxGroup("Basic", ShowLabel = false, Order = 0)]
        [HorizontalGroup("Basic/Split")]
        public string path;

        [BoxGroup("Basic")]
        [PropertySpace(SpaceBefore = 5, SpaceAfter = 10), PropertyOrder(0)]
        [HorizontalGroup("Basic/Split")]
        [Button("更新资源", ButtonSizes.Large)]
        void Load()
        {
            //LoadAssets("mid", mid, TerrainLayer.Mid);
           // LoadAssets("front", front, TerrainLayer.Front);
        }

        #region
        [TabGroup("中景")]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> mid = new List<TerrainAssetGroup>();

        [TabGroup("前景")]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> front = new List<TerrainAssetGroup>();

        [TabGroup("背景", Order = 1)]
        [TableList(ScrollViewHeight = 550, AlwaysExpanded = true)]
        public List<TerrainAssetGroup> back = new List<TerrainAssetGroup>();

        #endregion

        void LoadAssets(string id, List<TerrainAssetGroup> terrainItemList, TerrainLayer terrainType)
        {
            /*
            terrainItemList.Clear();
            List<Sprite> listSprite = new List<Sprite>();
            if(terrainType == TerrainLayer.Front)
            {
                listSprite = AssetsFinder.FindAllSprites(EditorPathUtility.terrainSpritePath + "shared/front");
            }
            else
            {
                listSprite = AssetsFinder.FindAllSprites(EditorPathUtility.terrainSpritePath + path + "/" + terrainType.ToString().ToLower());
            }

            if (!OniEditorUtility.CheckSpriteNaming(listSprite))
                return;

            List<List<Sprite>> listGroup = OniEditorUtility.GroupItemGeneric<Sprite>(listSprite);

            for (int i = 0; i < listGroup.Count; i++)
            {
                TerrainAssetGroup terrainItem = new TerrainAssetGroup();
                terrainItemList.Add(terrainItem);

                //点击回调
                List<Action> listAction = new List<Action>();
                foreach (Sprite sprite in listGroup[i])
                {
                    TerrainObject item = new TerrainObject();
                    item.itemName = sprite.name;
                    item.terrainLayer = terrainType;
                    listAction.Add(item.OnSelect);
                }

                terrainItem.Init(listGroup[i], listAction);
            }*/
        }
    }

    [Serializable]
    public class TerrainObject
    {
        public string itemName;
        public TerrainLayer terrainLayer;
        
        public void OnSelect()
        {
            EditTargetTerrainArgs args = new EditTargetTerrainArgs();
            args.targetName = itemName;
            args.editSprite = itemName;
            args.mapObjectType = MapObjectType.Terrain;
            args.terrainLayer = terrainLayer;

            //Tile层级需要对齐网格
            if(terrainLayer == TerrainLayer.Hole)
            {
                args.isSnapToGrid = true;
                args.gridSize = 2;
            }
            
            InputEditor.Instance.OnSelect(args);
        }
    }

    [Serializable]
    public class TerrainAssetGroup : ISelectableAsset
    {
        public void OnSelect()
        {

        }
        /*
        public void InitSelectableArgs(TerrainType terrainType)
        {
            base.actorType = ActorType.terrain;
            base.terrainType = terrainType;
        }*/
    }
}



#endif

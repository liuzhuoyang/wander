#if UNITY_EDITOR

using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace onicore.editor
{
    [HideLabel]
    [Serializable]
    public class EditMapTool : OdinWindowBase
    {
        public static void OpenWindow()
        {
            GetWindow<EditMapTool>().Show();
        }

        public override void Reset()
        {
            base.Reset();
            toggleErase = false;
            toggleEdit = false;
            OnReset();
        }
        
        #region 橡皮擦工具
        bool toggleErase = false;
        [HideIf("toggleErase")]
        [DisableIf("toggleEdit")]
        [FoldoutGroup("EditGroup", Order = 0, GroupName = "物件编辑工具")]
        [HorizontalGroup("EditGroup/Action")]
        [Button("开启橡皮擦功能", ButtonHeight = 48), GUIColor(0.4f, 0.9f, 1f)]
        public void OnErase()
        {
             // 开启橡皮擦编辑工具，左键确认，右键取消
            InputEditor.Instance.OnEnableEdit(handle => handle.OnErase(), () => Reset());
            toggleErase = true;
        }

        [ShowIf("toggleErase")]
        [HorizontalGroup("EditGroup/Action")]
        [Button("关闭橡皮擦功能", ButtonHeight = 48), GUIColor(1f, 0.3f, 0.3f)]
        public void DisableErase()
        {
            InputEditor.Instance.OnDisableEdit();
            toggleErase = false;
        }
        #endregion

        #region 物件编辑工具
        [FoldoutGroup("EditGroup")]
        bool toggleEdit = false;

        [DisableIf("toggleErase")]
        [HideIf("toggleEdit")]
        [HorizontalGroup("EditGroup/Action")]
        [Button("开启物件编辑", ButtonHeight = 48), GUIColor(0.4f, 0.9f, 1f)]
        public void OnEdit()
        {
            // 开启物件编辑工具，左键确认，右键取消，背景物件不需要被编辑，过滤Terrain层，=
            InputEditor.Instance.OnEnableEdit(handle => handle.OnEdit(), () => Reset(), LayerMask.GetMask("Terrain"));
            toggleEdit = true;
        }

        [ShowIf("toggleEdit")]
        [HorizontalGroup("EditGroup/Action")]
        [Button("关闭物件编辑工具", ButtonHeight = 48), GUIColor(1f, 0.3f, 0.3f)]
        public void DisableEdit()
        {
            InputEditor.Instance.OnDisableEdit();
            toggleEdit = false;
        }
        #endregion

/*
        #region 地形编辑工具
        bool toggleTerrain = false;
        [HideIf("toggleTerrain")]
        [DisableIf("toggleEdit")]
        [HorizontalGroup("EditGroup/Action")]
        [Button("开启地形编辑", ButtonHeight = 48), GUIColor(0.4f, 0.9f, 1f)]
        public void OnTerrainEdit()
        {
            InputEditor.Instance.OnEnableEdit(handle => handle.OnEdit(), () => Reset(), LayerMask.GetMask("Default"));
            toggleTerrain = true;
        }

        [ShowIf("toggleTerrain")]
        [HorizontalGroup("EditGroup/Action")]
        [Button("关闭地形编辑", ButtonHeight = 48), GUIColor(1f, 0.3f, 0.3f)]
        public void DisableTerrainEdit()
        {
            InputEditor.Instance.OnDisableEdit();
            toggleTerrain = false;
        }
        #endregion
*/

        #region 寻路编辑工具
        bool toggleNodeAvailable = false;
        bool toggleNodeBlock = false;

        [FoldoutGroup("NodeGroup", GroupName = "寻路编辑工具")]
        [FoldoutGroup("NodeGroup/节点编辑")]
        [HorizontalGroup("NodeGroup/节点编辑/Availability")]
        [Button("可用节点", ButtonHeight = 48)]
        [DisableIf("toggleNodeAvailable")]
        public void OnNodeAvailable()
        {
            toggleNodeAvailable = true;
            toggleNodeBlock = false;

            InputEditor.Instance.OnDisableNodeInput();
            InputEditor.Instance.OnEnableNodeInput((worldPos)=>
            {
                Debug.Log("=== EditMapTool: 设置可用节点 ===");
                MapNodeControl.Instance.SetNodeAvailable(worldPos);
            });
        }

        [FoldoutGroup("NodeGroup/节点编辑")]
        [VerticalGroup("NodeGroup/节点编辑/Availability/ver")]
        [HideLabel]
        [Range(0, 3)] // 使用 Range 属性
        public int nodeEditRange = 0;
        [FoldoutGroup("NodeGroup/节点编辑")]
        [VerticalGroup("NodeGroup/节点编辑/Availability/ver")]
        [Button("不可用节点", ButtonHeight = 48)]
        [DisableIf("toggleNodeBlock")]
        public void OnNodeBlock()
        {
            toggleNodeAvailable = false;
            toggleNodeBlock = true;

            InputEditor.Instance.OnDisableNodeInput();
            InputEditor.Instance.OnEnableNodeInput((worldPos)=>
            {
                Debug.Log($"=== EditMapTool: 设置不可用节点 {worldPos.x},{worldPos.y} ===");
                MapNodeControl.Instance.SetNodeBlock(worldPos, nodeEditRange);
            });
        }

        [FoldoutGroup("NodeGroup/碰撞编辑")]
        [HorizontalGroup("NodeGroup/碰撞编辑/hor")]
        [Button("放置碰撞", ButtonHeight = 48)]
        public void OnPlaceCollider()
        {
            EditTargetArgs args = new EditTargetArgs();
            args.isSnapToGrid = true;
            args.targetName = "collider";
            args.editSprite = "edit_collider";
            args.mapObjectType = MapObjectType.Collider;
            InputEditor.Instance.OnSelect(args);
        }

        [FoldoutGroup("NodeGroup/显示隐藏")]
        [HorizontalGroup("NodeGroup/显示隐藏/Display")]
        [Button("隐藏节点Gizmos", ButtonHeight = 48)]
        public void OnHideNodeGizmos()
        {
            MapNodeControl.Instance.OnHideNodeGizmos();
        }

        [FoldoutGroup("NodeGroup/显示隐藏")]
        [HorizontalGroup("NodeGroup/显示隐藏/Display")]
        [Button("显示节点Gizmos", ButtonHeight = 48)]
        public void OnShowNodeGizmos()
        {
            MapNodeControl.Instance.OnShowNodeGizmos();
        }

        [FoldoutGroup("NodeGroup/工具")]
        [HorizontalGroup("NodeGroup/工具/Tool")]
        [Button("重新计算场流方向", ButtonHeight = 48)]
        public void OnRecalculateFlowField()
        {
            MapNodeControl.Instance.OnRecalculateFlowField();
        }

        [FoldoutGroup("NodeGroup/工具")]
        [HorizontalGroup("NodeGroup/工具/Tool")]
        [Button("清除所有节点", ButtonHeight = 48)]
        public void OnClearAllNode()
        {
            MapNodeControl.Instance.OnClearAllNode();
        }

        public void OnNodeEditorClose()
        {
            MapNodeControl.Instance.OnDisableMapNode();
        }

        #endregion

        #region 出生点编辑工具
        [FoldoutGroup("SpawnGroup", GroupName = "出生点编辑工具")]
        [HorizontalGroup("SpawnGroup/Action")]
        [Button("设置出生点", ButtonHeight = 48)]
        public void OnSetSpawnPoint()
        {
            EditTargetArgs args = new EditTargetArgs();
            args.targetName = "spawn_point";
            args.editSprite = "edit_spawn_point";
            args.mapObjectType = MapObjectType.SpawnPoint;
            InputEditor.Instance.OnSelect(args);
        }
        #endregion

        [FoldoutGroup("TowerDefenseGroup", GroupName = "防御塔编辑工具")]
        [HorizontalGroup("TowerDefenseGroup/Action")]
        [Button("设置防御塔", ButtonHeight = 48)]
        public void OnSetDefenseTowerPoint()
        {
            EditTargetArgs args = new EditTargetArgs();
            args.targetName = "defenseTower_point";
            args.editSprite = "edit_defenseTower_point";
            args.mapObjectType = MapObjectType.TowerDefensePoint;
            InputEditor.Instance.OnSelect(args);
        }

        #region 补给点编辑工具
        /*
        [BoxGroup("SupplyGroup", GroupName = "补给点编辑工具")]
        [Button("设置一个补给点", ButtonHeight = 48)]
        public void OnSetSupplyPoint()
        {
            EditTargetArgs args = new EditTargetArgs();
            args.targetName = "supply_point";
            args.editSprite = "edit_supply_point";
            args.mapObjectType = MapObjectType.SupplyPoint;
            InputEditor.Instance.OnSelect(args);
        }*/
        #endregion

        [BoxGroup("ResetGroup", Order = 10, ShowLabel = false)]
        [Button("重置", ButtonHeight = 48)]
        public void OnReset()
        {
            toggleErase = false;
            toggleEdit = false;
            //toggleTerrain = false;
            //if(InputEditor.Instance != null)
                //InputEditor.Instance.OnDisableEdit();

            if(InputEditor.Instance != null)
                InputEditor.Instance.OnDisableNodeInput();
            toggleNodeAvailable = false;
            toggleNodeBlock = false;

            OniEditorUtility.ClearAllSceneViewEvents();
        }
    }
}

#endif

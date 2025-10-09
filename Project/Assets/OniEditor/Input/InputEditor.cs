#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace onicore.editor
{
    [ExecuteInEditMode]
    public class InputEditor : Singleton<InputEditor>
    {
        bool isSelecting;           //选中了物件,执行更新
        bool isSnapToGrid;         //是否对齐网格
        int gridSize = 1;          //网格大小
        EditInputSelector selector;
        Action<EditTargetArgs> callbackConfirm;
        Action callbackCancel;

        void OnDestroy()
        {
            Clear();
            OniEditorUtility.ClearAllSceneViewEvents();
        }

        private void Start()
        {
            Clear();
        }

        void Clear()
        {
            isSelecting = false;
            selector = null;
            callbackConfirm = null;
            callbackCancel = null;
        }

        #region 放置输入监听
        public void OnEnable()
        {
            Debug.Log("=== InputEditor: 开启输入控制器 ===");
            SceneView.duringSceneGui += OnUpdate;
            //EditorApplication.update += OnEditorUpdate;
        }

        public void OnDisable()
        {
            Debug.Log("=== InputEditor: 关闭输入控制器 ===");
            SceneView.duringSceneGui -= OnUpdate;
            //EditorApplication.update -= OnEditorUpdate;
            Clear();
        }
        #endregion

        void OnUpdate(SceneView sceneView)
        {
            if (!isSelecting || sceneView == null || SceneCamera == null) return;

            // 获取鼠标位置并转换为世界坐标
            Vector2 fixedMousePosition;
            GetScreenMousePosition(Event.current.mousePosition, sceneView, out fixedMousePosition);
            Vector2 mouseWorldPosition = SceneCamera.ScreenToWorldPoint(fixedMousePosition);

            // 调试日志
            //Debug.Log($"Mouse position: {fixedMousePosition}");

            // 处理鼠标输入
            HandleMouseInput(mouseWorldPosition);
        }

        void HandleMouseInput(Vector2 mouseWorldPosition)
        {
            var mouse = Mouse.current;

            // 左键点击
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Debug.Log("=== InputEditor: 鼠标左键点击 ===");
                OnConfirm(mouseWorldPosition);
                Event.current.Use();  // 标记事件已处理
                return;
            }
            
            // 鼠标移动
            if (Event.current.delta != Vector2.zero)
            {
                CheckMove(mouseWorldPosition);
            }

            // 右键点击
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                Debug.Log("=== InputEditor: 鼠标右键点击 ===");
                OnDeselect();
                Event.current.Use();  // 标记事件已处理
                return;
            }
        }

        public void CheckMove(Vector2 fixedMousePosition)
        {
            if(isSnapToGrid)
            {
                //对齐到网格，格子大小为2x2
                Vector2 gridPosition = new Vector2(Mathf.Round(fixedMousePosition.x / gridSize) * gridSize, Mathf.Round(fixedMousePosition.y / gridSize) * gridSize) + new Vector2(0.5f, 0.5f);
                selector.transform.position = gridPosition;
            }
            else
            {
                selector.transform.position = fixedMousePosition;
            }
        }

        #region 交互
        //确认选择
        void OnConfirm(Vector2 moustToWorldPosition)
        {
            Debug.Log("=== InputEditor: on click pos" + moustToWorldPosition);

            Vector2 targetPosition = moustToWorldPosition;
            if(isSnapToGrid)
            {
                //targetPosition = new Vector2(Mathf.Round(moustToWorldPosition.x / 1), Mathf.Round(moustToWorldPosition.y / 1));
                targetPosition = new Vector2(Mathf.Round(moustToWorldPosition.x / gridSize) * gridSize, Mathf.Round(moustToWorldPosition.y / gridSize) * gridSize) + new Vector2(0.5f, 0.5f);
            }
            MapControl.Instance.OnClickAction(targetPosition, selector.args);

            callbackConfirm?.Invoke(selector.args);
        }

        //点击菜单里物件的放置按钮，根据类型创建选择物件
        public void OnSelect<T>(T targetArgs, Action<EditTargetArgs> callbackConfirm = null, Action callbackCancel = null) where T : EditTargetArgs
        {
            Clear();
            GameObject go;
            
            //edit_input_selector 是一个通用对象，点击任何需要在编辑地图交互，需要跟随鼠标显示的，都会创建这个对象，再应用图片。告诉当前的是什么，在什么位置等信息
            go = Instantiate(GameAsset.GetAssetEditor<GameObject>("edit_input_selector"), transform) as GameObject;
            selector = go.GetComponent<EditInputSelector>();
            selector.Init(targetArgs);

            isSelecting = true;

            //对齐到网格
            isSnapToGrid = targetArgs.isSnapToGrid;

            this.callbackConfirm = callbackConfirm;
            this.callbackCancel = callbackCancel;
        }

        public void OnDeselect()
        {
            if (selector != null)
                DestroyImmediate(selector.gameObject);
            foreach (Transform t in transform)
                DestroyImmediate(t.gameObject);

            callbackCancel?.Invoke();

            Clear();
            Debug.Log("=== InputEditor: 取消选中 ===");
        }
        #endregion

        RaycastHit2D lastHit = new RaycastHit2D();
        #region 编辑监听事件
        Action<EditHandle> onConfirm;
        Action onCancel;
        LayerMask layerMask;  // 层级过滤字段
        
        public void OnEnableEdit(Action<EditHandle> onConfrim, Action onCancel, LayerMask layerMask = default)
        {
            this.onConfirm = onConfrim;
            this.onCancel = onCancel;
            this.layerMask = layerMask;
            Debug.Log("=== InputEditor: 开启编辑输入控制器 ===");
            SceneView.duringSceneGui += OnEditUpdate;
        }

        public void OnDisableEdit()
        {
            Debug.Log("=== InputEditor: 关闭编辑输入控制器 ===");
            SceneView.duringSceneGui -= OnEditUpdate;

            if (lastHit.collider != null)
                lastHit.collider.GetComponent<EditHandle>().OnDeselect();
        }

        void OnEditUpdate(SceneView sceneView)
        {
            Event e = Event.current;
            
            // 处理鼠标移动
            if (e.type == EventType.MouseMove)
            {
                RaycastHit2D hit = GetHit(sceneView);
                
                // 处理碰撞体变化
                if (hit.collider != lastHit.collider)
                {
                    if (lastHit.collider != null)
                        lastHit.collider.GetComponent<EditHandle>().OnDeselect();
                    
                    lastHit = hit;
                }
                
                // 处理悬停
                if (lastHit.collider != null)
                    lastHit.collider.GetComponent<EditHandle>().OnHoveOver();
            }

            // 处理鼠标点击
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0 && lastHit.collider != null)
                {
                    onConfirm?.Invoke(lastHit.collider.GetComponent<EditHandle>());
                    e.Use();
                }
                else if (e.button == 1)
                {
                    OnDisableEdit();
                    onCancel();
                    e.Use();
                }
            }
        }
        #endregion
        
        #region 寻路编辑监听事件
        Action<Vector2> onNodeConfirm;
        public void OnEnableNodeInput(Action<Vector2> onConfrim)
        {
            onNodeConfirm = onConfrim;
            Debug.Log("=== InputEditor: 开启寻路节点输入控制器 ===");
            SceneView.duringSceneGui += OnNodeInputUpdate;
        }

        public void OnDisableNodeInput()
        {
            Debug.Log("=== InputEditor: 关闭寻路节点输入控制器 ===");
            MapNodeDebugControl.Instance.OnReset();
            SceneView.duringSceneGui -= OnNodeInputUpdate;
        }

        void OnNodeInputUpdate(SceneView sceneView)
        {
            Event e = Event.current;
            Vector2 fixedMousePosition;
            GetScreenMousePosition(Event.current.mousePosition, sceneView, out fixedMousePosition);
            Vector2 mouseToWorldPosition = new Vector2(SceneCamera.ScreenToWorldPoint(fixedMousePosition).x, SceneCamera.ScreenToWorldPoint(fixedMousePosition).y);
            MapNodeDebugControl.Instance.OnMouse(MapNodeControl.Instance.GetNodeX(mouseToWorldPosition), MapNodeControl.Instance.GetNodeY(mouseToWorldPosition));
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Debug.Log("=== InputEditor: mouse left click ===");
                onNodeConfirm?.Invoke(mouseToWorldPosition);
                e.Use();
            }
            else if (e.type == EventType.MouseDown && e.button == 1)
            {
                Debug.Log("=== InputEditor: mouse right click ===");
                OnDisableNodeInput();
                e.Use();
            }
        }
        #endregion
        
        #region 辅助方法
        void GetScreenMousePosition(Vector2 mousePosition, SceneView sceneView, out Vector2 fixedPosition)
        {
            float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            mousePosition.x *= pixelsPerPoint;
            mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * pixelsPerPoint;//鼠标位置校正
            fixedPosition = new Vector2(mousePosition.x, mousePosition.y);
        }


        private static Camera sceneCamera;
        private static Camera SceneCamera
        {
            get
            {
                if (sceneCamera == null)
                {
                    sceneCamera = SceneView.currentDrawingSceneView.camera;
                }
                return sceneCamera;
            }
        }

        //获取鼠标碰撞的第一个物件
        RaycastHit2D GetHit(SceneView sceneView)
        {
            var mouse = Event.current.mousePosition;
            float pixelPerPoint = EditorGUIUtility.pixelsPerPoint;
            mouse.x *= pixelPerPoint;
            mouse.y = sceneView.camera.pixelHeight - mouse.y * pixelPerPoint;
            var worldPosition = SceneCamera.ScreenToWorldPoint(mouse);
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPosition, Vector2.zero, Mathf.Infinity, ~layerMask);
            RaycastHit2D firstHit = new RaycastHit2D();
            if (hits.Length > 0)
            {
                firstHit = hits[0];
            }
            return firstHit;
        }
        #endregion
    }
}
#endif
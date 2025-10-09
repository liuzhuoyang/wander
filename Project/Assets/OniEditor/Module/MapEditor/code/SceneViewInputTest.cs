#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SceneViewInputTest : EditorWindow
{
    [MenuItem("Window/SceneView Input Test")]
    public static void ShowWindow()
    {
        GetWindow<SceneViewInputTest>("Input Test");
    }

    private void OnEnable()
    {
        // 注册SceneView事件
        SceneView.duringSceneGui += OnSceneGUI;
        Debug.Log("SceneView Input Test Enabled");
    }

    private void OnDisable()
    {
        // 取消注册SceneView事件
        SceneView.duringSceneGui -= OnSceneGUI;
        Debug.Log("SceneView Input Test Disabled");
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // 输出基本事件信息
        Debug.Log($"Event Type: {e.type}, Mouse Position: {e.mousePosition}");

        // 处理鼠标点击
        if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
            {
                Debug.Log("Left Mouse Click");
            }
            else if (e.button == 1)
            {
                Debug.Log("Right Mouse Click");
            }
        }

        // 处理鼠标移动
        if (e.type == EventType.MouseMove)
        {
            Debug.Log("Mouse Moved");
        }

        // 处理键盘事件
        if (e.type == EventType.KeyDown)
        {
            Debug.Log($"Key Pressed: {e.keyCode}");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("SceneView Input Test", EditorStyles.boldLabel);
        if (GUILayout.Button("Clear Console"))
        {
            Debug.ClearDeveloperConsole();
        }
    }
}
#endif
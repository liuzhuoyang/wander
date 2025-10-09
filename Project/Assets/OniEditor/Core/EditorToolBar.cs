#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

namespace onicore.editor
{
    [ExecuteInEditMode]
    public class EditorToolBar : OdinEditorWindow
    {
        [MenuItem("Onicore/Editor Tool Bar")]
        private static void Open()
        {
            var window = GetWindow<EditorToolBar>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(500, 800);
        }

        protected override void OnBeginDrawEditors()
        {
            SirenixEditorGUI.Title("Tool Bar", "", TextAlignment.Center, true);
            //在工具栏下开始一部分方法
            SirenixEditorGUI.BeginHorizontalToolbar(36);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  开Preload场景  ")))
                {
                    OpenPreloadScene();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  开Main场景  ")))
                {
                    OpenMainScene();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  开Edit场景  ")))
                {
                    OpenEditScene();
                }
            }
            //结束工具栏告知
            SirenixEditorGUI.EndHorizontalToolbar();

            SirenixEditorGUI.Title("Asset Edit Tool 资源编辑辅助工具 ", "", TextAlignment.Center, true);
            SirenixEditorGUI.BeginHorizontalToolbar(36);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  更新CSV  ")))
                {
                    OnUpdateCSV();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();


            SirenixEditorGUI.Title("Level Edit Tool 关卡编辑辅助工具 ", "", TextAlignment.Center, true);
            SirenixEditorGUI.BeginHorizontalToolbar(36);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  设置战斗场景相机  ")))
                {
                    OnSetCameraFight();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  设置战斗Boss场景相机  ")))
                {
                    OnSetCameraFightBoss();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  设置合成场景相机  ")))
                {
                    OnSetCameraMerge();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        void OpenPreloadScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/preload.unity");
        }
        private void OpenMainScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/main.unity");
        }

        private void OpenEditScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/map_editor.unity");
        }

        void OnSetCameraFight()
        {
            Camera.main.orthographicSize = 15;
            Camera.main.transform.position = new Vector3(0, GetCameraPosOffsetY(), -10);
        }

        float GetCameraPosOffsetY()
        {
            float offsetY = 0;
            switch (EditorData.currentLevelAsset.cameraPos)
            {
                case LevelCameraPos.Top:
                    offsetY = 5;
                    break;
                case LevelCameraPos.Middle:
                    offsetY = 0.5f; 
                    break;
                case LevelCameraPos.Bottom:
                    offsetY = -5;
                    break;
            }
            return offsetY;
        }

        void OnSetCameraFightBoss()
        {
            Camera.main.orthographicSize = 18;
            Camera.main.transform.position = new Vector3(0, GetCameraPosOffsetY(), -10);
        }

        void OnSetCameraMerge()
        {
            Camera.main.orthographicSize = 8;
            Camera.main.transform.position = new Vector3(0, -1.25f, -10);
        }

        void OnUpdateCSV()
        {
            CSVWriter.OnUpdateCSV();
        }
    }
}
#endif

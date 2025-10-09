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
    public class OniEditor : OdinMenuEditorWindow
    {
        [MenuItem("Onicore/Oni Editor")]
        private static void Open()
        {
            var window = GetWindow<OniEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(500, 800);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Map",  new MenuMap()},
            };

            string terrainAssetsPath = "Assets/EditorAssets/terrain/";
            var terrainItems = AssetDatabase.FindAssets("t:TerrainAsset", new[] { terrainAssetsPath })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<TerrainAsset>(path))
                .Where(asset => asset != null)
                .OrderBy(asset => asset.name) // Sort by name
                .ToList();

            foreach (var item in terrainItems)
            {
                tree.Add("Terrain/" + item.name, item);
            }

            string path = GameDataControl.GetAssetPath("all_level");
            var levelItems = AssetDatabase.FindAssets("t:LevelAssetData", new[] { path })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<LevelData>(path))
                .Where(asset => asset != null)
                .OrderBy(asset => asset.name) // Sort by name
                .ToList();

            foreach (var item in levelItems)
            {
                tree.Add("Level/" + item.name, item);
            } 

            return tree;
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
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("  开Edit场景  ")))
                {
                    OpenEditScene();
                }

            }
            //结束工具栏告知
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        void OpenPreloadScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/preload.unity");
        }

        private void OpenEditScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/map_editor.unity");
        }
        
        void OnUpdateCSV()
        {
            CSVWriter.OnUpdateCSV();
        }
    }
}


#endif
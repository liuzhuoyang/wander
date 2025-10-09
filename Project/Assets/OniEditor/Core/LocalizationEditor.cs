#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace onicore.editor
{
    [ExecuteInEditMode]
    public class LocalizationEditor : OdinMenuEditorWindow
    {
        const string ASSETS_PATH = "Assets/Module/8.Localization/asset/";
        [MenuItem("Onicore/Localization")]
        private static void Open()
        {
            var window = GetWindow<LocalizationEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(500, 800);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Menu",                           new MenuLocalization()                                                },
            };

            #region ui组，根据文件夹归类
            string locAssetUIPath = ASSETS_PATH + "ui/";
            var locAssetUI = AssetDatabase.FindAssets("t:LocalizationData", new[] { locAssetUIPath })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => new { Asset = AssetDatabase.LoadAssetAtPath<LocalizationData>(path), Path = path })
                .Where(item => item.Asset != null)
                .GroupBy(item => "ui/" + GetSubFolderPath(item.Path, ASSETS_PATH)) // 保留 "ui/" 前缀
                .ToDictionary(group => group.Key, group => group.OrderBy(item => item.Asset.name).Select(item => item.Asset).ToList());

            foreach (var folder in locAssetUI)
            {
                foreach (var item in folder.Value)
                {
                    tree.Add(folder.Key + "/" + item.name, item); // 保留 "ui/" 前缀并拼接子文件夹
                }
            }

            string GetSubFolderPath(string fullPath, string baseFolder)
            {
                var relativePath = fullPath.Replace(baseFolder + "/", ""); // 去掉baseFolder
                var folderParts = relativePath.Split('/');
                if (folderParts.Length > 1)
                {
                    return string.Join("/", folderParts.Take(folderParts.Length - 1)); // 返回完整的子目录结构
                }
                return ""; // 如果没有子文件夹，返回空字符串
            }
            #endregion

            string locAssetMappingPath = ASSETS_PATH + "mapping/";
            var locAssetMapping = AssetDatabase.FindAssets("t:LocalizationData", new[] { locAssetMappingPath })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => new { Asset = AssetDatabase.LoadAssetAtPath<LocalizationData>(path), Path = path })
                .Where(item => item.Asset != null)
                .GroupBy(item => "mapping/" + GetSubFolderPath(item.Path, ASSETS_PATH)) // 保留 "ui/" 前缀
                .ToDictionary(group => group.Key, group => group.OrderBy(item => item.Asset.name).Select(item => item.Asset).ToList());

            foreach (var folder in locAssetMapping)
            {
                foreach (var item in folder.Value)
                {
                    tree.Add(folder.Key + "/" + item.name, item); // 保留 "ui/" 前缀并拼接子文件夹
                }
            }

            string locAssetPlotPath = ASSETS_PATH + "plot/";
            var locAssetPlot = AssetDatabase.FindAssets("t:LocalizationData", new[] { locAssetPlotPath })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<LocalizationData>(path))
                .Where(asset => asset != null)
                .OrderBy(asset => asset.name) // Sort by name
                .ToList();

            foreach (var item in locAssetPlot)
            {
                tree.Add("plot/" + item.name, item);
            }

            return tree;
        }


        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }
    }
}
#endif
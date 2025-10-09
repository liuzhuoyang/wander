#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;
using System.Linq;
public static class OniEditorUtility
{
    public static List<List<T>> GroupItemGeneric<T>(List<T> items, int groupSize = 5)
    {
        List<List<T>> groupedItems = new List<List<T>>();
        List<T> currentGroup = new List<T>();

        foreach (T item in items)
        {
                currentGroup.Add(item);

                if (currentGroup.Count == groupSize)
                {
                    groupedItems.Add(new List<T>(currentGroup));
                    currentGroup.Clear();
                }
            }

            if (currentGroup.Count > 0)
            {
                groupedItems.Add(currentGroup);
            }

            return groupedItems;
        }

        //获取目录下的文件数量
        public static int GetFileCount(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly);
                // 过滤掉 .meta 文件
                int fileCount = files.Count(file => !file.EndsWith(".meta"));

                foreach (string file in files)
                {
                    if (!file.EndsWith(".meta"))
                    {
                        Debug.Log(file);
                    }
                }

                return fileCount;
            }
            else
            {
                Debug.LogError("目录不存在: " + directoryPath);
                return 0;
            }
        }

        /// <summary>
        /// 检查sprite名字是否正确
        /// </summary>
        public static bool CheckSpriteNaming(List<Sprite> listSprites)
        {
            foreach (Sprite sprite in listSprites)
            {
                string targetName = sprite.name;
                // Check if the name is not already in lowercase or contains spaces
                if (targetName != targetName.ToLower() || targetName.Trim() != targetName || targetName.Contains(" "))
                {
                    EditorUtility.DisplayDialog("错误", "文件名存在大小写或空格错误: " + targetName, "ok");
                    return false; // Stop checking further if an error is found
                }
                return true;
            }
            Debug.Log("=== OnicoreEditorUtility: all sprites naming correct ===");
            return true;
        }

        //移动场景到目标位置
        public static void MoveSceneViewCamera(Vector3 position, float size)
        {
            // Get the current SceneView or create one if none exists
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null)
            {
                SceneView.FrameLastActiveSceneView();
                sceneView = SceneView.lastActiveSceneView;
            }

            if (sceneView != null)
            {
                // Set the position and size
                sceneView.LookAt(position, sceneView.rotation, size);
            }
        }

        public static void ClearAllSceneViewEvents()
        {
            Debug.Log("=== OniEditorUtility: ClearAllSceneViewEvents ===");
            // 获取 SceneView 的 duringSceneGui 事件字段
            FieldInfo eventField = typeof(SceneView).GetField("duringSceneGui", BindingFlags.Static | BindingFlags.NonPublic);
            if (eventField != null)
            {
                // 获取事件的委托
                Delegate eventDelegate = (Delegate)eventField.GetValue(null);
                if (eventDelegate != null)
                {
                    // 获取所有已注册的事件处理程序
                    foreach (Delegate handler in eventDelegate.GetInvocationList())
                    {
                        // 使用 System.Action 替代过时的 OnSceneFunc
                        SceneView.duringSceneGui -= (System.Action<SceneView>)handler;
                    }
                }
            }
        }
    }
#endif
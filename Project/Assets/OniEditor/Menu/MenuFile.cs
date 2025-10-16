#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

namespace onicore.editor
{
    public class MenuFile
    {
        [Button("创建关卡地图模板Json文件", ButtonSizes.Gigantic)]
        public void OnCreateMapJson()
        {
            string path = GameDataControl.GetAssetPath("all_level");
            path = path.Replace("asset", "map");
            MapJsonData levelRawData = new MapJsonData();
            ReadWrite.CreateLevelJsonFile(path, "map_template", levelRawData);
        }
    }
}
#endif
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace onicore.editor
{
    public class EditorControl : Singleton<EditorControl>
    {
        /// <summary>
        /// 初始化地图模块编辑器控制器
        /// </summary>
        public void InitEditorControl()
        {
            GameObject csvLoader = new GameObject("[CSVLoader]");
            csvLoader.AddComponent<CSVLoader>();

            //await GameData.Init();
       
            Debug.Log("=== EditorControl: init editor ===");
            gameObject.AddComponent<InputEditor>();

            GameObject map = new GameObject("Map");
            map.AddComponent<MapControl>().Init();

            editorGizmosView = new GameObject("Gizmos").AddComponent<EditorGizmosView>();
        }

        public void OnKill()
        {
            Destroy(gameObject);
        }

        public void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DateTime now = DateTime.Now;
                string timestamp = now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filename = EditorPathUtility.screenshotPath + timestamp + ".png";
                ScreenCapture.CaptureScreenshot(filename, 2);
            }*/
        }

        EditorGizmosView editorGizmosView;
        public async UniTask CreateGizmosObject<T>(Vector2 pos, string targetName, string spriteName, Type handleType, T args) where T : class
        {
            await editorGizmosView.CreateView(pos, targetName, spriteName, handleType, args);
        }
    }
}
#endif

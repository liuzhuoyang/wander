#if UNITY_EDITOR

namespace onicore.editor
{
    using System.Collections;
    using System.Collections.Generic;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;


    public class OdinWindowBase : OdinEditorWindow
    {
        #region 自动初始化方法
        protected override void OnEnable()
        {
            base.OnEnable();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                Reset();
            }
        }

        public virtual void Reset()
        {

        }
        #endregion
    }
}

#endif
#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using UnityEditor;

namespace onicore.editor
{
    public class ItemDrawer<TItem> : OdinValueDrawer<TItem>
            where TItem : ItemData
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {

            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if (label != null)
            {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            }
            else
            {
                rect = EditorGUI.IndentedRect(rect);
            }

            ItemData item = this.ValueEntry.SmartValue;
            Texture2D texture = null;

            if (item)
            {
                texture = GUIHelper.GetAssetThumbnail(item.icon, typeof(TItem), true);
                texture.alphaIsTransparency = true;
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : item.name);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), item, texture, this.ValueEntry.BaseValueType);
        }
    }
}
#endif

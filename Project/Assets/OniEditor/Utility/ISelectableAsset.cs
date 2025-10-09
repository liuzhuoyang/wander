using UnityEngine;  
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
namespace onicore.editor
{
    public class ISelectableAsset
    {
        List<Action> listAction;
        public void Init(List<Sprite> listSprites, List<Action> listAction)
        {
            this.listAction = listAction;
            for (int i = 0; i < listSprites.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        icon1 = listSprites[i];
                        break;
                    case 1:
                        icon2 = listSprites[i];
                        break;
                    case 2:
                        icon3 = listSprites[i];
                        break;
                    case 3:
                        icon4 = listSprites[i];
                        break;
                    case 4:
                        icon5 = listSprites[i];
                        break;
                }
            }
        }

        [BoxGroup("BoxGroup1", ShowLabel = false)]
        [HideLabel]
        [TableColumnWidth(100, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100)]
        public Sprite icon1;

        [BoxGroup("BoxGroup1", ShowLabel = false)]
        [Button("放置", ButtonSizes.Medium)]
        void OnSelectIcon1()
        {
            OnSelect(1);
        }

        [BoxGroup("BoxGroup2", ShowLabel = false)]
        [HideLabel]
        [TableColumnWidth(100, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100)]
        public Sprite icon2;

        [BoxGroup("BoxGroup2")]
        [Button("放置", ButtonSizes.Medium)]
        void OnSelectIcon2()
        {
            OnSelect(2);
        }

        [BoxGroup("BoxGroup3", ShowLabel = false)]
        [HideLabel]
        [TableColumnWidth(100, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100)]
        public Sprite icon3;

        [BoxGroup("BoxGroup3")]
        [Button("放置", ButtonSizes.Medium)]
        void OnSelectIcon3()
        {
            OnSelect(3);
        }

        [BoxGroup("BoxGroup4", ShowLabel = false)]
        [HideLabel]
        [TableColumnWidth(100, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100)]
        public Sprite icon4;

        [BoxGroup("BoxGroup4")]
        [Button("放置", ButtonSizes.Medium)]
        void OnSelectIcon4()
        {
            OnSelect(4);
        }

        [BoxGroup("BoxGroup5", ShowLabel = false)]
        [HideLabel]
        [TableColumnWidth(100, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100)]
        public Sprite icon5;

        [BoxGroup("BoxGroup5")]
        [Button("放置", ButtonSizes.Medium)]
        void OnSelectIcon5()
        {
            OnSelect(5);
        }

        void OnSelect(int index)
        {
            int id = index - 1;

            if(listAction == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("需要初始化资源", "点击一次更新资源按钮", "ok");
                return;
            }

            listAction[id]?.Invoke();
        }
    }
}
#endif
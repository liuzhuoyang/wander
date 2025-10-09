#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Sirenix.OdinInspector;
using System;

namespace onicore.editor
{
    /// <summary>
    /// 编辑器编辑用，跟随鼠标的显示物件
    /// </summary>
    public class EditInputSelector : MonoBehaviour
    {
        public EditTargetArgs args;
        public SpriteRenderer render;

        public async void Init(EditTargetArgs args)
        {
            this.args = args;
            render.sprite = await GameAsset.GetSpriteAsync(args.editSprite);
        }
    }

    public class EditTargetArgs
    {
        [HideInInspector]
        //编辑器里跟随的图片
        public string editSprite = "";

        public MapObjectType mapObjectType;

        [HideInInspector]
        public string targetName = "";

        [HideInInspector]
        public bool isSnapToGrid = false;
        [HideInInspector]
        public int gridSize = 1;
    }

    public class EditTargetTerrainArgs : EditTargetArgs
    {
        public TerrainLayer terrainLayer;
        public int rotateZ;
    }
}

#endif
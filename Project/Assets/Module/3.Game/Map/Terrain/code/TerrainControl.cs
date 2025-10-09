using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TerrainControl : Singleton<TerrainControl>
{
    public TerrainView terrainView;
    public void Init()
    {
        terrainView = gameObject.AddComponent<TerrainView>();
        terrainView.Init(transform);
        terrainView.transform.SetParent(this.transform);
    }

    public async UniTask<GameObject> CreateViewObject(Vector2 pos, string spriteName, TerrainLayer terrainType)
    {
        return await terrainView.CreateView(pos, spriteName, terrainType);
    }

/*
    public async UniTask CreateBG(string targetName, float offsetX, float offsetY)
    {
        await terrainView.CreateView(new Vector2(offsetX, offsetY), "ter_bg_" + targetName, TerrainLayer.BG);
    }
*/
/*
    public void SetSize(float size)
    {
        terrainView.SetSize(size);
    }*/
}

using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class TerrainView : MonoBehaviour
{
    Dictionary<TerrainLayer, Transform> terrainDict;

    public void Init(Transform parent)
    {
        terrainDict = new Dictionary<TerrainLayer, Transform>();
        foreach (TerrainLayer terrainLayer in Enum.GetValues(typeof(TerrainLayer)))
        {
            // 检查是否已存在该key
            if (!terrainDict.ContainsKey(terrainLayer))
            {
                GameObject groupObj = new GameObject(terrainLayer.ToString().ToLower());
                groupObj.transform.SetParent(parent);
                terrainDict.Add(terrainLayer, groupObj.transform);
            }
        }
    }

    public async UniTask<GameObject> CreateView(Vector2 pos, string spriteName, TerrainLayer terrainLayer)
    {
        GameObject viewObj;
        viewObj = Instantiate(await GameAsset.GetPrefabAsync("map_terrain"), transform);

        SpriteRenderer render = viewObj.GetComponent<SpriteRenderer>();
        AssignGroupAndOrder(viewObj.transform, terrainLayer, render);
        viewObj.transform.position = pos;
        viewObj.GetComponent<SpriteRenderer>().sprite = await GameAsset.GetSpriteAsync(spriteName);

#if UNITY_EDITOR
        //编辑器场景创建碰撞体
        if (SceneManager.GetActiveScene().name == "map_editor")
        {
            //非背景地形添加碰撞体，用于删除检查
            if (terrainLayer != TerrainLayer.Tile)
            {
                //添加EditHandleTerrain类型编辑器控制组件
                viewObj.AddComponent<EditHandleTerrain>().AddPoligonCollider2D();
                viewObj.GetComponent<EditHandleTerrain>().Init(spriteName, terrainLayer);
                //控制期加入到编辑列表，后续数据用于保存地图
                MapControl.Instance.LevelData.AddTerrainHandle(viewObj.GetComponent<EditHandleTerrain>());
            }
        }

        viewObj.name = terrainLayer.ToString() + "|" + spriteName + "|" + pos.x + "," + pos.y;
#endif
        return viewObj;
    }

    /// <summary>
    /// 配置组和渲染层级
    /// </summary>
    void AssignGroupAndOrder(Transform transform, TerrainLayer terrainLayer, SpriteRenderer render)
    {
        //设置父级
        transform.SetParent(terrainDict[terrainLayer]);
        //render.sortingLayerName = "terrain";
        switch (terrainLayer)
        {
            case TerrainLayer.Block:
                //block层级使用默认层级,和unit会有层级交互
                //render.sortingLayerName = "Default";
                render.sortingLayerName = "terrain_top";
                //退出，不设置排序
                return;
            default:
                break;
        }
        //根据TerrainLayer设置排序
        render.sortingOrder = (int)terrainLayer;
    }
}

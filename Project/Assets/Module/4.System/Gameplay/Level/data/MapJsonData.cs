using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using onicore.editor;
#endif

public class MapJsonData
{
    public int chapterID;
    public int levelID;
    public string levelType;
    public string themeName;

    public Dictionary<string, Dictionary<string, List<string>>> terrainData = new Dictionary<string, Dictionary<string, List<string>>>();
    public Dictionary<string, List<string>> vfxData = new Dictionary<string, List<string>>();

    //节点数据,前面节点是权重，后面是x_y的格式
    //public Dictionary<byte, List<string>> nodeData = new Dictionary<byte, List<string>>();
    //碰撞节点数据
    //public List<string> colliderData = new List<string>();
    
#if UNITY_EDITOR

    //编辑器场景创建的编辑器控制组件列表
    List<EditHandleTerrain> listTerrainHandle = new List<EditHandleTerrain>();
    List<EditHandleVFX> listVFXHandle = new List<EditHandleVFX>();
    List<EditHandleSpawnPoint> listSpawnPointHandle = new List<EditHandleSpawnPoint>();
    //读取编辑器场景创建的编辑器控制组件列表，定义需要写入的数据结构
    public void InitEditorMapData()
    {
        terrainData = new Dictionary<string, Dictionary<string, List<string>>>();

        foreach (var item in listTerrainHandle)
        {
            var pos = item.transform.position;
            AddTerrain((float)Math.Round(pos.x, 2), (float)Math.Round(pos.y, 2), item.targetName, item.terrainLayer);
        }

        foreach (var item in listVFXHandle)
        {
            var pos = item.transform.position;
            AddVFX((float)Math.Round(pos.x, 2), (float)Math.Round(pos.y, 2), item.targetName);
        }
    }

    #region 地形创建与移除
    public void AddTerrainHandle(EditHandleTerrain handle)
    {
        listTerrainHandle.Add(handle);
    }

    public void RemoveTerrainHandle(EditHandleTerrain handle)
    {
        listTerrainHandle.Remove(handle);
    }

    /// <summary>
    /// 添加Terrain到数据，结构：Dictionay<类型, Dictionary<物件名字, 坐标列表>>
    /// </summary>
    public void AddTerrain(float posX, float posY, string targetName, TerrainLayer terrainType)
    {
        string type = terrainType.ToString();
        if (!terrainData.ContainsKey(type))
        {
            terrainData.Add(type, new Dictionary<string, List<string>>());
        }

        if (!terrainData[type].ContainsKey(targetName))
        {
            List<string> listPosition = new List<string>();
            terrainData[type].Add(targetName, listPosition);
        }

        terrainData[type][targetName].Add(posX + "," + posY);
    }
    #endregion

    #region VFX创建与移除
    public void AddVFXHandle(EditHandleVFX handle) => listVFXHandle.Add(handle);
    public void RemoveVFXHandle(EditHandleVFX handle) => listVFXHandle.Remove(handle);

    public void AddVFX(float posX, float posY, string targetName)
    {
        if (!vfxData.ContainsKey(targetName))
        {
            List<string> listPosition = new List<string>();
            vfxData.Add(targetName, listPosition);
        }

        vfxData[targetName].Add(posX + "," + posY);
    }
    #endregion

/*
    #region 出生点创建与移除
    public void AddSpawnPointHandle(EditHandleSpawnPoint handle)
    {
        listSpawnPointHandle.Add(handle);
    }
    public void RemoveSpawnPointHandle(EditHandleSpawnPoint handle) => listSpawnPointHandle.Remove(handle);
    // 加入到出生点数据
    void AddSpawnPoint(SpawnPointArgs args)
    {
        spawnPointData.Add(args);
    }

    /*
        public void AddSupplyPointHandle(EditHandleSupplyPoint handle) => listSupplyPointHandle.Add(handle);
        public void RemoveSupplyPointHandle(EditHandleSupplyPoint handle) => listSupplyPointHandle.Remove(handle);*/

    /*
        // 通用方法添加功能点
        void AddFeaturePoint<T>(FeaturePointType type, T args) where T : FeaturePointArgs
        {
            string key = type.ToString();
            if (!spawnPointData.ContainsKey(key))
            {
                spawnPointData.Add(key, new List<FeaturePointArgs>());
            }
            spawnPointData[key].Add(args);
        }*/



    /*
        // 创建补给点
        void AddSupplyPoint(SupplyPointArgs args) => AddFeaturePoint(FeaturePointType.Supply, args);
        
    #endregion

/*
    #region 防御塔创建与移除
    public void AddTowerDefensePointHandle(EditHandleTowerDefensePoint handle) => listTowerDefensePointHandle.Add(handle);
    public void RemoveTowerDefensePointHandle(EditHandleTowerDefensePoint handle) => listTowerDefensePointHandle.Remove(handle);
    #endregion
    */

    /// <summary>
    /// 验证地图数据是否有效
    /// 主要检查是否包含Boss出生点
    /// </summary>
    /// <returns>true表示数据有效，false表示数据存在问题</returns>
    public bool ValidateMapData()
    {
        /*
        // 检查是否包含Boss出生点
        bool isContainBossSpawnPoint = spawnPointData.Any(item => item.spawnPointType == SpawnPointRouteType.Boss);

        if (!isContainBossSpawnPoint) // 如果没有找到Boss出生点
        {
            // 弹出错误对话框
            EditorUtility.DisplayDialog("错误", "Boss出生点数据不存在", "ok"); 
            return false; 
        }*/

        //检验没有问题
        return true; 
    }

#endif


    #region 节点创建与移除
    public static void AddNodeToDict(ref Dictionary<byte, List<string>> dict, byte cost, int x, int y)
    {
        if (!dict.ContainsKey(cost)) { dict[cost] = new List<string>(); }
        dict[cost].Add(x + "," + y);
    }
    #endregion
    /*
    public void AddNode(byte cost, int x, int y)
    {
        if (nodeData == null) { nodeData = new Dictionary<byte, List<string>>(); }
        if (!nodeData.ContainsKey(cost)) { nodeData[cost] = new List<string>(); }
        nodeData[cost].Add(x + "," + y);
    }

    public void RemoveNode(byte cost, int x, int y)
    {
        if (nodeData == null) { return; }
        if (!nodeData.ContainsKey(cost)) { return; }
        nodeData[cost].Remove(x + "," + y);
    }

    public void ClearAllNode()
    {
        nodeData.Clear();
    }
    #endregion
*/
}

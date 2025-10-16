using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Globalization;
using UnityEngine;
using System;
using Newtonsoft.Json;

#if UNITY_EDITOR
using onicore.editor;
#endif

public class MapControl : Singleton<MapControl>
{
    //地图数据
    MapJsonData levelData;
    public MapJsonData LevelData { get { return levelData; } }

    //GameObject mapNode;

    //地形组件
    GameObject groupTerrain;
    //特效组件
    GameObject groupVFX;

    public void Init()
    {
        groupTerrain = new GameObject("Terrain");
        groupTerrain.AddComponent<TerrainControl>().Init();
        groupTerrain.transform.SetParent(transform);

        /*
        mapNode = new GameObject("MapNode");
        mapNode.AddComponent<MapNodeControl>().Init();
        mapNode.transform.SetParent(transform);
        */
        // groupVFX = new GameObject("VFX");
        // groupVFX.AddComponent<EnvVfxControl>().Init();
        // groupVFX.transform.SetParent(transform);
    }

    public void Clear()
    {
        foreach (Transform child in groupTerrain.transform)
        {
            foreach (Transform item in child)
            {
                Destroy(item.gameObject);
            }
        }

        foreach (Transform child in groupVFX.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public async UniTask OpenLevel(LevelData levelData, bool isEditor = false)
    {
        string stream = await ReadWrite.ReadDataAsync(levelData.mapName);

        //处理派生类的情况，如FeaturePointArgs会被
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects, // 使用Objects来处理类型信息
            SerializationBinder = new FeaturePointBinder() // 添加自定义的Binder
        };
        //反序列化
        this.levelData = JsonConvert.DeserializeObject<MapJsonData>(stream, settings);

        Debug.Log($"=== MapControl: open map: {levelData.levelName} ===");

        //MapNodeControl.Instance.OnEnableMapNode(levelData.nodeData);

        await GenerateTerrain(levelData);

        // 如果是在编辑器中，则生成编辑器Gizmos
        if (isEditor)
        {
#if UNITY_EDITOR
            await GenerateEditorGizmos();
#endif
        }
    }

    //创建地形
    public async UniTask GenerateTerrain(LevelData args)
    {
        Dictionary<string, Dictionary<string, List<string>>> terrainData = levelData.terrainData;

        /*
        float tileSize = 2;
        float offsetX = -16;
        float offsetY = -26;
        string tileName = $"ter_{args.themeName}_tile_01";
        //创建背景
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 26; j++)
            {
                await TerrainControl.Instance.CreateViewObject(new Vector2(i * tileSize + offsetX, j * tileSize + offsetY), tileName, TerrainLayer.Tile);
            }
        }*/

        //float offset = 0.05f;//消除组装图的缝隙
        //await TerrainControl.Instance.CreateBG(args.themeName + "_tr", -offset, -offset);
        //await TerrainControl.Instance.CreateBG(args.themeName + "_tl", offset, -offset);
        //await TerrainControl.Instance.CreateBG(args.themeName + "_br", -offset, 0);
        //await TerrainControl.Instance.CreateBG(args.themeName + "_bl", offset, 0);

        int batchCount = 0;
        int batchIndex = 0;
        int maxBatch = 30;
        List<List<UniTask>> taskGroup = new List<List<UniTask>>();
        List<UniTask> tasks = new List<UniTask>();
        taskGroup.Add(tasks);

        foreach (string group in terrainData.Keys)
        {
            foreach (string itemName in terrainData[group].Keys)
            {
                foreach (string xy in terrainData[group][itemName])
                {
                    batchCount++;
                    if (batchCount >= maxBatch)
                    {
                        tasks = new List<UniTask>();
                        taskGroup.Add(tasks);
                        batchIndex++;
                        batchCount = 0;
                    }

                    string[] array = xy.Split(",");
                    float posX = float.Parse(array[0], new CultureInfo("en-US").NumberFormat);
                    float posY = float.Parse(array[1], new CultureInfo("en-US").NumberFormat);
                    //int rotateZ = int.Parse(array[2], new CultureInfo("en-US").NumberFormat);
                    Vector2 worldPosition = new Vector2(posX, posY);

                    TerrainLayer terrainLayer;
                    Utility.TryParseEnum<TerrainLayer>(group, out terrainLayer);
                    taskGroup[batchIndex].Add(TerrainControl.Instance.CreateViewObject(worldPosition, itemName, terrainLayer));
                }
            }
        }

        foreach (List<UniTask> group in taskGroup)
        {
            await UniTask.WhenAll(group);
        }
    }

    //创建特效
    public async UniTask GenerateVFX()
    {
        Dictionary<string, List<string>> vfxData = levelData.vfxData;

        int batchCount = 0;
        int batchIndex = 0;
        int maxBatch = 30;
        List<List<UniTask>> taskGroup = new List<List<UniTask>>();
        List<UniTask> tasks = new List<UniTask>();
        taskGroup.Add(tasks);

        foreach (string itemName in vfxData.Keys)
        {
            foreach (string xy in vfxData[itemName])
            {
                batchCount++;
                if (batchCount >= maxBatch)
                {
                    tasks = new List<UniTask>();
                    taskGroup.Add(tasks);
                    batchIndex++;
                    batchCount = 0;
                }

                string[] array = xy.Split(",");
                float posX = float.Parse(array[0], new CultureInfo("en-US").NumberFormat);
                float posY = float.Parse(array[1], new CultureInfo("en-US").NumberFormat);
                Vector2 worldPosition = new Vector2(posX, posY);

                // taskGroup[batchIndex].Add(EnvVfxControl.Instance.CreateViewObject(worldPosition, itemName));
            }
        }

        foreach (List<UniTask> group in taskGroup)
        {
            await UniTask.WhenAll(group);
        }
    }


#if UNITY_EDITOR

    //创建编辑器Gizmos（比如出生点的位置提示等，只会在编辑器中显示的指示物）
    public async UniTask GenerateEditorGizmos()
    {
        //await CreateGizmosForSpawnPoints(levelData.spawnPointData, "spawn_point");
        //await CreateGizmosForDefenseTowerPoints(levelData.defenseTowerPointArgs, "defensetower_point");
    }

    //创建出生点Gizmos
    private async UniTask CreateGizmosForSpawnPoints(List<SpawnPointArgs> listSpawnPointArgs, string objectName)
    {
        foreach (var spawnPointArgs in listSpawnPointArgs)
        {
            string editSprite = "edit_spawn_point";
            await EditorControl.Instance.CreateGizmosObject(
                new Vector2(spawnPointArgs.posX, spawnPointArgs.posY),//位置
                objectName,                     //对象名字
                editSprite,                     //编辑图片
                typeof(EditHandleSpawnPoint),   //创建handle类型
                spawnPointArgs);                //数据
        }
    }

    /// <summary>
    /// 编辑器地图确认操作，创建物件并写入数据
    /// </summary>
    public async void OnClickAction<T>(Vector2 worldPosition, T editTargetArgs) where T : EditTargetArgs
    {
        Debug.Log("=== MapGenerator: worldPos: " + worldPosition + " targetName: " + editTargetArgs.targetName);

        float posX = 0;
        float posY = 0;
        switch (editTargetArgs.mapObjectType)
        {
            case MapObjectType.VFX:
                // await EnvVfxControl.Instance.CreateViewObject(worldPosition, editTargetArgs.targetName);
                // posX = (float)Math.Round(worldPosition.x, 3);
                // posY = (float)Math.Round(worldPosition.y, 3);
                // levelData.AddVFX(posX, posY, editTargetArgs.targetName);
                break;
            case MapObjectType.Terrain:
                //地形
                var terrainArgs = editTargetArgs as EditTargetTerrainArgs;
                await TerrainControl.Instance.CreateViewObject(worldPosition, editTargetArgs.targetName, terrainArgs.terrainLayer);
                break;
            case MapObjectType.SpawnPoint:
                //出生点
                await EditorControl.Instance.CreateGizmosObject(
                    worldPosition,                  //位置
                    editTargetArgs.targetName,      //对象名字
                    editTargetArgs.editSprite,      //编辑图片
                    typeof(EditHandleSpawnPoint),   //创建handle类型
                    new SpawnPointArgs()            //预设数据
                    {

                    });
                break;
            default:
                break;
        }
    }
#endif
}

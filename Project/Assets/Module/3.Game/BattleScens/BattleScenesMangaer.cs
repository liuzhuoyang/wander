using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


//不管数据，只管显示
public class BattleScenesMangaer : Singleton<BattleScenesMangaer>
{
    [Header("人物模型")]
    public GameObject characterPrefab;

    [Header("角色管理")]
    [SerializeField] private Transform characterParent; // 角色的父对象

    [Header("map管理")]
    [SerializeField] private Transform mapParent; // 角色的父对象


    // 当前场景中的角色列表
    private List<BattleCharacter> currentCharacters = new List<BattleCharacter>();
    private GameObject currentMap;

    protected override void Awake()
    {
        base.Awake();

        // 如果没有设置父对象，创建一个
        if (characterParent == null)
        {
            GameObject parentObj = new GameObject("Characters");
            characterParent = parentObj.transform;
        }

        if (mapParent == null)
        {
            GameObject parentObj = new GameObject("Map");
            mapParent = parentObj.transform;
        }
    }


    //加载关卡数据，读取天气，水流，环境。障碍物不从这里进，可以理解为地图贴图
    public async UniTask LoadScene(LevelData levelData)
    {
        currentMap = Instantiate(await GameAsset.GetPrefabAsync(levelData.name, levelData.mapPrefab), mapParent);
        currentMap.name = "Map";
    }
    public void CleanUpScene()
    {
        Destroy(currentMap);
    }

    /// <summary>
    /// 读取玩家数据，替换场景中的人物，宠物，围栏，树等外围显示，也是不管数据，只管显示
    /// </summary>
    public void LoadUserData()
    {
        // 检查是否有法阵
        if (!BattleFormationMangaer.Instance.HasFormatian())
        {
            Debug.LogError("没有法阵，无法开始战斗");
            return;
        }
        // 创建角色并开始移动
        CreateCharacterAndStartMoving();
    }

    /// <summary>
    /// 创建角色并开始移动
    /// </summary>
    private void CreateCharacterAndStartMoving()
    {
        // 获取第一个节点
        var formatianNodes = BattleFormationMangaer.Instance.GetCurrentFormatianNodes();
        if (formatianNodes.Count == 0)
        {
            Debug.LogError("法阵中没有节点");
            return;
        }

        // 找到第一个节点（索引最小的）
        GameObject firstNode = null;
        int firstNodeIndex = int.MaxValue;

        foreach (var node in formatianNodes)
        {
            string nodeName = node.name;
            if (nodeName.Contains("_"))
            {
                string[] parts = nodeName.Split('_');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int nodeIndex))
                {
                    if (nodeIndex < firstNodeIndex)
                    {
                        firstNodeIndex = nodeIndex;
                        firstNode = node;
                    }
                }
            }
        }

        if (firstNode == null)
        {
            Debug.LogError("无法找到第一个节点");
            return;
        }

        // 创建角色
        BattleCharacter character = CreateCharacter("Player", firstNodeIndex);

        if (character != null)
        {
            // 开始移动
            character.StartMoving();
            Debug.Log($"角色创建完成，开始从节点 {firstNodeIndex} 移动");
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <param name="startNodeIndex">起始节点索引</param>
    /// <returns>创建的角色</returns>
    public BattleCharacter CreateCharacter(string characterName = "default", int startNodeIndex = 0)
    {
        if (characterPrefab == null)
        {
            Debug.LogError("角色Prefab未设置");
            return null;
        }

        // 创建角色实例
        GameObject characterObj = Instantiate(characterPrefab, characterParent);
        characterObj.name = characterName;

        // 获取BattleCharacter组件
        BattleCharacter character = characterObj.GetComponent<BattleCharacter>();
        if (character == null)
        {
            character = characterObj.AddComponent<BattleCharacter>();
        }

        // 初始化角色
        character.Initialize(startNodeIndex);

        // 添加到角色列表
        currentCharacters.Add(character);

        Debug.Log($"创建角色: {characterName}，起始节点: {startNodeIndex}");

        return character;
    }

    /// <summary>
    /// 销毁角色
    /// </summary>
    /// <param name="character">要销毁的角色</param>
    public void DestroyCharacter(BattleCharacter character)
    {
        if (character == null) return;

        // 从列表中移除
        currentCharacters.Remove(character);

        // 销毁GameObject
        Destroy(character.gameObject);

        Debug.Log($"销毁角色: {character.name}");
    }

    /// <summary>
    /// 销毁所有角色
    /// </summary>
    public void DestroyAllCharacters()
    {
        foreach (var character in currentCharacters)
        {
            if (character != null)
            {
                Destroy(character.gameObject);
            }
        }

        currentCharacters.Clear();
        Debug.Log("销毁所有角色");
    }

    /// <summary>
    /// 获取所有当前角色
    /// </summary>
    /// <returns>角色列表</returns>
    public List<BattleCharacter> GetAllCharacters()
    {
        return new List<BattleCharacter>(currentCharacters);
    }

    /// <summary>
    /// 停止所有角色的移动
    /// </summary>
    public void StopAllCharacters()
    {
        foreach (var character in currentCharacters)
        {
            if (character != null)
            {
                character.StopMoving();
            }
        }

        Debug.Log("停止所有角色移动");
    }

    /// <summary>
    /// 开始所有角色的移动
    /// </summary>
    public void StartAllCharacters()
    {
        foreach (var character in currentCharacters)
        {
            if (character != null)
            {
                character.StartMoving();
            }
        }

        Debug.Log("开始所有角色移动");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // 清理所有角色
        DestroyAllCharacters();
    }
}

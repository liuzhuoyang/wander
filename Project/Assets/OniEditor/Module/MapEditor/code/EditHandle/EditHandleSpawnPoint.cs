#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using onicore.editor;

public class EditHandleSpawnPoint : EditHandle
{
    /*
    public SpawnPointRouteType spawnPointType;

    [ValueDropdown("GetWaveList")]
    public int unlockWaveID;

    [ShowIf("IsFixedOrBoss")]
    public int delay = 0;

    [ShowIf("spawnPointType", SpawnPointRouteType.Fixed)]
    [ValueDropdown("GetEnemyNameList")]
    public string unitName;

    [ShowIf("spawnPointType", SpawnPointRouteType.Fixed)]
    public int unitNum;

    [LabelText("出怪频率"), ShowIf("IsFixedOrRandom")]
    public SpawnFrequencyType spawnFrequency;

    [LabelText("可用作召唤物召唤点")]
    public bool isSpawnBossSummonners;

    void OnEnable()
    {
        transform.Find("hud").gameObject.SetActive(true);
    }

    public void Init(SpawnPointArgs spawnPointArgs)
    {
        this.spawnPointType = spawnPointArgs.spawnPointType;
        this.unlockWaveID = spawnPointArgs.unlockWaveID;
        this.delay = spawnPointArgs.delay;
        this.spawnFrequency = spawnPointArgs.spawnFrequency;
        this.unitName = spawnPointArgs.unitName;
        this.unitNum = spawnPointArgs.unitNum;
        this.isSpawnBossSummonners = spawnPointArgs.canSpawnSummonners;
        RefreshView();
    }

    public override void OnErase()
    {
        base.OnErase();
        MapControl.Instance.LevelData.RemoveSpawnPointHandle(this);
    }

    public override void OnEdit()
    {
        isValidating = true;
        base.OnEdit();
    }

    // 在编辑器中，当属性值发生变化时，会触发OnValidate方法
    // 只有在第一次点开编辑才会开启这个标签，避免在初始化时候，触发OnValidate
    // OnSaveAndClose里会关闭，如果用红色按钮关闭就不会重置
    bool isValidating = false;
    void OnValidate()
    {
        if (isValidating)
            OnValueChanged();
    }

    void OnValueChanged()
    {
        RefreshView();
    }

    async void RefreshView()
    {
        // 获取当前游戏对象的SpriteRenderer组件
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Sprite newSprite = await GameAssets.GetSpriteAsync("edit_spawn_point_" + spawnPointType.ToString().ToLower());
        // 替换图片
        renderer.sprite = newSprite;


        TextMeshProUGUI text = transform.Find("hud/text").GetComponent<TextMeshProUGUI>();
        text.text = unlockWaveID.ToString();

        if (spawnPointType == SpawnPointRouteType.Boss)
            text.gameObject.SetActive(false);
    }

    public override void OnSaveAndClose()
    {
        base.OnSaveAndClose();
        isValidating = false;
    }

    List<string> GetEnemyNameList()
    {
        List<string> enemyNameList = new List<string>();
        enemyNameList.Add("");
        string unitRace = EditorData.currentLevelAsset.unitRace;
        UnitRaceAsset unitRaceAsset = AssetsFinder.FindAssetByName<UnitRaceAsset>(EditorPathUtility.unitRaceAssetPath, "race_" + unitRace);
        foreach (UnitAsset unitAsset in unitRaceAsset.listUnit)
        {
            enemyNameList.Add(unitAsset.unitName);
        }
        return enemyNameList;
    }

    List<int> GetWaveList()
    {
        int totalWave = EditorData.currentLevelAsset.totalWave;
        List<int> waveList = new List<int>();
        for (int i = 0; i <= totalWave; i++)
        {
            waveList.Add(i);
        }
        return waveList;
    }

    private bool IsFixedOrBoss()
    {
        return spawnPointType == SpawnPointRouteType.Fixed || spawnPointType == SpawnPointRouteType.Boss;
    }

    private bool IsFixedOrRandom()
    {
        return spawnPointType == SpawnPointRouteType.Fixed || spawnPointType == SpawnPointRouteType.Random;
    }*/
}
#endif
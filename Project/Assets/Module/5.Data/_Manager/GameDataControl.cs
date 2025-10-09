using UnityEngine;

public class GameDataControl : MonoBehaviour
{
    public static GameDataControl Instance;

    //自动初始化数据，为了避免添加数据的维护，所有模块数据都是通过自动初始化的方式来加载，这里是数据集合
    //各自的模块会Get GameDataBase来获取数据，所以这里在做的是把序列化数据先初始化为字典，方便各模块通过统一方法用名字获取
    //其他模块的Init会通过RuntimeInitializeOnLoadMethod来调用，这里Awake必须先执行gameDataCollection.Init();否则其他模块无法访问到数据
    public void Awake()
    {
        Instance = this;
        gameDataCollection.Init();
    }

    public GameDataCollection gameDataCollection;

    public GameDataCollectionBase Get(string dataName)
    {
        return gameDataCollection.dictGameData[dataName];
    }

#if UNITY_EDITOR
    const string ASSETS_PATH = "Assets/Module/5.Data/_Manager/asset/";

    // 只会在编辑器里使用,因为不想在所有数据文件都写死Path，而且静态参数在编辑器里无法存储
    // 通过这个方法，通过找到这个管理器来找到对象的GameDataCollectionBase里定义的路径

    // 编辑器中获取GameDataCollection
    public static GameDataCollection GetGameDataCollectionEDITOR()
    {
        GameDataCollection gameDataControl = AssetsFinder.FindAssetByName<GameDataCollection>(ASSETS_PATH, "all_game_data");
        return gameDataControl;
    }

    // 传入数据名字，获取数据的路径
    public static string GetAssetPath(string dataName)
    {
        GameDataCollection gameDataControl = AssetsFinder.FindAssetByName<GameDataCollection>(ASSETS_PATH, "all_game_data");
        gameDataControl.Init();
        return gameDataControl.dictGameData[dataName].path;
    }

    // 获取本地化数据的路径
    public static string GetLocPath(string dataName)
    {
        GameDataCollection gameDataControl = AssetsFinder.FindAssetByName<GameDataCollection>(ASSETS_PATH, "all_game_data");
        gameDataControl.Init();
        return gameDataControl.dictGameData[dataName].GetLocPath();
    }
#endif
}

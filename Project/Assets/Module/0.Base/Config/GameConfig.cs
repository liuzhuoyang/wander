using UnityEngine;
public enum PlatformType
{
        Global,
        Wechat,
        Douyin,
};

[Tooltip("OfflineDev，离线开发模式，不连不服务。Dev常规开发模式，连服务器； Sandbox带Debugger测试用； Release正式出包")]
public enum ProductMode
{
        DevOffline, //离线开发模式，不连不服务，一般功能开发在这里
        Dev,        //一般开发模式，连服务器，游戏服务器功能完善后切换到这个模式
        Sandbox,    //带Analytics的Dev版本，用于打包测试Analytics SDK
        Release     //正式出包
    }

[Tooltip("是否带Debug工具")]
public enum DebugTool
{
    On,
    Off,
}

public enum AnalyticsTool
{
    Off,
    On
}

[Tooltip("如何显示伤害跳字")]
 public enum HudDamageShowType
{
    Normal, //正常显示
    Off,    //全部关闭
    Debug,  //debug模式
}

public static class GameConfig
{   
    //运行时DebugMode,用于Debug的控制器，不要去改变main里的config参数
    public static DebugTool debugToolRunTime;
    public static GameConfigAsset main;

    public static void Init()
    {
        main = GameConfigHandler.Instance.gameConfig;
        debugToolRunTime = main.debugTool;
    }
}



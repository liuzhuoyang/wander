using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public enum UIPageType
{
    Mode,
    Normal,     //常规页面
    Overlay,    //覆盖
}

public class UIMain : Singleton<UIMain>
{
    [SerializeField]
    int cacheSize = 10;

    [HideInInspector]
    public Transform groupActive;
    [HideInInspector]
    public Transform groupInactive;
    [HideInInspector]
    public Transform groupOverlay;
    [HideInInspector]
    public Transform groupMode;
    [HideInInspector]
    public Transform groupModeInactive;
    [HideInInspector]
    public Transform groupPopup;

    public Canvas canvas;

    UICache uiCache;

    Dictionary<string, Transform> modePageDict;

    public async UniTask Init()
    {
        modePageDict = new Dictionary<string, Transform>();

        groupActive = transform.Find("active");
        groupInactive = transform.Find("inactive");
        groupOverlay = transform.Find("overlay");
        groupMode = transform.Find("mode");
        groupModeInactive = transform.Find("mode_inactive");
        groupPopup = transform.Find("popup");

        //初始化ui页面缓存管理组件
        uiCache = gameObject.AddComponent<UICache>();
        uiCache.Init(cacheSize);

        //创建模式页面
        await CreateModeUI("home");
        await CreateModeUI("battle");

        InitDebugger();
    }

    #region 模式页面
    //模式页面，比如战斗模式的主页，大厅模式的主页，副玩法主页
    async UniTask CreateModeUI(string name)
    {
        string pageName = "ui_mode_" + name;
        GameObject go = Instantiate(await GameAsset.GetPrefabAsync(pageName), GetGroup(UIPageType.Mode), false);
        go.name = pageName;
        go.transform.SetParent(groupModeInactive);

        modePageDict.Add(name, go.transform);
    }

    //创建模式页面下的子页面
    public async UniTask CreateModeSubPage(string name, string modeName)
    {
        string pageName = "ui_mode_" + modeName + "_" + name;
        GameObject go = Instantiate(await GameAsset.GetPrefabAsync(pageName), transform, false);
        go.name = pageName;

        //TODO 需要优化
        //等待给予注册事件的时间
        await UniTask.WaitForSeconds(0.05f);

        Transform parent = modePageDict[modeName].Find("inactive");
        go.transform.SetParent(parent);
    }
    #endregion

    #region 常规页面
    public async UniTask OpenUI(string name, UIPageType pageType)
    {
        string pageName = "ui_" + name;
        GameObject page = uiCache.Get(pageName);
        Transform parentGroup = GetGroup(pageType);

        if (page != null)
        {
            page.transform.SetParent(parentGroup);
        }
        else
        {
            try
            {
                GameObject prefab = await GameAsset.GetPrefabAsync(pageName);
                if (prefab == null)
                {
                    Debug.LogError($"=== UIMain: OpenUI Prefab not found: {pageName} ===");
                    return;
                }

                page = Instantiate(prefab, parentGroup, false);
                page.name = pageName;
                uiCache.Add(pageName, page);
            }
            catch (Exception ex)
            {
                Debug.LogError($"=== UIMain: OpenUI Failed to load page: {pageName}. Exception: {ex.Message} ===");
                // 这里可以添加更多的错误处理逻辑
            }
        }

        /*
        // 确保 active 的层级正确
        Canvas canvas = groupActive.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = CalculateSortingOrder();
        }*/
        //动态设置一下active的层级，避免unity的渲染层级bug
        //groupActive.GetComponent<Canvas>().sortingOrder = groupActive.GetComponent<Canvas>().sortingOrder;
    }

    public async UniTask OpenUIWithParent(string name, Transform parent)
    {
        string pageName = "ui_" + name;
        GameObject page = uiCache.Get(pageName);

        if (page != null)
        {
            page.transform.SetParent(parent);
        }
        else
        {
            try
            {
                GameObject prefab = await GameAsset.GetPrefabAsync(pageName);
                if (prefab == null)
                {
                    Debug.LogError($"=== UIMain: OpenUI Prefab not found: {pageName} ===");
                    return;
                }

                page = Instantiate(prefab, parent, false);
                page.name = pageName;
                uiCache.Add(pageName, page);
            }
            catch (Exception ex)
            {
                Debug.LogError($"=== UIMain: OpenUI Failed to load page: {pageName}. Exception: {ex.Message} ===");
                // 这里可以添加更多的错误处理逻辑
            }
        }
    }

    Transform GetGroup(UIPageType pageType)
    {
        switch (pageType)
        {
            case UIPageType.Mode:
                return groupMode;
            case UIPageType.Normal:
                return groupActive;
            case UIPageType.Overlay:
                return groupOverlay;
            default:
                return groupActive;
        }
    }

    //切换模式
    public void OnModeUI(string targetMode)
    {
        foreach (Transform child in groupMode)
        {
            child.GetComponent<UIMode>().Deactivate();
            child.transform.SetParent(groupModeInactive);
        }

        GameObject go = groupModeInactive.Find("ui_mode_" + targetMode).gameObject;
        go.GetComponent<UIMode>().Activate();
        go.transform.SetParent(groupMode);
    }

    public void CloseUI(Transform targetPage)
    {
        targetPage.SetParent(groupInactive);
        //activePage.transform.SetParent(groupInactive);
        //activePage = null;
        //homeUI.OnShow();

        //发送全局UI关闭事件
        EventManager.TriggerEvent<GPTriggerArgs>(GPTriggerEventName.EVENT_UI_CLOSED, null);
    }
    #endregion

    #region 二级页面
    public async UniTask<GameObject> CreateSecPageUI(string name, Transform parent)
    {
        string pageName = "ui_" + name;
        GameObject go = Instantiate(await GameAsset.GetPrefabAsync(pageName), parent);
        return go;
    }

    #endregion

    #region 常驻页面
    //这个页面是动态创建出来的，创建之后就不会删除，存放在ui上，如ui_acting这种常驻ui
    public async UniTask<GameObject> CreateStaticPage(string name)
    {
        string pageName = "ui_" + name;
        GameObject go = Instantiate(await GameAsset.GetPrefabAsync(pageName), this.transform);
        return go;
    }
    #endregion

    #region Debug
    DebuggerControl debuggerControl;
    async void InitDebugger()
    {
        // 如果配置文件没有开启Debug，则不初始化Debugger
        if (GameConfig.debugToolRunTime == DebugTool.Off) return;

        // 初始化Debugger
        GameObject go = Instantiate(await GameAsset.GetPrefabAsync("debugger"), transform);
        go.GetComponent<Debugger>().Init();

        // 初始化Debugger控制器，全局控制开启关闭调试器，在右上角透明区域点击三下
        GameObject goGlobalControl = Instantiate(await GameAsset.GetPrefabAsync("debugger_control"), transform);
        debuggerControl = goGlobalControl.GetComponent<DebuggerControl>();
        debuggerControl.Init();
    }

    public void OnOpenDebugger()
    {
        GameConfig.debugToolRunTime = DebugTool.On;
        if (debuggerControl == null)
        {
            InitDebugger();
        }
        else
        {
            debuggerControl.Init();
        }
    }

    #endregion

    #region Hepler
    public bool CheckIsNoActiveUI()
    {
        bool result = true;
        if (groupActive.childCount > 0)
        {
            result &= false;
        }
        if (groupOverlay.childCount > 0)
        {
            result &= false;
        }

        //如果有Popup是激活的，则返回false
        //目前popup关闭时不会立马删除，只会设置active为false,所有用以下方法检查
        foreach (Transform child in groupPopup)
        {
            if (child.gameObject.activeSelf)
            {
                result &= false;
            }
        }
        return result;
    }
    #endregion
}

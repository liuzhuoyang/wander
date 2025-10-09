using System.Collections.Generic;
public static class HeaderControl
{
    private static List<string> listShowHub = new List<string>();
    private static Dictionary<string, List<string>> dictShowHub = new Dictionary<string, List<string>>();
    public static void Init()
    {
        //初始化货币
        OnUpdateHeaderItemNumDisplay(ConstantItem.COIN, 0, ItemSystem.Instance.GetItemNum(ConstantItem.COIN), false);
        OnUpdateHeaderItemNumDisplay(ConstantItem.GEM, 0, ItemSystem.Instance.GetItemNum(ConstantItem.GEM), false);
        OnUpdateHeaderItemNumDisplay(ConstantItem.ENERGY, 0, ItemSystem.Instance.GetItemNum(ConstantItem.ENERGY), false);
        OnUpdateHeaderItemNumDisplay(ConstantItem.TOKEN_TAVERN, 0, ItemSystem.Instance.GetItemNum(ConstantItem.TOKEN_TAVERN), false);

        OnRefreshProfile();
    }

    //显示顶部栏,统一管理事件，避免带参数事件散落在各处导致管理困难
    public static void OnShow()
    {
        EventManager.TriggerEvent<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_UI, new UIHeaderArgs
        {

        });
    }

    //隐藏顶部栏
    public static void OnHide()
    {
        EventManager.TriggerEvent<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_HIDE_UI, new UIHeaderArgs
        {

        });
    }

    //显示隐藏顶部栏的部件, 比如传入“coin”, 则只显示“coin”, 其他所有顶部部件会被隐藏
    public static void OnShowMainHideHub(List<string> listShowHub = null)
    {
        EventManager.TriggerEvent<UIHeaderShowHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_HIDE_HUB_UI, new UIHeaderShowHeaderArgs
        {
            listShowHub = listShowHub
        });
        HeaderControl.listShowHub = listShowHub;
    }
    public static void OnShowUIHideHub(string pageName, List<string> listShowHub = null)
    {
        EventManager.TriggerEvent<UIHeaderShowHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_HIDE_HUB_UI, new UIHeaderShowHeaderArgs
        {
            listShowHub = listShowHub
        });
        dictShowHub[pageName] = HeaderControl.listShowHub;
        HeaderControl.listShowHub = listShowHub;
    }

    public static void OnCloseUIHideHub(string pageName)
    {
        if (!dictShowHub.ContainsKey(pageName))
        {
            return;
        }
        listShowHub = dictShowHub[pageName];
        dictShowHub.Remove(pageName);
        EventManager.TriggerEvent<UIHeaderShowHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_HIDE_HUB_UI, new UIHeaderShowHeaderArgs
        {
            listShowHub = listShowHub
        });
    }

    //处理单个货币刷新
    //isPlayAnimation 是否播放动画, 货币从当前值变到目标值的动画
    public static void OnUpdateHeaderItemNumDisplay(string itemName, int startValue, int targetValue, bool isPlayAnimation = true)
    {
        EventManager.TriggerEvent<UIHeaderItemNumArgs>(EventNameHeader.EVENT_HEADER_UPDATE_ITEM_NUM_UI, new UIHeaderItemNumArgs()
        {
            isPlayAnimation = isPlayAnimation,
            itemName = itemName,
            startValue = startValue,
            endValue = targetValue,
        });
    }

    //检查是否需要刷新顶部栏货币
    public static void OnCheckHeaderItemRefresh(string itemName, int addValue)
    {
        if (isHeaderItem(itemName))
        {
            int currentValue = ItemSystem.Instance.GetItemNum(itemName);
            OnUpdateHeaderItemNumDisplay(itemName, currentValue - addValue, currentValue);
        }
    }

    //检查是否是顶部栏货币
    static bool isHeaderItem(string itemName)
    {
        return itemName == ConstantItem.COIN || itemName == ConstantItem.GEM || itemName == ConstantItem.ENERGY;
    }

    public static void OnRefreshProfile()
    {
        EventManager.TriggerEvent<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_REFRESH_PROFILE, new UIHeaderArgs
        {

        });
    }
}

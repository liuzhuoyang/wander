
using System.Collections.Generic;

public static class ModeHomeControl
{
    private static int lastIndex = -1;  // 添加一个变量来保存上一次的索引
    public static List<string> listModeHome;

    public static void Init(List<string> listModeHome)
    {
        // 初始化页面列表，根据UI里创建的顺序，传入页面名称排序，用于后面检索页面，影响播放动画的左右顺序
        ModeHomeControl.listModeHome = listModeHome;
    }

    public static void OnOpen(string targetPage)
    {
        // 获取当前页面的索引
        int currentIndex = listModeHome.IndexOf(targetPage);
        // 获取左侧页面，如果当前是第一个页面则返回空字符串
        string leftPage = currentIndex > 0 ? listModeHome[currentIndex - 1] : "";
        // 获取右侧页面，如果当前是最后一个页面则返回空字符串
        string rightPage = currentIndex < listModeHome.Count - 1 ? listModeHome[currentIndex + 1] : "";

        // 判断移动方向：如果上一次的索引小于当前索引，则说明是向左移动
        bool isMovingLeft = lastIndex < currentIndex;
        lastIndex = currentIndex;  // 更新上一次的索引

        // 触发事件，传递页面切换的相关参数
        EventManager.TriggerEvent<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_ONSELECT_UI, new UIModeHomeArgs { 
            targetPage = targetPage,
            leftPage = leftPage,
            rightPage = rightPage,
            isMovingLeft = isMovingLeft
        });
    }

    public static void OnHideCurrentPage()
    {
        EventManager.TriggerEvent<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_HIDE_CURRENT_UI, new UIModeHomeArgs { });
    }
}

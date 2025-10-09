using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;

public class UIModeHome : UIMode
{
    #region 变量
    float fadeInDuration = 0.2f;
    public Transform groupInactive;
    public Transform groupActive;
    public Transform groupCache;

    Transform selectedPage; //当前选中的页面
    Transform cachePage;    //缓存页面，动画切换用

    Vector2 leftPagePos;
    Vector2 rightPagePos;
    Dictionary<string, Transform> dictPage;
    #endregion

    private void OnDestroy()
    {
        Deactivate();
    }

    public override void Deactivate()
    {
        EventManager.StopListening<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_ONSELECT_UI, OnSelect);
        EventManager.StopListening<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_HIDE_CURRENT_UI, OnHideCurrentPage);
        //EventManager.StopListening<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_SHOW_CURRENT_UI, OnShowCurrentPage);
    }

    public override void Activate()
    {
        // 获取主UI的RectTransform组件
        RectTransform canvasRect = UIMain.Instance.GetComponent<RectTransform>();
        
        // 计算精确的偏移量
        leftPagePos = new Vector2(-canvasRect.rect.width, 0);  // 向左移动一个完整屏幕宽度
        rightPagePos = new Vector2(canvasRect.rect.width, 0);  // 向右移动一个完整屏幕宽度
        
        // 激活当前UI对象
        gameObject.SetActive(true);
        
        if(dictPage == null)
        {
             // 初始化页面字典
            dictPage = new Dictionary<string, Transform>();
            
            // 遍历组中的所有页面，添加到字典中
            foreach (Transform page in groupInactive.Cast<Transform>()
                .Concat(groupActive.Cast<Transform>())
                .Concat(groupCache.Cast<Transform>()))
            {
                dictPage.Add(page.name, page);
            }
              
            // 初始化页面列表，根据UI里创建的顺序，传入页面名称排序，用于后面检索页面，影响播放动画的左右顺序
            ModeHomeControl.Init(dictPage.Keys.Select(key => key.Replace("ui_mode_home_", "")).ToList());
        }
     
        // 注册页面选择事件监听
        EventManager.StartListening<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_ONSELECT_UI, OnSelect);
        EventManager.StartListening<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_HIDE_CURRENT_UI, OnHideCurrentPage);
        //EventManager.StartListening<UIModeHomeArgs>(EventNameModeHome.EVENT_HOME_SHOW_CURRENT_UI, OnShowCurrentPage);
        // 隐藏非活动组
        groupInactive.gameObject.SetActive(false);
    }

    //页面选择事件
    public void OnSelect(UIModeHomeArgs args)
    {
        string pageName = "ui_mode_home_" + args.targetPage;

        gameObject.SetActive(true);

        //如果当前没有选中页面，则直接选中
        if(selectedPage == null)
        {
            selectedPage = dictPage[pageName];
            selectedPage.SetParent(groupActive);
            return;
        }

        // 如果当前页面就是目标页面，直接返回
        if (selectedPage != null && selectedPage == dictPage[pageName]) 
            return;

        // 处理旧页面的退出动画
        if (selectedPage != null)
        {
            cachePage = selectedPage;
            cachePage.SetParent(groupCache);
            
            AnimatePageExit(cachePage, args.isMovingLeft);
        }
        
        // 处理新页面的进入动画
        selectedPage = dictPage[pageName];
        AnimatePageEnter(selectedPage, args.isMovingLeft);
    }

    public void OnHideCurrentPage(UIModeHomeArgs args)
    {
        selectedPage.SetParent(groupInactive);
        selectedPage = null;
        //ResetPagePosition(selectedPage);
    }

    public void OnShowCurrentPage(UIModeHomeArgs args)
    {
        string pageName = "ui_mode_home_" + args.targetPage;
        groupInactive.transform.Find(pageName).SetParent(groupActive);
    }

    #region 动画
    //退出动画
     private void AnimatePageExit(Transform page, bool isMovingLeft)
    {
        // 停止当前页面的所有动画
        page.DOKill();
        // 修正移动方向：如果isMovingLeft为true，页面应该向右移出
        Vector2 targetPos = isMovingLeft ? leftPagePos : rightPagePos;
        // 执行页面退出动画，完成后重置页面位置
        page.DOLocalMoveX(targetPos.x, fadeInDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => ResetPagePosition(page));
    }

    //进入动画      
    private void AnimatePageEnter(Transform page, bool isMovingLeft)
    {
        // 如果页面为空则直接返回
        if (page == null) return;

        // 修正移动方向：如果isMovingLeft为true，页面应该从左边进入
        Vector2 startPos = isMovingLeft ? rightPagePos : leftPagePos;
        // 设置页面初始位置并移动到活动组
        page.localPosition = startPos;
        page.SetParent(groupActive);
        // 停止当前页面的所有动画
        page.DOKill();
        // 执行页面进入动画
        page.DOLocalMoveX(0, fadeInDuration).SetEase(Ease.Linear);
    }

    //重置页面位置
    private void ResetPagePosition(Transform page)
    {
        // 如果页面为空则直接返回
        if (page == null) return;
        // 将页面移回非活动组并重置位置
        page.SetParent(groupInactive);
        page.localPosition = Vector2.zero;
    }
    #endregion
    
    #region 调试
    public void OnDebugChangeMode()
    {
        Game.Instance.OnChangeState(GameStates.Battle);
    }
    #endregion
}

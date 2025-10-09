using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIFooter : MonoBehaviour
{
    public Transform groupTab;
    public Transform groupSub;
    float originalTabWidth;
    float selectedTabWidth;

    float footerHeight = 220;
    //int selectedTabHeight = 240;
    //int originalTabHeight = 212;

    private void Start()
    {
        Init();
        EventManager.StartListening<UIFooterArgs>(EventNameFooter.EVENT_SHOW_FOOTER_UI, OnShow);
        EventManager.StartListening<UIFooterArgs>(EventNameFooter.EVENT_HIDE_FOOTER_UI, OnHide);
        EventManager.StartListening<UIFooterArgs>(EventNameFooter.EVENT_ON_SELECT_FOOTER_UI, OnSelect);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIFooterArgs>(EventNameFooter.EVENT_SHOW_FOOTER_UI, OnShow);
        EventManager.StopListening<UIFooterArgs>(EventNameFooter.EVENT_HIDE_FOOTER_UI, OnHide);
        EventManager.StopListening<UIFooterArgs>(EventNameFooter.EVENT_ON_SELECT_FOOTER_UI, OnSelect);
    }

    void OnShow(UIFooterArgs args)  
    {
        gameObject.SetActive(true);
    }

    void OnHide(UIFooterArgs args)
    {
        gameObject.SetActive(false);
    }

    void Init()
    {
        //LayoutRebuilder.ForceRebuildLayoutImmediate(rectTabLobby);

        foreach(Transform tab in groupTab)
        {
            if(tab.name == "selected")
            {
                continue;
            }

            /*
            tab.GetComponent<Button>().onClick.AddListener(()=>
            {
                OnSelect(tab.name.Replace("tab_", ""));
            });*/
        }
    }

    // 事件触发
    public void OnSelect(UIFooterArgs args)
    {
        OnSelect(args.tabName);
    }

    //按钮触发
    public void OnSelect(string targetPage)
    {
        Transform selectedTab = groupTab.Find("tab_" + targetPage);
        
        selectedTab.GetComponent<LayoutElement>().DOPreferredSize(new Vector2(selectedTabWidth, selectedTab.GetComponent<LayoutElement>().preferredHeight), 0.1f).SetEase(Ease.OutSine);

        //selected跳出
        foreach (Transform tab in groupTab)
        {
            if (tab.name == selectedTab.name || tab.name == "selected")
            {
                continue;
            }

            //tab.Find("cover").gameObject.SetActive(true);
            tab.Find("text").gameObject.SetActive(false);
            tab.Find("selected").gameObject.SetActive(false);
            tab.Find("icon").DOKill();
            tab.Find("icon").localScale = Vector2.one;
            tab.Find("icon").localPosition = Vector2.zero;
            tab.GetComponent<LayoutElement>().DOPreferredSize(new Vector2(originalTabWidth, tab.GetComponent<LayoutElement>().preferredHeight), 0.1f).SetEase(Ease.OutSine);
        }

        selectedTab.Find("text").gameObject.SetActive(true);
        //selectedTab.Find("cover").gameObject.SetActive(false);
        selectedTab.Find("selected").gameObject.SetActive(true);
        //selectedTab.Find("icon").DOScale(Vector2.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        selectedTab.Find("icon").DOLocalMoveY(36, 0.2f).SetEase(Ease.OutBack);

        Transform selectedSub = groupSub.Find("footer_sub_" + targetPage);
        //隐藏所有二级组件
        foreach(Transform sub in groupSub)
        {
            sub.gameObject.SetActive(false);
        }

        //显示选中二级组件
        if(selectedSub != null)
        {
            selectedSub.gameObject.SetActive(true);
            selectedSub.localPosition = Vector3.zero;
            selectedSub.DOMoveY(footerHeight, 0.3f).SetEase(Ease.OutSine);
        }
    }
}

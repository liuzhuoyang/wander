using UnityEngine;
using System.Collections.Generic;
using System;

public class FooterSub : MonoBehaviour
{
    // 可用标签数量
    public int availableTabNum;
    List<GameObject> listTab;
    List<Action> listActionTab;

    void Awake()
    {
        listTab = new List<GameObject>();
        for(int i = 0; i < availableTabNum; i++)
        {
            listTab.Add(transform.Find("hub_tab").GetChild(i).gameObject);
        }
    }

    // 初始化底部栏，添加底部标签按钮回调
    public void Init(Action onTab1 = null, Action onTab2 = null, Action onTab3 = null, Action onTab4 = null)
    {
        listActionTab = new List<Action>();
        listActionTab.Add(onTab1);
        listActionTab.Add(onTab2);
        listActionTab.Add(onTab3);
        listActionTab.Add(onTab4);
    }

    void OnEnable()
    {
        
        Refresh();
    }

    public void Refresh()
    {
        // 隐藏所有标签
        foreach(GameObject tab in listTab)
        {
                tab.SetActive(false);
        }

        // 显示可用标签
        if(availableTabNum != 0)
        {
            for(int i = 0; i < availableTabNum; i++)
            {
                listTab[i].SetActive(true);
            }
        }
    }

    public void OnTab(int index)
    {
        // 执行回调
        listActionTab[index]?.Invoke();

        // 隐藏所有选中状态
        foreach(GameObject tab in listTab)
        {
            tab.transform.Find("selected").gameObject.SetActive(false);
        }

        // 显示选中状态
        listTab[index].transform.Find("selected").gameObject.SetActive(true);
    }
}

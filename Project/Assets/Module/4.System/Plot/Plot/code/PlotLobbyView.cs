using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class PlotLobbyView : MonoBehaviour
{
    //Lobby对话类型里需要被动态创建的prefab，分为普通对话，玩家选择和图片
    [SerializeField] GameObject prefabLobbySlot;
    [SerializeField] GameObject prefabLobbyPic;
    [SerializeField] GameObject prefabLobbyReward;
    [SerializeField] CanvasGroup maskContinueCanvasGroup;
    public GameObject objOption; //玩家选择的容器，用于显示隐藏
    public List<PlotLobbyOptionView> listOptionView; //玩家选择的选项
    public Transform container;             //对话条目容器
    [SerializeField] ScrollRect scrollRect; //滚动容器
    [SerializeField] GameObject objSkip, objTextContinue, objTextClose;

    PlotItem currentPlotItem;
    bool isSkip;
    bool isEnd;

    public void Init()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        isSkip = false;
        isEnd = false;
        objOption.SetActive(false);
        objSkip.SetActive(true);
        objTextContinue.SetActive(true);
        objTextClose.SetActive(false);
        //开启全屏继续按钮
        OnFullScreenClick(true);
    }

    //玩家点击屏幕
    public void OnButtonContinue()
    {
        PlotSystem.Instance.OnNextStep();
    }

    public void OnNext(PlotItem currentPlotItem)
    {
        //maskContinueCanvasGroup.gameObject.SetActive(false);

        this.currentPlotItem = currentPlotItem;
        switch (currentPlotItem.dialogType)
        {
            case PlotLobbyDialogType.Dialog:
                GameObject obj = Instantiate(prefabLobbySlot, container);
                obj.GetComponent<PlotLobbySlotView>().Init(currentPlotItem);
                break;
            case PlotLobbyDialogType.Option:
                if (isSkip)
                {
                    GameObject objOption = Instantiate(prefabLobbySlot, container);
                    objOption.GetComponent<PlotLobbySlotView>().Init(currentPlotItem, 0);
                }
                else
                {
                    objSkip.SetActive(false);
                    OnDialogOption();
                }
                break;
        }

        //滚动到最底部
        DOTween.To(() => scrollRect.verticalNormalizedPosition,
                  x => scrollRect.verticalNormalizedPosition = x,
                  0f, 0.3f)
                  .SetEase(Ease.OutCubic)
                  .SetUpdate(true)
                  .OnComplete(() =>
                  {
                      if (isSkip && !isEnd)
                      {
                          PlotSystem.Instance.OnNextStep();
                      }
                  });

        //等待动画播放完毕
        //await UniTask.Delay(500);
        //maskContinueCanvasGroup.gameObject.SetActive(true);
        //判断是否结束
        if (PlotSystem.Instance.CheckIsPlotEnd())
        {
            OnPlotLobbyEnd();
        }
    }

    void OnPlotLobbyEnd()
    {
        isEnd = true;
        objTextContinue.SetActive(false);
        objTextClose.SetActive(true);
        ActingSystem.Instance.StopActing(this.name);
        objSkip.SetActive(false);
        //关闭全屏继续按钮
        OnFullScreenClick(false);
    }

    public void OnRewardStep(UIPlotLobbyRewardArgs args)
    {
        GameObject obj = Instantiate(prefabLobbyReward, container);
        obj.GetComponent<PlotLobbyRewardView>().Init(args.rewardName, args.rewardNum);
        OnPlotLobbyEnd();
    }

    //显示选项
    void OnDialogOption()
    {
        ActingSystem.Instance.OnActing(this.name);
        objOption.SetActive(true);

        RectTransform objChoiseRect = objOption.GetComponent<RectTransform>();
        objChoiseRect.sizeDelta = new Vector2(objChoiseRect.sizeDelta.x, 0f);
        Vector2 targetSize2 = new Vector2(objChoiseRect.sizeDelta.x, 600f);

        //关闭全屏继续按钮，避免阻挡玩家点击
        OnFullScreenClick(false);

        string optionKey1 = currentPlotItem.optionKey1;
        string optionKey2 = currentPlotItem.optionKey2;

        listOptionView[0].Init(UtilityLocalization.GetPlotLocalization(optionKey1));
        listOptionView[1].Init(UtilityLocalization.GetPlotLocalization(optionKey2));

        objChoiseRect.DOSizeDelta(targetSize2, 0.3f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() =>
        {
            ActingSystem.Instance.StopActing(this.name);
        });
    }

    //全屏点击
    void OnFullScreenClick(bool isClick)
    {
        maskContinueCanvasGroup.interactable = isClick;
        maskContinueCanvasGroup.blocksRaycasts = isClick;
    }

    //玩家选择选项,Button事件
    public void OnButtonOption(int optionIndex)
    {
        DoneOption(optionIndex);
        //开启全屏继续按钮
        OnFullScreenClick(true);
    }

    //玩家选择选项,进入下一条目
    void DoneOption(int optionIndex)
    {
        //ActingSystem.Instance.OnActing();
        foreach (var item in listOptionView)
        {
            item.Reset();
        }

        objOption.SetActive(false);

        GameObject obj = Instantiate(prefabLobbySlot, container);
        obj.GetComponent<PlotLobbySlotView>().Init(currentPlotItem, optionIndex);
        //滚动到最底部
        DOTween.To(() => scrollRect.verticalNormalizedPosition,
              x => scrollRect.verticalNormalizedPosition = x,
              0f, 0.3f)
              .SetEase(Ease.OutCubic)
              .SetUpdate(true);

        objSkip.SetActive(true);
    }

    public void OnSkip()
    {
        isSkip = true;
        ActingSystem.Instance.OnActing(this.name);
        PlotSystem.Instance.OnSkipPlot();
        objSkip.SetActive(false);
        objTextContinue.SetActive(false);
    }


    public void OnScroll()
    {
        // ResetCD();
    }

    public void OnClose()
    {
        PlotSystem.Instance.OnNextStep();
    }
}

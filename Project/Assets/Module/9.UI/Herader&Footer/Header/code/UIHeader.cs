using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIHeader : Singleton<UIHeader>
{
    public TextMeshProUGUI textUserName;
    [SerializeField] TextMeshProUGUI textPower;
    [SerializeField] TextMeshProUGUI textCoin, textGem, textTavern;
    [SerializeField] GameObject objHubGem, objHubCoin, objHubProfile, objHubEnergy, objHubTavern;
    Dictionary<string, GameObject> mapHideHub;
    private void Start()
    {
        //EventManager.StartListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_INIT_UI, OnInit);
        EventManager.StartListening<UIHeaderItemNumArgs>(EventNameHeader.EVENT_HEADER_UPDATE_ITEM_NUM_UI, OnRefreshItemNumDisplay);
        //显示隐藏顶部栏
        EventManager.StartListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_UI, OnShowHeader);
        EventManager.StartListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_HIDE_UI, OnHideHeader);
        EventManager.StartListening<UIHeaderShowHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_HIDE_HUB_UI, OnShowHideHub);

        //EventManager.StartListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_REFRESH_PROFILE, OnRefreshProfile);

        EventManager.StartListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_REFRESH_POWER, OnRefreshPower);

        mapHideHub = new Dictionary<string, GameObject>();
        mapHideHub.Add("profile", objHubProfile);
        mapHideHub.Add("coin", objHubCoin);
        mapHideHub.Add("gem", objHubGem);
        mapHideHub.Add("energy", objHubEnergy);
        mapHideHub.Add("tavern", objHubTavern);
    }

    private void OnDestroy()
    {
        //EventManager.StopListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_INIT_UI, OnInit);
        EventManager.StopListening<UIHeaderItemNumArgs>(EventNameHeader.EVENT_HEADER_UPDATE_ITEM_NUM_UI, OnRefreshItemNumDisplay);
        //显示隐藏顶部栏
        EventManager.StopListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_UI, OnShowHeader);
        EventManager.StopListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_HIDE_UI, OnHideHeader);
        EventManager.StopListening<UIHeaderShowHeaderArgs>(EventNameHeader.EVENT_HEADER_SHOW_HIDE_HUB_UI, OnShowHideHub);

        //EventManager.StopListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_REFRESH_PROFILE, OnRefreshProfile);

        EventManager.StopListening<UIHeaderArgs>(EventNameHeader.EVENT_HEADER_REFRESH_POWER, OnRefreshPower);
    }

    void OnRefreshPower(UIHeaderArgs args)
    {
        textPower.text = args.power.ToString();
    }

    void OnShowHideHub(UIHeaderShowHeaderArgs args)
    {
        //默认显示所有模块
        foreach (GameObject hub in mapHideHub.Values)
        {
            hub.SetActive(false);
        }

        //如果为空，不显示任何模块
        if (args.listShowHub == null)
        {
            return;
        }

        //隐藏指定模块
        foreach (string hideHub in args.listShowHub)
        {
            mapHideHub[hideHub].SetActive(true);
        }

        if (args.listShowHub.Contains("energy"))
        {
            //RefreshEnergy();
        }
    }

    //刷新顶部栏物品数量
    private void OnRefreshItemNumDisplay(UIHeaderItemNumArgs args)
    {
        TextMeshProUGUI targetText;
        switch (args.itemName)
        {
            case ConstantItem.COIN:
                targetText = textCoin;
                break;
            case ConstantItem.GEM:
                targetText = textGem;
                break;
            case ConstantItem.ENERGY:
                //RefreshEnergy();
                return;
            default:
                return;
        }

        // 先停止相同targetText上的所有动画
        // 无论是否使用动画，都先停止
        DOTween.Kill(targetText);

        if (args.isPlayAnimation)
        {
            OnPlayAnimation(args.endValue - args.startValue, args.startValue, args.endValue, targetText);
        }
        else
        {
            DOTween.Kill(targetText);
            targetText.text = args.endValue.ToString();
        }
    }

    // 添加静态计数器，避免ID冲突
    int animationCounter = 0;
    //播放动画
    void OnPlayAnimation(int add, int startValue, int endValue, TextMeshProUGUI targetText)
    {
        int changeValue = 0;
        float delay = 0.3f;
        float duration = 1f;

        // 使用递增计数器生成唯一ID
        string uniqueId = $"currencyAnimation_{animationCounter++}";

        DOTween.To(() => changeValue, x => changeValue = x, add, duration).
                       OnStart(() => { targetText.text = startValue.ToString(); }).
                       OnUpdate(() => { targetText.text = (startValue + changeValue).ToString(); }).
                       OnComplete(() => { targetText.text = endValue.ToString(); }).
                       SetId<Tween>(uniqueId).  // 使用唯一ID
                       SetDelay(delay).
                       SetEase(Ease.Linear);
    }

    void OnShowHeader(UIHeaderArgs args)
    {
        gameObject.SetActive(true);

    }

    void OnHideHeader(UIHeaderArgs args)
    {
        gameObject.SetActive(false);
    }
}
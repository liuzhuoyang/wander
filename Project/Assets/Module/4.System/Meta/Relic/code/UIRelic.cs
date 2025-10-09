using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遗物界面
/// </summary>
public class UIRelic : UIBase
{
    [SerializeField] GameObject prefabRarity;
    [SerializeField] RectTransform rectContent;
    [SerializeField] RelicInfo relicInfo;
    [SerializeField] ScrollRect scrollRect;

    void Awake()
    {
        EventManager.StartListening<UIRelicArgs>(EventNameRelic.EVENT_RELIC_REFRESH_UI, RefreshUI);
        EventManager.StartListening<RelicSlotViewArgs>(EventNameRelic.EVENT_RELIC_REFRESH_INFO, RefreshInfo);
        EventManager.StartListening<RelicSlotViewArgs>(EventNameRelic.EVENT_RELIC_REFRESH_STARUP, RefreshStarUp);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIRelicArgs>(EventNameRelic.EVENT_RELIC_REFRESH_UI, RefreshUI);
        EventManager.StopListening<RelicSlotViewArgs>(EventNameRelic.EVENT_RELIC_REFRESH_INFO, RefreshInfo);
        EventManager.StopListening<RelicSlotViewArgs>(EventNameRelic.EVENT_RELIC_REFRESH_STARUP, RefreshStarUp);
    }

    void RefreshUI(UIRelicArgs args)
    {
        relicInfo.gameObject.SetActive(false);

        foreach (Transform child in rectContent)
        {
            Destroy(child.gameObject);
        }
        foreach (var data in args.dictRelicSlotViewArgs)
        {
            GameObject obj = Instantiate(prefabRarity, rectContent);
            obj.GetComponent<RelicRarityCom>().Init(data.Key, data.Value);
        }
        if (args.needRefresh)
        {
            StartCoroutine(RefreshLayoutNextFrame());
        }
    }
    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectContent);
        scrollRect.verticalNormalizedPosition = 1;
    }
    void RefreshInfo(RelicSlotViewArgs args)
    {
        relicInfo.gameObject.SetActive(true);
        relicInfo.Init(args);
    }
    void RefreshStarUp(RelicSlotViewArgs args)
    {
        relicInfo.RefreshStar();
    }
    public void OnClose()
    {
        base.CloseUI();
    }
}
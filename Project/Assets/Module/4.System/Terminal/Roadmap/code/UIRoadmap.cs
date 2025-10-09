using System.Linq;
using UnityEngine;
public class UIRoadmap : UIBase
{
    [SerializeField] RectTransform rectRoadmap;
    [SerializeField] GameObject prefabRoadmapSlot;
    [SerializeField] GameObject objBtnClaim, objBtnGray;
    void Awake()
    {
        EventManager.StartListening<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_UI, OnRefresh);
        EventManager.StartListening<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_CLAIM, OnRefreshClaim);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_UI, OnRefresh);
        EventManager.StopListening<UIRoadmapArgs>(EventNameRoadmap.EVENT_ROADMAP_ON_REFRESH_CLAIM, OnRefreshClaim);
    }

    void OnRefresh(UIRoadmapArgs args)
    {
        foreach (Transform child in rectRoadmap)
        {
            Destroy(child.gameObject);
        }
        foreach (RoadmapSlotArgs slotArgs in args.listRoadmapSlot)
        {
            GameObject obj = Instantiate(prefabRoadmapSlot, rectRoadmap);
            obj.GetComponent<RoadmapSlot>().Init(slotArgs);
        }
        bool canClaim = RoadmapSystem.Instance.IsCanClaim();
        objBtnClaim.SetActive(canClaim);
        objBtnGray.SetActive(!canClaim);
    }

    public void OnClose()
    {
        base.CloseUI();
        TerminalSystem.Instance.Open();
    }

    void OnRefreshClaim(UIRoadmapArgs args)
    {
        bool canClaim = RoadmapSystem.Instance.IsCanClaim();
        objBtnClaim.SetActive(canClaim);
        objBtnGray.SetActive(!canClaim);
        foreach (Transform child in rectRoadmap)
        {
            child.GetComponent<RoadmapSlot>().RefreshBtn();
        }
    }

    public void OnClaim()
    {
        RoadmapSystem.Instance.OnClaim();
    }
}
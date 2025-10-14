using UnityEngine;

//Tooltip的全局UI，默认已经预先挂载好
public class UITooltip : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject mask;

    [SerializeField] GameObject prefabText;
    [SerializeField] GameObject prefabItem;

    void Start()
    {
        EventManager.StartListening<UITooltipArgs>(EventNameTooltip.EVENT_TOOLTIP_OPEN_UI, OnOpen);
        EventManager.StartListening<UITooltipArgs>(EventNameTooltip.EVENT_TOOLTIP_CLOSE_UI, OnClose);
        mask.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UITooltipArgs>(EventNameTooltip.EVENT_TOOLTIP_OPEN_UI, OnOpen);
        EventManager.StopListening<UITooltipArgs>(EventNameTooltip.EVENT_TOOLTIP_CLOSE_UI, OnClose);
    }

    void OnOpen(UITooltipArgs args)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        GameObject objContent;
        switch (args.tooltipType)
        {
            case TooltipType.Text:
                objContent = Instantiate(prefabText, container);
                objContent.GetComponent<TooltipViewText>().Init();
                break;
            case TooltipType.ItemHub:
                TooltipItemHubArgs tooltipItemHubArgs = args as TooltipItemHubArgs;
                objContent = Instantiate(prefabItem, container);
                //TooltipItemHubArgs tooltipItemHubArgs = args.tooltipArgs as TooltipItemHubArgs;
                objContent.GetComponent<TooltipViewItemHub>().Init(tooltipItemHubArgs);
                break;
        }

        mask.SetActive(true);
    }

    public void OnClose()
    {
        mask.SetActive(false);

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    void OnClose(UITooltipArgs args)
    {
        OnClose();
    }
}

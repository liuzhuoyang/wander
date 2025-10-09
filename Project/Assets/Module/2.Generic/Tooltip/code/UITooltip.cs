using UnityEngine;

//Tooltip的全局UI，默认已经预先挂载好
public class UITooltip : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject mask;

    [SerializeField] GameObject prefabText;
    [SerializeField] GameObject prefabImage;
    [SerializeField] GameObject prefabItem;
    [SerializeField] GameObject prefabItemInfo;

    void Start()
    {
        EventManager.StartListening<UITooltipArgs>(EventNameTooltip.EVENT_OPEN_UI, OnOpen);
        EventManager.StartListening<UITooltipArgs>(EventNameTooltip.EVENT_CLOSE_UI, OnSystemClose);
        mask.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UITooltipArgs>(EventNameTooltip.EVENT_OPEN_UI, OnOpen);
        EventManager.StopListening<UITooltipArgs>(EventNameTooltip.EVENT_CLOSE_UI, OnSystemClose);
    }

    void OnOpen(UITooltipArgs args)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        GameObject objContent;
        switch (args.tooltipArgs.tooltipType)
        {
            case TooltipType.TEXT:
                objContent = Instantiate(prefabText, container);
                objContent.GetComponent<TooltipViewText>().Init(args.tooltipArgs.contentArgs, args.tooltipArgs.posArgs);
                break;
            case TooltipType.IMAGE:
                objContent = Instantiate(prefabImage, container);
                objContent.GetComponent<TooltipViewImage>().Init(args.tooltipArgs.contentArgs, args.tooltipArgs.posArgs);
                break;
            case TooltipType.ITEMREWARD:
                objContent = Instantiate(prefabItem, container);
                objContent.GetComponent<TooltipViewItemReward>().Init(args.tooltipArgs.contentArgs, args.tooltipArgs.posArgs);
                break;
            case TooltipType.ITEMINFO:
                objContent = Instantiate(prefabItemInfo, container);
                objContent.GetComponent<TooltipViewItemInfo>().Init(args.tooltipArgs.contentArgs, args.tooltipArgs.posArgs);
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

    void OnSystemClose(UITooltipArgs args)
    {
        OnClose();
    }
}

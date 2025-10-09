using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选择物品弹窗
/// </summary>
public class PopupItemInfoArgs : PopupArgs
{
    public string itemName;
}

public class PopupItemInfo : PopupBase
{
    [SerializeField] ItemViewSlot itemViewSlot;
    [SerializeField] TextMeshProUGUI textName, textCount, textInfo;
    [SerializeField] GameObject objGet;
    [SerializeField] RectTransform rectGetContent,rectGet;
    [SerializeField] GameObject objPrefab;

    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        PopupItemInfoArgs popupItemInfoArgs = args as PopupItemInfoArgs;
        //获取道具数据
        ItemData itemData = AllItem.dictData[popupItemInfoArgs.itemName];
        //道具icon
        itemViewSlot.Init(popupItemInfoArgs.itemName, 0);
        //道具名称
        textName.text = UtilityLocalization.GetLocalization(itemData.displayName);
        //道具数量
        string colorHex = ColorUtility.ToHtmlStringRGBA(UtilityColor.Instance.GetTextEmphasisColorGreen());
        textCount.text = UtilityLocalization.GetLocalization("dynamic/quantity_x",
            $"<color=#{colorHex}>" + ItemSystem.Instance.GetItemNum(popupItemInfoArgs.itemName) + "</color>");
        //道具描述
        textInfo.text = UtilityLocalization.GetLocalization(itemData.infoKey);
        
        //获取方式
        if (itemData.listNavigatorName.Count <= 0)
        {
            objGet.SetActive(false);
            return;
        }

        objGet.SetActive(true);
        foreach (Transform child in rectGetContent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (string navigatorName in itemData.listNavigatorName)
        {
            GameObject obj = Instantiate(objPrefab, rectGetContent);
            obj.GetComponent<PopupItemInfoSlot>().Init(navigatorName, OnClose);
        }
        
        StartCoroutine(RefreshLayoutNextFrame());
    }

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectGet);
    }
}
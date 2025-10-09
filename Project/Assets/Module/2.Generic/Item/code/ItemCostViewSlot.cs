using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 通用item view slot
/// </summary>
public class ItemCostViewSlot : MonoBehaviour
{
    public Image imgTarget;
    public Image imgFrame;
    public TextMeshProUGUI textNum;
    public SlicedFilledImage imgFill;
    public GameObject objShard;
    string itemName;

    public void Init(string itemName, int num, int maxNum)
    {
        this.itemName = itemName;
        objShard.SetActive(itemName.Contains("item_shard"));

        string iconName = itemName;
        
        GameAssetControl.AssignIcon(iconName, imgTarget);

        ItemData itemData = AllItem.dictData[itemName];
        Rarity rarity = itemData.rarity;

        if (rarity != Rarity.None)
            GameAssetControl.AssignSpriteUI(ItemUtility.GetRarityFrameName(rarity), imgFrame);

        textNum.text = num.ToString() + "/" + maxNum.ToString();
        imgFill.fillAmount = (float)num / maxNum;
    }

    public void OnClick()
    {
        // TooltipManager.Instance.ShowTooltipItemInfo(itemName, GetComponent<RectTransform>(), transform.position, Direction.Top);
    }
}

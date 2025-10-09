using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// 通用item view slot
/// </summary>
public class ItemViewSlot : MonoBehaviour
{
    public Image imgTarget;
    public Image imgFrame;
    public TextMeshProUGUI textNum;
    public GameObject objShard;
    private ItemViewSlotArgs args;

    void Init(string itemName, int num, Rarity rarity)
    {
        objShard.SetActive(itemName.Contains("item_shard"));
        GameAssetControl.AssignIcon(itemName, imgTarget);
        if (rarity != Rarity.None)
            GameAssetControl.AssignSpriteUI(ItemUtility.GetRarityFrameName(rarity), imgFrame);
        if (num > 1)
            textNum.text = num.ToString();
        else
            textNum.gameObject.SetActive(false);
    }

    public void Init(string itemName, int num)
    {
        objShard.SetActive(itemName.Contains("item_shard"));
        var itemData = AllItem.dictData[itemName];
        GameAssetControl.AssignSpriteUI(itemData.iconName, imgTarget);
        GameAssetControl.AssignSpriteUI(ItemUtility.GetRarityFrameName(itemData.rarity), imgFrame);
        if (num > 0)
        {
            textNum.gameObject.SetActive(true);
            textNum.text = num.ToString();
        }
        else
        {
            textNum.gameObject.SetActive(false);
        }
    }

    public void ShowShard(bool show)
    {
        objShard.SetActive(show);
    }

    public void Init(ItemViewSlotArgs args)
    {
        this.args = args;
        Init(args.itemName, 1, args.rarity);
    }

    public void OnClick()
    {
        if (args == null || !args.isInteractable)
        {
            return;
        }
    }

    public void SetClickAction(Action action)
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => action?.Invoke());
    }
}

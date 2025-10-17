using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBattleItemSlot : MonoBehaviour
{
    public FormationWorldItem formationItem;

    public Transform commonTransform;
    public Transform rareTransform;
    public Transform epicTransform;
    public Transform legendaryTransform;

    public Image imgIcon;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI info;

    public DoSlicedBar doSlicedBar;

    public FormationItem itemdata;

    public void Init(FormationItem formationItem)
    {
        itemdata = formationItem;
        imgIcon.sprite = formationItem.itemConfig.itemIcon;
        levelText.text = formationItem.itemConfig.level.ToString();
        info.text = UtilityLocalization.GetLocalization(formationItem.itemConfig.itemName);
        SetRarityImage(itemdata.Rarity);
        RefreshSetBar();
    }


    public void SetRarityImage(Rarity rarity)
    {
        commonTransform.gameObject.SetActive(rarity == Rarity.Common);
        rareTransform.gameObject.SetActive(rarity == Rarity.Rare);
        epicTransform.gameObject.SetActive(rarity == Rarity.Epic);
        legendaryTransform.gameObject.SetActive(rarity == Rarity.Legendary);
    }

    public void Refresh()
    {
        imgIcon.sprite = itemdata.itemConfig.itemIcon;
        levelText.text = itemdata.itemConfig.level.ToString();
        info.text = UtilityLocalization.GetLocalization(itemdata.itemConfig.itemName);
        SetRarityImage(itemdata.Rarity);
        RefreshSetBar();
    }

    //刷新充能条
    public void RefreshSetBar()
    {
        if (itemdata.isActivated)
        {
            if (itemdata.isInCooldown)
            {
                doSlicedBar.OnSetFill(0, itemdata.cooldownTime);
            }
            else
            {
                doSlicedBar.OnSetFill(0);
            }
        }
        else
        {
            doSlicedBar.OnSetFill((float)itemdata.currentChargeCount / (float)itemdata.requiredChargeCount);
        }

    }
}

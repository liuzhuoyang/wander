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
        SetRarityImage(itemdata.rarity);
        RefreshSetBar();
    }


    public void SetRarityImage(Rarity rarity)
    {
        commonTransform.gameObject.SetActive(rarity == Rarity.Common);
        rareTransform.gameObject.SetActive(rarity == Rarity.Rare);
        epicTransform.gameObject.SetActive(rarity == Rarity.Epic);
        legendaryTransform.gameObject.SetActive(rarity == Rarity.Legendary);
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
                doSlicedBar.OnSetFill(itemdata.energyConsumption / itemdata.requiredEnergyConsumption);
            }
        }
        else
        {
            doSlicedBar.OnSetFill(itemdata.currentChargeCount / itemdata.requiredChargeCount);
        }

    }
}

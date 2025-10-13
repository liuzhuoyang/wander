using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGearSlot : MonoBehaviour
{
    public Image imgIcon;
    public DoSlicedBar doSlicedBar;

    public Transform commonImage;
    public Transform rareImage;
    public Transform epicImage;
    public Transform legendaryImage;

    public TextMeshProUGUI textName;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI barText;


    public Transform equipTransform;
    public Transform lockTransform;

    GearSlotViewArgs args;

    public void Init(GearSlotViewArgs args)
    {
        this.args = args;
        textName.text = args.displayName;
        SetRarityImage(args.rarity);
        GameAssetControl.AssignIcon(args.iconName, imgIcon);

        equipTransform.gameObject.SetActive(args.isEquip);
        lockTransform.gameObject.SetActive(!args.isLocked);

        if (args.isLocked)
        {
            doSlicedBar.OnSetFill(args.progress);
            levelText.text = args.level.ToString();
            barText.text = $"{args.count}/{args.needCount}";
        }
    }

    public void SetRarityImage(Rarity rarity)
    {
        commonImage.gameObject.SetActive(rarity == Rarity.Common);
        rareImage.gameObject.SetActive(rarity == Rarity.Rare);
        epicImage.gameObject.SetActive(rarity == Rarity.Epic);
        legendaryImage.gameObject.SetActive(rarity == Rarity.Legendary);
    }

    public void OnClick()
    {
        // 确保卡片的点击事件能够正常触发
        // 这里可以添加一些额外的逻辑，比如检查是否已装备等
        GearSystem.Instance.OnClickSlot(args.gearName);
    }
}


public class GearSlotViewArgs
{
    public string gearName;
    public string displayName;
    public string iconName;
    public Rarity rarity;

    public bool isEquip;
    public bool isLocked;

    //解锁后的进度
    public float progress;
    public int level;
    public int count;
    public int needCount;
}

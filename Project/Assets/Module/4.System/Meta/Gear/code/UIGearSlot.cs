using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGearSlot : MonoBehaviour
{
    public Image imgIcon;
    public DoSlicedBar doSlicedBar;
    
    public TextMeshProUGUI textName;
    public TextMeshProUGUI levelText;

    public void Init(GearSlotViewArgs args)
    {
        GameAssetControl.AssignSpriteUI(args.icon, imgIcon);
        doSlicedBar.OnSetFill(args.progress);
        textName.text = args.name;
        levelText.text = args.level.ToString();
    }
}


public class GearSlotViewArgs
{
    public string icon;
    public float progress;
    public string name;
    public int level;
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardViewSlot : MonoBehaviour
{
    public Image imgFrame;
    public Image imgItem;
    public TextMeshProUGUI textNum;
    public GameObject objShard;
    [SerializeField] GameObject hubText;

    public void Init(RewardArgs args)
    {
        var itemArgs = AllItem.dictData[args.reward];
        GameAssetControl.AssignSpriteUI(ItemUtility.GetRarityFrameName(itemArgs.rarity), imgFrame);
        // GameAssetsManager.Instance.AssignIcon(string.IsNullOrEmpty(args.showName) ? args.reward : args.showName, imgItem);
        GameAssetControl.AssignSpriteUI(itemArgs.iconName, imgItem);
        if (args.num <= 0)
        {
            hubText.gameObject.SetActive(false);
        }
        else
        {
            hubText.gameObject.SetActive(true);
            textNum.text = args.num.ToString();
        }
        objShard.SetActive(itemArgs.isShard);
    }

    public void Init(RewardShowArgs args)
    {
        var itemArgs = AllItem.dictData[args.name];
        GameAssetControl.AssignSpriteUI(ItemUtility.GetRarityFrameName(itemArgs.rarity), imgFrame);
        GameAssetControl.AssignSpriteUI(itemArgs.iconName, imgItem);
        if (args.num <= 0)
        {
            hubText.gameObject.SetActive(false);
        }
        else
        {
            hubText.gameObject.SetActive(true);
            textNum.text = args.num.ToString();
        }
        objShard.SetActive(itemArgs.isShard);
    }
    public void SetReward(RewardShowArgs args, bool showNum)
    {
        Init(args);
        hubText.gameObject.SetActive(showNum);
    }
    public void SetNum(int num)
    {
        textNum.text = num.ToString();
    }
    public void AddNum(int num)
    {
        textNum.text = (int.Parse(textNum.text) + num).ToString();
    }
}

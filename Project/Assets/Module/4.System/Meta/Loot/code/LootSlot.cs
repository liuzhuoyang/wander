using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    [SerializeField] GameObject objSelect, objPin;
    [SerializeField] Image imgIcon, imgBg;
    [SerializeField] TextMeshProUGUI textCount;
    int slotIndex;

    public void Init(int index)
    {
        slotIndex = index;
        GameAssetControl.AssignIcon("item_loot_box_" + (index + 1), imgIcon);
    }

    public void Refresh(int chooseIndex, int count)
    {
        objSelect.SetActive(slotIndex == chooseIndex);
        textCount.text = count.ToString();
        if (count > 0)
        {
            objPin.SetActive(true);
            textCount.color = Color.white;
            imgBg.color = Color.white;
            imgIcon.color = Color.white;
        }
        else
        {
            objPin.SetActive(false);
            textCount.color = Color.gray;
            imgBg.color = Color.gray;
            if (slotIndex == chooseIndex)
            {
                imgIcon.color = Color.white;
            }
            else
            {
                imgIcon.color = Color.gray;
            }
        }
    }

    public void OnClick()
    {
        LootSystem.Instance.OnClickSlot(slotIndex);
    }
}

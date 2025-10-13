using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGearPropertySlot : MonoBehaviour
{
    public Image imgIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI valveText;

    public void SetDetail(Transform detail, float value, float addValue, DetailType detailType)
    {
        string valueText = ((int)value).ToString();
        //textName.text = detailType.ToString();
        switch (detailType)
        {
            case DetailType.Atk:
                valueText = (value).ToString();
                // GameAssetControl.AssignIcon(ConstantItem.ATK, imgIcon);
                break;
            case DetailType.TriggerSpeed:
                valueText = (value).ToString();
                // GameAssetControl.AssignIcon(ConstantItem.TRIGGER_SPEED, imgIcon);
                break;
        }


        valveText.text = valueText;
        if (addValue > 0)
        {
            valveText.text += $"<color=#FFD700><b>+{((int)addValue).ToString()}</b></color>";
            valveText.richText = true;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TagView : MonoBehaviour
{
    [SerializeField] Image imgFrame;
    [SerializeField] TextMeshProUGUI textName;

    TagData tagData;
    public void Init(string tagName)
    {
        tagData = AllTag.dictData[tagName];
        textName.text = UtilityLocalization.GetLocalization(tagData.displayName);

        string frameName = "ui_" + tagName;
        GameAssetControl.AssignSpriteUI(frameName, imgFrame);

        textName.color = tagData.color;
    }

    public void OnClick()
    {
        string content = UtilityLocalization.GetLocalization(tagData.contentKey);
        TooltipManager.Instance.ShowTooltipText(new List<string> { content }, transform as RectTransform, transform.position, Direction.Bottom);
    }
}

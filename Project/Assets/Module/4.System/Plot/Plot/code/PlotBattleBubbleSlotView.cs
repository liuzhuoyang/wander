using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlotBattleBubbleSlotView : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image imgAvatar;
    public Transform transMain;
    public void Init(string avatarName, string dialogKey)
    {
        GameAssetControl.AssignIcon(avatarName, imgAvatar);
        text.text = UtilityLocalization.GetPlotLocalization(dialogKey);
        transMain.localScale = Vector3.zero;
        transMain.DOScale(Vector3.one, 0.25f);
    }

    public void OnHide()
    {
        transMain.DOScale(Vector3.zero, 0.25f);
    }
}

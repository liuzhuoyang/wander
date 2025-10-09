using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlotLobbySlotView : MonoBehaviour
{
    [SerializeField] GameObject objMain;
    [SerializeField] GameObject objFrame; 
    [SerializeField] GameObject objAvatarLeft;
    [SerializeField] GameObject objAvatarRight;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] AvatarSlotView avatarSlotViewLeft;
    [SerializeField] AvatarSlotView avatarSlotViewRight;

    public void Init(PlotItem plotItem, int optionIndex = 1, bool isResize = true)
    {
        AvatarData avatarData;
        bool isPlayer = false;
        if(string.IsNullOrEmpty(plotItem.avatarNPC))
        {
            isPlayer = true;
        }

        string avatarName = plotItem.avatarNPC;
        if(isPlayer)
        {
            avatarName = GameData.userData.userProfile.userAvatar;
        }

        objAvatarLeft.SetActive(!isPlayer);
        objAvatarRight.SetActive(isPlayer);

        avatarData = AllAvatar.dictData[avatarName];
        avatarSlotViewLeft.InitStatic(avatarData);
        avatarSlotViewRight.InitStatic(avatarData);
        
        string contentKey = plotItem.dialogKey;
        if (plotItem.dialogType == PlotLobbyDialogType.Option)
        {
            if(optionIndex == 1)
            {
                contentKey = plotItem.optionKey1;
            }
            else
            {
                contentKey = plotItem.optionKey2;
            }
        }

        content.text = UtilityLocalization.GetPlotLocalization(contentKey);

        if (isResize)
        {
            Resize();
        }

        //对话框的动画
        objMain.transform.localScale = Vector3.zero;
        objMain.transform.DOScale(Vector3.one, 0.25f).SetUpdate(true);
    }
    

    void Resize()
    {
        float preferredHeight = content.preferredHeight;
        //对话框的高度自适应
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, preferredHeight);
        //外框的高度加40
        objFrame.GetComponent<RectTransform>().sizeDelta = new Vector2(objFrame.GetComponent<RectTransform>().sizeDelta.x, preferredHeight + 60);
        //对象的高度加60
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, preferredHeight + 80);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyViewLevel : MonoBehaviour
{

    public TextMeshProUGUI textChapterDisplayName;
    public TextMeshProUGUI textLevelID;
    public Image imgLevel;

    public void Init(string chapterDisplayName, string picName, int chapterID, int levelID)
    {
        GameAssetControl.AssignPicture(picName, imgLevel);

        textChapterDisplayName.text = UtilityLocalization.GetLocalization(chapterDisplayName);
        textLevelID.text = UtilityLocalization.GetLocalization("level/dynamic/level_x-y", $"{chapterID} - {levelID}");
    }
}

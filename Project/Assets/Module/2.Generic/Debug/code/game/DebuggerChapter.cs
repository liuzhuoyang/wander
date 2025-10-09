using UnityEngine;
using TMPro;
public class DebuggerChapter : MonoBehaviour
{
    public TMP_Dropdown dropdownChapter;
    public TMP_Dropdown dropdownLevel;

    public void Init()
    {
        dropdownChapter.options.Clear();
        foreach (var item in AllChapter.data.Keys)
        {
            dropdownChapter.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
        }

        dropdownChapter.value = 0;

        RefreshLevel();
    }

    void RefreshLevel()
    {
        int chapterID = int.Parse(dropdownChapter.options[dropdownChapter.value].text);
        ChapterData chapterData = AllChapter.data[chapterID];

        dropdownLevel.options.Clear();
        for (int i = 1; i <= chapterData.totalLevel; i++)
        {
            string levelName = "level_normal_" + chapterID.ToString("D3") + "_" + i.ToString("D2");
            dropdownLevel.options.Add(new TMP_Dropdown.OptionData(levelName));
        }

        dropdownLevel.value = 0;
    }

    public void OnSelectChapter()
    {
        RefreshLevel();
    }

    public void OnJump()
    {
        //int chapterID = dropdownChapter.value + 1;
        int levelID = dropdownLevel.value + 1;   
        GameData.userData.userLevel.levelProgressMain.levelIndex = levelID;

        Debugger.Instance.OnCloseDebug();
        LobbySystem.Instance.Refresh();        
    }
}

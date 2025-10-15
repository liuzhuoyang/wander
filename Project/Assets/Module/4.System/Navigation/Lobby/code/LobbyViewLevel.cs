using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>
/// 大厅关卡视图控制器
/// 负责关卡信息的显示和关卡切换动画
/// </summary>
public class LobbyViewLevel : MonoBehaviour
{
    [Header("UI 组件")]
    public Transform groupInfo;                    // 关卡信息组（章节名、关卡ID等）
    public Transform groupAction;                  // 操作组，有操作时候关闭，避免动画中的错误操作
    public TextMeshProUGUI textTitle;              // 章节显示名称文本
    public TextMeshProUGUI textBestWave;           
    //public Image imgLevel;                         // 关卡图片

    [Header("关卡切换动画")]
    public Transform groupSelected;                // 当前选中的关卡组
    public Transform groupIncoming;                // 即将进入的关卡组

    [Header("导航箭头")]
    public GameObject objArrowLeft;                // 左箭头按钮
    public GameObject objArrowRight;               // 右箭头按钮

    [Header("状态控制")]
    bool isAnimating = false;                      // 是否正在播放动画


    public void Init(string themeName, int themeVarient, int selectedLevel, int totalLevel)
    {
        groupSelected.GetChild(0).GetComponent<LevelSlot>().Init(themeName, themeVarient);

        CheckArrowButton(selectedLevel, totalLevel);
    }

    /// <summary>
    /// 刷新关卡信息显示
    /// </summary>
    public void OnRefresh(string displayName,  int levelID)
    {
        // 设置关卡图片
        //GameAssetControl.AssignPicture(picName, imgLevel);

        // 设置本地化文本
        textTitle.text = $"{levelID}. {UtilityLocalization.GetLocalization(displayName)}";
        //textBestWave.text = $"{levelID}";
        //+ UtilityLocalization.GetLocalization("level/dynamic/level_x-y", $"{chapterID} - {levelID}");
    }

    /// <summary>
    /// 切换关卡（由外部系统调用）
    /// </summary>
    public void OnChangeLevel(bool isNextLevel, int selectedLevel, int totalLevel, string commingThemeName, int commingThemeVarient)
    {
        CheckArrowButton(selectedLevel, totalLevel);
        
        // 隐藏关卡信息，避免在动画过程中显示错误信息
        groupInfo.gameObject.SetActive(false);

        if(isNextLevel)
        {
            // 下一个关卡：新关卡从右侧(2000)滑入，当前关卡向左(-2000)滑出
            OnAnimation(2000, -2000, commingThemeName, commingThemeVarient);
        }
        else
        {
            // 上一个关卡：新关卡从左侧(-2000)滑入，当前关卡向右(2000)滑出
            OnAnimation(-2000, 2000, commingThemeName, commingThemeVarient);
        }
    }
    
    /// <summary>
    /// 对象禁用时清理动画
    /// </summary>
    void OnDisable()
    {
        // 停止并清理所有正在进行的动画
        DOTween.Kill(groupIncoming.GetChild(0));
        DOTween.Kill(groupSelected.GetChild(0));
        isAnimating = false;
    }

    /// <summary>
    /// 对象启用时初始化状态
    /// </summary>
    void OnEnable()
    {
        // 设置初始显示状态
        groupSelected.GetChild(0).gameObject.SetActive(true);   // 显示当前关卡
        groupIncoming.GetChild(0).gameObject.SetActive(false);   // 隐藏待切换关卡

        // 重置位置
        groupSelected.GetChild(0).transform.localPosition = Vector2.zero;

        // 重置动画状态
        isAnimating = false;

        // 显示关卡信息
        groupInfo.gameObject.SetActive(true);
    }

    /// <summary>
    /// 执行关卡切换动画
    /// </summary>
    /// <param name="fromPos">新关卡起始位置（屏幕外位置）</param>
    /// <param name="toPos">当前关卡结束位置（屏幕外位置）</param>
    void OnAnimation(float fromPos, float toPos, string commingThemeName, int commingThemeVarient)
    {
        // 防止重复触发动画
        if(isAnimating) return;
        
        // 隐藏关卡信息，避免动画过程中显示错误信息
        groupInfo.gameObject.SetActive(false);

        // 设置动画状态
        isAnimating = true;
        
        // 获取当前关卡和下一个关卡的引用
        GameObject selectedLevel = groupIncoming.transform.GetChild(0).gameObject;  // 即将成为当前关卡的关卡
        GameObject nextLevel = groupSelected.transform.GetChild(0).gameObject;      // 当前关卡

        nextLevel.GetComponent<LevelSlot>().Init(commingThemeName, commingThemeVarient);

        // 交换父对象：让新关卡成为选中状态，当前关卡成为待切换状态
        selectedLevel.transform.SetParent(groupSelected);
        nextLevel.transform.SetParent(groupIncoming);
        
        // 激活两个关卡对象
        nextLevel.SetActive(true);
        selectedLevel.SetActive(true);

        // 设置新关卡的初始位置（屏幕外）
        selectedLevel.transform.localPosition = new Vector2(fromPos, 0);
        
        // 开始动画：
        // 1. 当前关卡移动到屏幕外
        // 2. 新关卡从屏幕外移动到中心位置
        nextLevel.transform.DOLocalMoveX(toPos, 0.3f).SetEase(Ease.Linear);
        selectedLevel.transform.DOLocalMoveX(0, 0.3f).SetEase(Ease.Linear).OnComplete(() => {
            // 动画完成后的处理
            nextLevel.SetActive(false);              // 隐藏已移出的关卡
            isAnimating = false;                     // 重置动画状态
            groupInfo.gameObject.SetActive(true);    // 显示关卡信息
        });
    }

    /// <summary>
    /// 检查箭头显示状态
    /// 根据当前关卡位置决定左右箭头是否显示
    /// </summary>
    void CheckArrowButton(int selectedLevel, int totalLevel)
    {
        bool isHideArrowLeft = selectedLevel == 1;
        bool isHideArrowRight = selectedLevel == totalLevel - 1;
        objArrowLeft.SetActive(!isHideArrowLeft);
        objArrowRight.SetActive(!isHideArrowRight);
    }

    #region 按钮事件
    /// 下一个关卡按钮点击事件
    public void OnNextLevel()
    {
        Debug.Log("=== LobbyViewLevel OnNextLevel ===");
        LobbySystem.Instance.OnChangeLevel(true);
    }

    /// 上一个关卡按钮点击事件
    public void OnPreviousLevel()
    {
        Debug.Log("=== LobbyViewLevel OnPreviousLevel ===");
        LobbySystem.Instance.OnChangeLevel(false);
    }
    #endregion
}

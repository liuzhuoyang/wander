using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PlotBattleView : MonoBehaviour
{
    [SerializeField] GameObject objFrame;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] Animator animator;
    [SerializeField] Image imgNPC;
    [SerializeField] Image imgPlayer;
    [SerializeField] GameObject objAvatarName, objPlayerName;
    [SerializeField] TextMeshProUGUI textAvatarName;
    [SerializeField] TextMeshProUGUI textPlayerName;
    [SerializeField] CanvasGroup overlayMaskCanvasGroup; //顶部遮罩，用于交互控制
    [SerializeField] GameObject objCursor; //对话完结后的光标

    string currentText = "";
    string fullText = "";
    Coroutine typer;
    bool isTyping;

    public void Init()
    {
        content.text = "";
        isTyping = false;

        //隐藏血条
        //EventManager.TriggerEvent<UIBattleMergeArgs>(EventNameModeBattle.EVENT_BATTLE_SHOW_HEALTH_UI, new UIBattleMergeArgs() { isShowHealthUI = false });
    }

    public void OnClose()
    {
        animator.SetTrigger("OnHide");
    }

    public void OnNext(PlotItem args)
    {
        UpdatePlot(args);
    }

    void UpdatePlot(PlotItem args)
    {
        //overlayMaskCanvasGroup.interactable = false;
        objCursor.SetActive(false);

        //如果没有配置，那么就是玩家
        bool isPlayer = false;
        if (string.IsNullOrEmpty(args.avatarNPC))
        {
            isPlayer = true;
        }

        string avatarName = args.avatarNPC;
        if (isPlayer)
        {
            //如果是玩家，给玩家的名字
            avatarName = GameData.userData.userProfile.userAvatar;
        }

        AvatarData avatarData = AllAvatar.dictData[avatarName];

        if (isPlayer)
        {
            imgPlayer.gameObject.SetActive(true);
            imgNPC.gameObject.SetActive(false);
            GameAssetControl.AssignSpriteUI("pic_" + GameData.userData.userProfile.userAvatar, imgPlayer);
            textPlayerName.text = UtilityLocalization.GetLocalization(avatarData.displayName);
            objAvatarName.SetActive(false);
            objPlayerName.SetActive(true);

            imgPlayer.transform.DOLocalMoveX(100f, 0.25f).From();
            imgPlayer.color = new Vector4(1, 1, 1, 0);
            imgPlayer.DOFade(1, 0.25f);
        }
        else
        {
            imgPlayer.gameObject.SetActive(false);
            imgNPC.gameObject.SetActive(true);
            GameAssetControl.AssignSpriteUI("pic_" + args.avatarNPC, imgNPC);
            textAvatarName.text = UtilityLocalization.GetLocalization(avatarData.displayName);
            objAvatarName.SetActive(true);
            objPlayerName.SetActive(false);

            imgNPC.transform.DOLocalMoveX(-100f, 0.25f).From();
            imgNPC.color = new Vector4(1, 1, 1, 0);
            imgNPC.DOFade(1, 0.25f);
        }

        fullText = UtilityLocalization.GetPlotLocalization(args.dialogKey);
        //content.text = fullText;

        InitTyping();
    }


    //打字机效果
    public void InitTyping(float typingSpeed = 0.008f)
    {
        isTyping = true;
        content.text = "";
        typer = StartCoroutine(TypeWriterEffect(fullText, typingSpeed));
    }

    IEnumerator TypeWriterEffect(string fullText, float typingSpeed)
    {
        //EventManager.StartListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_STOP_CURSOR_UI, OnStopTypingAndCursor);
        for (int i = 1; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            content.text = currentText; // + (showCursor ? "|" : "");  // 追加光标
            yield return new WaitForSeconds(typingSpeed);
        }
        DoneTyping();
    }

    void DoneTyping()
    {
        StopCoroutine(typer);
        content.text = fullText;
        //overlayMaskCanvasGroup.interactable = true;   
        objCursor.SetActive(true);
        isTyping = false;
    }

    public void OnButtonContinue()
    {
        if (isTyping)
        {
            DoneTyping();
            return;
        }

        PlotSystem.Instance.OnNextStep();
    }

    public void OnSkip()
    {
        PlotSystem.Instance.OnSkipPlot();
    }
}

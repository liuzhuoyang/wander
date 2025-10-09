using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class MailDetail : MonoBehaviour
{
    public GameObject prefabSlot;
    public TextMeshProUGUI textMailTitle;
    public TextMeshProUGUI textMailContent;
    public Transform containerMailReward;
    public GameObject objBtnReceive;
    public GameObject objBtnDefine;
    public GameObject rect;
    public ScrollRect contentRect;
    List<RewardArgs> listRewardArgs;
    int mailID;

    public void Open(MailArgs args)
    {
        foreach (Transform child in containerMailReward)
        {
            Destroy(child.gameObject);
        }
        mailID = args.id;
        if (args.isGM)
        {
            RefreshGMMail(args);
            return;
        }
        textMailTitle.text = args.title;
        textMailContent.text = args.content;
        contentRect.verticalNormalizedPosition = 1;

        if (string.IsNullOrEmpty(args.reward))
        {
            objBtnReceive.SetActive(false);
            objBtnDefine.SetActive(true);
            rect.gameObject.SetActive(false);
            return;
        }
        showBtn(args.status);
        listRewardArgs = MailSystem.Instance.getReward(args.reward);
        foreach (RewardArgs rewardArgs in listRewardArgs)
        {
            GameObject go = Instantiate(prefabSlot, containerMailReward);
            go.GetComponent<RewardViewSlot>().Init(rewardArgs);
        }
    }

    void RefreshGMMail(MailArgs args)
    {
        objBtnReceive.SetActive(false);
        objBtnDefine.SetActive(true);
        rect.gameObject.SetActive(false);
        textMailTitle.text = UtilityLocalization.GetLocalization("page/mail/page_mail_title_gm_mail");
        switch (args.functionType)
        {
            case MailFunctionType.ChangeChapter:
                textMailContent.text = UtilityLocalization.GetLocalization("page/mail/page_mail_content_gm_change_chapter", args.content);
                break;
            default:
                break;
        }
    }
    public void OnClose()
    {
        this.gameObject.SetActive(false);
    }

    public void OnConfirm()
    {
        MailSystem.Instance.OnConfirm(mailID);
        OnClose();
    }

    public void OnReceive()
    {
        MailSystem.Instance.OnReceive(mailID);
        showBtn(2);
    }

    void showBtn(int status)
    {
        if (status != 2)
        {
            objBtnReceive.SetActive(true);
            objBtnDefine.SetActive(false);
            rect.gameObject.SetActive(true);
        }
        else
        {
            objBtnDefine.SetActive(true);
            objBtnReceive.SetActive(false);
            rect.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

public class UITerminal : UIBase
{
    // [SerializeField] GameObject secondAry;
    [SerializeField] Animator animator;
    //点击邮件
    public void OnClickMail()
    {
        //MailSystem.Instance.Open();
        OnClose();
    }

    //点击设置
    public void OnClickSetting()
    {
        SettingManager.Instance.Open();
        OnClose();
    }

    //关闭界面
    public void OnClose()
    {
        CloseUI();
        //animator.SetTrigger("Close");
    }
}
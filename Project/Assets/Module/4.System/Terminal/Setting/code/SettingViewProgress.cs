using UnityEngine;

public class SettingViewProgress : MonoBehaviour
{
    //上传数据
    public GameObject btnUpload;
    //Facebook
    public GameObject objBtnFacebookLogin;
    public GameObject objBtnFacebookLogout;
    //Apple
    public GameObject objBtnAppleLogin;
    public GameObject objBtnAppleLogout;
    //Google
    public GameObject objBtnGoogleLogin;
    public GameObject objBtnGoogleLogout;

    public void OnOpen()
    {
        SettingManager.Instance.OnGetLinkStatus(() =>
        {
            Refresh();
            gameObject.SetActive(true);
        });
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        objBtnFacebookLogout.SetActive(SettingManager.Instance.linkStatusArgs.facebookID != "");
        objBtnFacebookLogin.SetActive(SettingManager.Instance.linkStatusArgs.facebookID == "");
        objBtnAppleLogout.SetActive(false);
        objBtnAppleLogin.SetActive(false);
        objBtnGoogleLogout.SetActive(SettingManager.Instance.linkStatusArgs.googleID != "");
        objBtnGoogleLogin.SetActive(SettingManager.Instance.linkStatusArgs.googleID == "");
#elif UNITY_IOS && !UNITY_EDITOR
        objBtnFacebookLogout.SetActive(SettingManager.Instance.linkStatusArgs.facebookID != "");
        objBtnFacebookLogin.SetActive(SettingManager.Instance.linkStatusArgs.facebookID == "");
        objBtnAppleLogout.SetActive(SettingManager.Instance.linkStatusArgs.appleID != "");
        objBtnAppleLogin.SetActive(SettingManager.Instance.linkStatusArgs.appleID == "");
        objBtnGoogleLogout.SetActive(SettingManager.Instance.linkStatusArgs.googleID != "");
        objBtnGoogleLogin.SetActive(SettingManager.Instance.linkStatusArgs.googleID == "");
#else
        objBtnFacebookLogout.SetActive(false);
        objBtnFacebookLogin.SetActive(false);
        objBtnAppleLogout.SetActive(false);
        objBtnAppleLogin.SetActive(false);
        objBtnGoogleLogout.SetActive(false);
        objBtnGoogleLogin.SetActive(false);
#endif
    }

    public void OnUnlinkFacebook()
    {
        SettingManager.Instance.OnUnlinkFacebook(() =>
        {
            Refresh();
        });
    }

    public void OnLinkFacebook()
    {
        SettingManager.Instance.OnLinkFacebook(() =>
        {
            Refresh();
        });
    }

    public void OnLinkApple()
    {
        SettingManager.Instance.OnLinkApple(() =>
        {
            Refresh();
        });
    }

    public void OnUnlinkApple()
    {
        SettingManager.Instance.OnUnlinkApple(() =>
        {
            Refresh();
        });
    }

    public void OnLinkGoogle()
    {

    }

    public void OnUnlinkGoogle()
    {

    }

    public void OnUploadData()
    {
        SettingManager.Instance.OnUploadData(
            () =>
            {
                btnUpload.SetActive(false);
            });
    }

    public void OnApple()
    {

    }

    public void OnDelete()
    {
        SettingManager.Instance.OnDeleteAccount();
    }
}

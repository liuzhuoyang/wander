using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingViewPrivacy : MonoBehaviour
{
    public GameObject btnConsentRevocation;

    public void OnOpen()
    {
        gameObject.SetActive(true);
        //只有GDPR地区才开启这个按钮
        //btnConsentRevocation.SetActive(ApplovinMaxControl.Instance.IsUserGDPR());
        btnConsentRevocation.SetActive(false);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnPrivacy()
    {
        Application.OpenURL(GameConfig.main.privacyPolicyUrl);
        //SettingManager.Instance.OnPrivacy();
    }

    public void OnTerms()
    {
        Application.OpenURL(GameConfig.main.termsOfServiceUrl);
        //SettingManager.Instance.OnTerms();
    }

/*
    public void OnTerms()
    {
        Application.OpenURL(GameConfig.main.termsOfServiceUrl);
    }*/

    public void OnDeleteAccount()
    {
        SettingManager.Instance.OnDeleteAccount();
    }

    public void OnManagePrivacy()
    {
        //ApplovinMaxControl.Instance.OnShowCmpForExistingUser();
    }
}

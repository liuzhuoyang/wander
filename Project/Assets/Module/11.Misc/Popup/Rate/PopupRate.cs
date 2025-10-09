using System.Collections;
using UnityEngine;

public class PopupRate : PopupBase
{
    public GameObject objMain;
    public GameObject objRate;
    public GameObject objFeedback;

    public void OnOpen()
    {
        /*
#if UNITY_ANDROID
        RateManager.Instance.InitGoogleInAppReview();
#endif*/
        objMain.SetActive(false);
        objRate.SetActive(true);
        objFeedback.SetActive(false);
    }

    public void OnThumbUp()
    {
        objMain.SetActive(false);
        objRate.SetActive(true);
        objFeedback.SetActive(false);
    }

    public void OnThumbDown()
    {
        objMain.SetActive(false);
        objRate.SetActive(false);
        objFeedback.SetActive(true);
    }

    public void OnRate()
    {
#if UNITY_ANDROID
        //RateManager.Instance.OnLaunchGoogleInAppReview();
#endif
        OnClose();
    }

    public void OnFeedback()
    {
        SettingManager.Instance.OnSupport();
    }
}

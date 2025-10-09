using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingViewLanguage : MonoBehaviour
{
    void OnLanguage(string language)
    {
        /*
        MessageManager popupConfirmArgs = new PopupConfirmArgs();
        popupConfirmArgs.popupName = "popup_confirm";
        popupConfirmArgs.locKey = "popup/popup_change_language_content";
        popupConfirmArgs.onConfirm = () =>
        {
            PlayerPrefs.SetString("loc", language.ToString());
            Game.Instance.Restart();
        };*/

        OnClose();

        //PopupManager.Instance.OnPopup<PopupConfirmArgs>(popupConfirmArgs);
    }

    public void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnEN()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_EN);
    }

    public void OnZHS()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_ZHS);
    }

    public void OnZHT()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_ZHT);
    }

    public void OnJA()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_JA);
    }

    public void OnKO()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_KO);
    }

    public void OnDE()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_DE);
    }

    public void OnFR()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_FR);
    }

    public void OnES()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_ES);
    }

    public void OnPT()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_PT);
    }

    public void OnIT()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_IT);
    }

    public void OnNL()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_NL);
    }

    public void OnRU()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_RU);
    }

    public void OnTH()
    {
        OnLanguage(ConstantLocKey.LANGUAGE_TH);
    }
}

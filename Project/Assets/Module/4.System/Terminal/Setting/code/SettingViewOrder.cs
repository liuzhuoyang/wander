using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingViewOrder : MonoBehaviour
{
    [SerializeField] TMP_InputField inputOrder;
    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnOpen()
    {
        gameObject.SetActive(true);
        inputOrder.text = "";
        //激活键盘
        inputOrder.ActivateInputField();
    }
}
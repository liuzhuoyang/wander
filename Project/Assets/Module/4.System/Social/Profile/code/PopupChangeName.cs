using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class PopupChangeNameArgs : PopupArgs
{
    public Action<string> onConfirm;
}

public class PopupChangeName : PopupBase
{
    [SerializeField] TMP_InputField inputField;
    float maxWidth = 500f;   // 允许的最大宽度（单位：TextMeshPro 的宽度单位，和 RectTransform 一致）

    PopupChangeNameArgs args;

    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        this.args = args as PopupChangeNameArgs;

        inputField.text = GameData.userData.userProfile.userName;

        //激活键盘
        inputField.ActivateInputField();
    }

    public void OnConfirm()
    {
        if (!CheckNameLegal())
        {
            return;
        }
        args.onConfirm?.Invoke(inputField.text);
        OnClose();
    }

    bool CheckNameLegal()
    {
        if (inputField.text == "")
        {
            return false;
        }
        return true; // All characters are valid
    }

    public void OnInputValueChanged()
    {
        string currentText = inputField.text;
        // 用当前输入框的 TextComponent 来计算宽度
        var tmpText = inputField.textComponent;
        Vector2 size = tmpText.GetPreferredValues(currentText);

        // Debug.Log("Current width: " + size.x);

        if (size.x > maxWidth)
        {
            // 超过宽度，自动截断到允许长度
            while (currentText.Length > 0 && tmpText.GetPreferredValues(currentText).x > maxWidth)
            {
                currentText = currentText.Substring(0, currentText.Length - 1);
            }

            inputField.text = currentText;

        }
    }
}

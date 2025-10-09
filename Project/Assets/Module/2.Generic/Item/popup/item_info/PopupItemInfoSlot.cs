using System;
using TMPro;
using UnityEngine;

public class PopupItemInfoSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textFeatureName;
    [SerializeField] GameObject objBtnGo;

    Action action;

    string navigatorName;
    public void Init(string navigatorName, Action action)
    {
        NavigatorData data = AllNavigator.dictData[navigatorName];
        this.navigatorName = navigatorName;
        this.action = action;
        textFeatureName.text = UtilityLocalization.GetLocalization(data.displayName);
        
        objBtnGo.SetActive(data.isNavigable);
    }
    public void OnGo()
    {
        //跳转到对应功能
        NavigatorSystem.Instance.OnNavigator(navigatorName);
        action?.Invoke();
    }
}

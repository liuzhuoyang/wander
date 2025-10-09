using UnityEngine;
using UnityEngine.UI;

public class UIForceCanvaUpdateHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //EventManager.StartListening<UIForceCanvaUpdateArgs>(EventNameUI.EVENT_UI_FORCE_CANVA_UPDATE, OnForceCanvaUpdate);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()GetComponent<RectTransform>());

        Canvas.ForceUpdateCanvases();
    }

}

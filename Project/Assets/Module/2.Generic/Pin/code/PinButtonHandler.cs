using System;
using onicore.editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinButtonHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PinData pinNodeData;
    [SerializeField] private GameObject pinObject;
    private PinNode pinNode;

    public void Start()
    {
        EventManager.StartListening<PinRestArgs>(EventNamePin.EVENT_ON_REST_PIN, RefreshPinView);

        //UI生成时，更新Pin
        if (PinSystem.Instance != null)
        {
            RefreshPinView(null);
        }
    }
    void OnEnable()
    {
        if (pinNode != null)
        {
            UpdatePinView(pinNode.isPined);
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening<PinRestArgs>(EventNamePin.EVENT_ON_REST_PIN, RefreshPinView);
    }
    //按下按钮时，自动触发Pin消除
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pinNode.selfResolve)
        {
            pinNode.SetNewPinState(false);
        }
    }
    void UpdatePinView(bool isPin)
    {
        if (pinObject == null)
        {
            return;
        }
        if (isPin)
        {
            pinObject.SetActive(true);
        }
        else
        {
            pinObject.SetActive(false);
        }
    }
    //更新Pin View
    void RefreshPinView(PinRestArgs args)
    {
        pinNode = PinSystem.Instance.pinGraph[pinNodeData.name];
        pinNode.PinUI_Update = UpdatePinView;
        UpdatePinView(pinNode.isPined);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;

public class PlotBattleBubbleView : MonoBehaviour
{
    [SerializeField] GameObject prefabBattleBubbleSlot;
    [SerializeField] Transform transContent;
    public void Init()
    {
        //gameObject.SetActive(true);
    }

    public void OnNext(PlotItem currentPlotItem)
    {
        foreach (Transform child in transContent)
        {
            Destroy(child.gameObject);
        }

        GameObject obj = Instantiate(prefabBattleBubbleSlot, transContent);
        obj.GetComponent<PlotBattleBubbleSlotView>().Init(currentPlotItem.avatarNPC, currentPlotItem.dialogKey);

        WaitForNextStep();
    }

    async void WaitForNextStep()
    {
        await UniTask.Delay(1500);
        foreach (Transform child in transContent)
        {
            child.GetComponent<PlotBattleBubbleSlotView>().OnHide();
        }
        await UniTask.Delay(500);
        PlotSystem.Instance.OnNextStep();
    }
}

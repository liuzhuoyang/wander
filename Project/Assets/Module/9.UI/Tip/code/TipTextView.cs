using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TipView : MonoBehaviour
{
    public TextMeshProUGUI textTip;
    public Transform transformTip;

    public void Init(string content)
    {
        Reset();

        /*
        textTip.text = content;
        textTip.DOFade(1, 0.135f).SetEase(Ease.OutSine);

        textTip.DOFade(0, 0.2f).SetEase(Ease.InSine).SetDelay(2).OnComplete(() => {
            gameObject.SetActive(false);
        });*/
        textTip.text = content;

        
        transformTip.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack);
        
        GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutSine);
        transformTip.DOLocalMoveY(transformTip.localPosition.y + 100, 0.8f).SetEase(Ease.InOutSine);
        GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.InSine).SetDelay(0.8f).OnComplete(() => {
           gameObject.SetActive(false);
        });
    }

    public void Reset()
    {
        transformTip.localPosition = Vector2.zero;
        transformTip.DOKill();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().DOKill();
    }
}

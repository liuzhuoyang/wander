using UnityEngine;
using TMPro;
using DG.Tweening;

public class TipUnlockView : MonoBehaviour
{
    public TextMeshProUGUI textTip;
    public Transform transformTip;

    public void Init(string content)
    {
        Reset();

        textTip.text = content;

        transformTip.DOLocalMove(Vector2.zero, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                transformTip.DOLocalMove(new Vector2(0, 300), 1f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            });
        });
    }

    public void Reset()
    {
        transformTip.localPosition = new Vector2(0, 300);
        transformTip.DOKill();
    }
}

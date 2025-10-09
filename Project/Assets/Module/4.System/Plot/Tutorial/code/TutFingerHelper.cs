using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class TutFingerHelper : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup canvasGroup;

    public GameObject objDrag;
    public GameObject objAnimation1;
    public GameObject objAnimation2;

    Sequence sequence;

    public void Init(Vector2 startPos, Vector2 endPos)
    {
        objDrag.SetActive(true);
        objAnimation1.SetActive(false);
        objAnimation2.SetActive(false);

        transform.position = startPos;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 30f), 0.5f));
        sequence.Append(transform.DOMove(endPos, 1f));
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f));

        sequence.Play().SetDelay(0.5f).SetLoops(-1).SetUpdate(true);

        animator.enabled = false;
    }

    public void Init(bool isMirror, float delayTime = 0f)
    {
        objDrag.SetActive(false);
        objAnimation1.SetActive(true);
        objAnimation2.SetActive(true);

        if (isMirror)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        canvasGroup.alpha = 0f;
        StartCoroutine(TimerTick.StartRealtime(delayTime, () =>
        {
            canvasGroup.alpha = 1f;
        }));
    }

    private void OnDestroy()
    {
        transform.DOKill();
        sequence.Kill();
    }
}

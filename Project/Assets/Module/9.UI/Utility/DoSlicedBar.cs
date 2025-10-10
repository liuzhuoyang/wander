using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(SlicedFilledImage))]
public class DoSlicedBar : MonoBehaviour
{
    private SlicedFilledImage img;

    void Awake()
    {
        img = GetComponent<SlicedFilledImage>();
        if (img == null)
        {
            Debug.LogError("DoSliced需要挂载在带有SlicedFilledImage组件的物体上");
        }
    }

    public void OnSetFill(float progress)
    {
        if (img == null) return;
        img.DOKill();
        img.fillAmount = progress;
    }

    public void OnSetFill(float progress, float duration, bool isReset = false, Action onComplete = null, bool isFlip = false)
    {
        if (img == null) return;
        img.DOKill();
        if (isReset)
        {
            img.fillAmount = 0;
        }

        img.DOFillAmount(progress, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    void OnDestroy()
    {
        img.DOKill();
    }
}

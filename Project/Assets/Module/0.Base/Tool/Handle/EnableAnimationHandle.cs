using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnableAnimationHandle : MonoBehaviour
{
    public EnableAnimationType animationType;
    public float delay;
    public bool ignoreTimeScale = false;

    Vector3 localPosition;
    private void Awake()
    {
        localPosition = transform.localPosition;
    }
    public enum EnableAnimationType
    {
        Scale = 0,
        ScaleSine = 1,
        Fade = 2,
        Rotate = 3,
        Jump = 4,
        Breath = 5,
        Flash = 6,
    }

    void OnEnable()
    {
        switch (animationType)
        {
            case EnableAnimationType.Scale:
                transform.DOKill();
                transform.localScale = Vector2.zero;
                if (ignoreTimeScale) transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetDelay(delay).SetUpdate(true);
                else transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetDelay(delay);
                break;
            case EnableAnimationType.ScaleSine:
                transform.DOKill();
                transform.localScale = Vector2.zero;
                transform.DOScale(Vector2.one, 0.16f).SetEase(Ease.OutSine).SetDelay(delay);
                break;
            case EnableAnimationType.Fade:
                transform.DOKill();
                transform.GetComponent<CanvasGroup>().alpha = 0;
                transform.GetComponent<CanvasGroup>().DOKill();
                transform.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.OutSine);
                break;
            case EnableAnimationType.Rotate:
                transform.DOKill();
                transform.eulerAngles = Vector3.zero;
                transform.DORotate(new Vector3(0, 0, 360), 12, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetUpdate(true);
                break;
            case EnableAnimationType.Jump:
                transform.DOKill();
                transform.DOLocalMoveY(transform.localPosition.y + 25, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetUpdate(true);
                break;
            case EnableAnimationType.Breath:
                transform.DOKill();
                transform.localScale = new Vector2(1.05f, 0.95f);
                transform.DOScale(new Vector2(0.95f, 1.05f), 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                break;
            case EnableAnimationType.Flash:
                transform.DOKill();
                transform.GetComponent<Image>().DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
        }
    }

    void OnDisable()
    {
        transform.DOKill();
    }

    void OnDestroy()
    {
        transform.DOKill();

        if (animationType == EnableAnimationType.Flash)
        {
            transform.GetComponent<Image>().DOKill();
        }
        
        if (animationType == EnableAnimationType.Fade)
        {
             transform.GetComponent<CanvasGroup>().DOKill();
        }
    }
}

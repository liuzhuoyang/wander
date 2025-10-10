using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public static class SlicedFilledImageDOTweenExtensions
{
    /// <summary>
    /// 为SlicedFilledImage创建fillAmount动画
    /// </summary>
    public static TweenerCore<float, float, FloatOptions> DOFillAmount(this SlicedFilledImage target, float endValue, float duration)
    {
        if (endValue > 1) endValue = 1;
        else if (endValue < 0) endValue = 0;
        
        TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
        t.SetTarget(target);
        return t;
    }
    
    /// <summary>
    /// 为SlicedFilledImage创建带有方向控制的fillAmount动画
    /// </summary>
    public static TweenerCore<float, float, FloatOptions> DOFillAmount(this SlicedFilledImage target, float endValue, float duration, SlicedFilledImage.FillDirection direction)
    {
        target.fillDirection = direction;
        return target.DOFillAmount(endValue, duration);
    }
}
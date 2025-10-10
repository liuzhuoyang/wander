using DG.Tweening;
using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace HUD_TEXT
{
    public class AttackResultData
    {
        public ElementType damageType;
        public float damage;
        public bool isCritical;
        public AttackResultData(float damage, ElementType damageType, bool isCrit)
        {
            this.damage = damage;
            this.damageType = damageType;
            this.isCritical = isCrit;
        }
    }
    public class HudTextDamage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textNum;
        [SerializeField] private GameObject objCrit;
        public void Init(AttackResultData resultData, HUD_TextStyle textStyle, float animeDurationMulti, float flyDistMulti, Action OnRecycle)
        {
            //向上取整 不显示小数点
            textNum.text = Mathf.Ceil(resultData.damage).ToString("F0");

            //暴击
            if (resultData.isCritical)
            {
                objCrit.SetActive(true);
                textNum.fontSizeMax = 0.6f;
                textNum.color = HUD.Instance.GetCriticDamageColor();
            }
            else
            {
                objCrit.SetActive(false);
                textNum.fontSizeMax = 0.25f;
                textNum.color = HUD.Instance.GetDamageColor(resultData.damageType);
            }

            //随机方向随机位置
            //Vector2 randomDirection = Random.insideUnitCircle * radius;
            //Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f);

            //还需要优化
            // 计算被击打方向（从子弹到目标）
            Vector2 hitDirection = Random.insideUnitCircle.normalized;
            // 基础偏移距离（力度）
            float baseOffset = textStyle.textDistance*flyDistMulti;
            // 添加随机性
            Vector2 randomOffset = new Vector2(
                Random.Range(-0.3f, 0.3f),
                Random.Range(0.5f, 0.8f) // 稍微向上偏移，更自然
            );
            // 最终位置 = 当前位置 + 反方向偏移 + 随机偏移
            Vector3 finalPosition = transform.position
                + new Vector3(-hitDirection.x, -hitDirection.y, 0f) * baseOffset
                + new Vector3(randomOffset.x, randomOffset.y, 0f);

            transform.localScale = Vector2.zero;
            //跳字动画
            transform.DOScale(Vector2.one * 1.5f, textStyle.textBounceDuration * animeDurationMulti).SetEase(Ease.OutBack).OnComplete(() => transform.DOScale(Vector3.zero * 0.5f, 0.7f * animeDurationMulti).SetEase(Ease.InQuad));
            transform.DOMove(finalPosition, textStyle.textFlyDuration * animeDurationMulti).SetEase(Ease.OutSine);
            textNum.DOFade(0, textStyle.textFadeDuration * animeDurationMulti).SetDelay(textStyle.textFadeDelay * animeDurationMulti).OnComplete(() =>
            {
                OnRecycle?.Invoke();
            });
        }
        public void Init(AttackResultData resultData, Action OnRecycle)
            => Init(resultData, HUD_TextStyle.Default_Style, 1, 1, OnRecycle);

        void OnDestroy()
        {
            textNum.DOKill();
            transform.DOKill();
        }
    }
}
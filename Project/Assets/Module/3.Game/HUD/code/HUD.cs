using UnityEngine;
using ObjectPool;

namespace HUD_TEXT
{
    public class HUD : Singleton<HUD>
    {
        [Header("Text Style Group")]
        [SerializeField] private Transform lowPriorityTransform;
        [SerializeField] private Transform defaultParent;
        [SerializeField] private Transform highPriorityParent;
        [Header("Text Style Control")]
        [SerializeField] private Vector2Int textAmountToMaxSpeedMulti = new Vector2Int(100, 300);
        [Range(0, 1)]
        [SerializeField] private float textAnimationDurationMulti = 0.5f;
        [SerializeField] private float textDistanceMulti = 0.5f;
        [SerializeField] private HUDTextStyleData textStyleSO;
        [Header("Prefab")]
        [SerializeField] private GameObject TextDamage;

        private HUD_TextStyle damageNumStyle;
        private const string TEXT_DAMAGE = "hud_text_damage";

        protected override void Awake()
        {
            base.Awake();
            damageNumStyle = textStyleSO.GetCurrentStyle();
        }
        public void OnHudDamage(Vector2 pos, AttackResultData resultData)
        {
            if (resultData.damage <= 0) return;

            //根据伤害类型，去修改跳字的优先级
            Transform parent = lowPriorityTransform;
            if (resultData.isCritical)
                parent = highPriorityParent;
            else if (resultData.damageType != ElementType.Physical)
                parent = defaultParent;

            float ratio = Mathf.InverseLerp(textAmountToMaxSpeedMulti.x, textAmountToMaxSpeedMulti.y, PoolManager.Instance.PoolActiveSize(TEXT_DAMAGE));
            float durationRatio = Mathf.Lerp(1, textAnimationDurationMulti, ratio);
            float distRatio = Mathf.Lerp(1, textDistanceMulti, ratio);

            GameObject textObj = PoolManager.Instance.GetObject(TEXT_DAMAGE, TextDamage, parent);
            textObj.transform.position = pos;
            textObj.GetComponent<HudTextDamage>().Init(resultData, damageNumStyle, durationRatio, distRatio,
                () => RecycleHUD(TEXT_DAMAGE, textObj));
        }
        public Color GetCriticDamageColor() { return Color.red; }
        public Color GetDamageColor(ElementType elementType) { return Color.red; }
        static void RecycleHUD(string poolName, GameObject go)
        {
            PoolManager.Instance.Release(poolName, go);
        }
    }
}
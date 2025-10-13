using UnityEngine;

namespace RTSDemo.Basement
{
    public class BasementShieldView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer shieldRender;//完美盾
        [SerializeField] private Animator shieldAnimator;

        [Header("VFX")]
        [SerializeField] private Transform vfxRoot;
        [SerializeField] private SpriteRenderer shieldBlur;
        [SerializeField] private SpriteRenderer shieldMask;
        [SerializeField] private ParticleSystem critic_electric;
        [SerializeField] private ParticleSystem critic_split;
        [SerializeField] private ParticleSystem critic_electric_loop;
        [SerializeField] private ParticleSystem defeat_electric;
        [SerializeField] private ParticleSystem defeat_small_explode;

        private const string HEALTH_FLOAT_KEY = "health";
        private const string ACTIVATE_BOOL_KEY = "activate";

        public void Init(int row, int col)
        {
            OnRefreshShieldSize(row, col);
            OnCloseShield();
            OnChangeBaseShieldView(1);
        }

        public void OnRefreshShieldSize(int row, int col)
        {
            // BattleGridArgs firstUnlockedGridArgs = BattleGridControl.Instance.GetFirstUnlockedGridArgs();
            // Vector3 originPoint = new Vector3(firstUnlockedGridArgs.x - 3, firstUnlockedGridArgs.y - 3, 0);

            // transform.localPosition = originPoint;
            // // 设置SpriteRenderer的size
            // if (shieldRender != null)
            // {
            //     // 通过修改sprite的pixelsPerUnit来间接控制size
            //     shieldRender.drawMode = SpriteDrawMode.Sliced;
            //     shieldRender.size = new Vector2(row, col);  // 根据行列设置大小
            //     shieldBlur.size = new Vector2(row, col);
            //     shieldMask.size = new Vector2(row, col);
            // }

            // Vector3 center = originPoint + new Vector3(row, col, 0) * BaseControl.CELL_SIZE;

            // vfxRoot.position = center;
            // var shapeModule = critic_electric.shape;
            // shapeModule.scale = new Vector3(row, col, 1);
            // shapeModule = critic_split.shape;
            // shapeModule.scale = new Vector3(row, col, 1);
            // shapeModule = defeat_small_explode.shape;
            // shapeModule.scale = new Vector3(row, col, 1);
            // shapeModule = defeat_electric.shape;
            // shapeModule.scale = new Vector3(row, col, 1);
            // shapeModule = critic_electric_loop.shape;
            // shapeModule.scale = new Vector3(row, col, 1);
        }

        public void OnOpenShield()
        {
            shieldAnimator.SetBool(ACTIVATE_BOOL_KEY, true);
        }

        public void OnCloseShield()
        {
            shieldAnimator.SetBool(ACTIVATE_BOOL_KEY, false);
        }

        public void OnChangeBaseShieldView(float healthRatio)
        {
            shieldAnimator.SetFloat(HEALTH_FLOAT_KEY, healthRatio);
        }
    }
}
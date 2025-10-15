using UnityEngine;
using DG.Tweening;

namespace RTSDemo.Basement
{
    public enum BaseWallDirection
    {
        Top,
        Bottom,
        Left,
        Right,
        CornerTopLeft,
        CornerTopRight,
        CornerBottomLeft,
        CornerBottomRight
    }
    public class BasementWall : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer render;
        [SerializeField] private SpriteRenderer decal;

        //是否新创建，是否要播放动画的标签
        private bool isNewCreated;
        private float offsetX = 0.3f;
        private float offsetCorner = 1;
        private BaseWallDirection direction;

        //后续拓展状态,如快破损时候，切换显示
        public async void Init(int x, int y, string spriteName, string decalSpriteName, Vector2 pos, BaseWallDirection direction, bool isPreview = false)
        {
            this.direction = direction;
            name = $"{spriteName}_{x}_{y}";

            //创建时候隐藏，后续动画来控制显示
            render.gameObject.SetActive(false);
            transform.position = pos;

            SetOffsetAndDirection();

            isNewCreated = true;
            render.sprite = await GameAsset.GetSpriteAsync(spriteName);
            decal.sprite = await GameAsset.GetSpriteAsync(decalSpriteName);

            if (isPreview)
            {
                render.color = new Vector4(0.7f, 0.7f, 0.7f, 1);
            }
        }
        public void OnShowAnimation()
        {
            if (!isNewCreated) return;

            isNewCreated = false;

            render.DOKill();
            render.transform.DOKill();

            render.color = new Vector4(1, 1, 1, 0);
            render.transform.localScale = Vector2.zero;
            render.gameObject.SetActive(true);
            render.transform.DOScale(1, 0.2f).SetEase(Ease.OutSine);
            render.DOFade(1, 0.3f).SetEase(Ease.OutSine);
        }
        public void OnRefreView(){}
        public void OnKill()=>Destroy(gameObject);
        void SetOffsetAndDirection()
        {
            switch (direction)
            {
                case BaseWallDirection.Top:
                    render.transform.position += new Vector3(0, -offsetX, 0);
                    render.sortingOrder = 0;

                    decal.transform.position = render.transform.position + new Vector3(0, 0.1f, 0);
                    break;
                case BaseWallDirection.Bottom:
                    render.transform.position += new Vector3(0, offsetX, 0);
                    render.transform.eulerAngles = new Vector3(0, 0, 180);
                    render.sortingOrder = 0;

                    decal.transform.position = render.transform.position + new Vector3(0, -0.1f, 0);
                    decal.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case BaseWallDirection.Left:
                    render.transform.position += new Vector3(offsetX, 0, 0);
                    render.transform.eulerAngles = new Vector3(0, 0, 90);
                    render.sortingOrder = 0;

                    decal.transform.position = render.transform.position + new Vector3(-0.1f, 0, 0);
                    decal.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case BaseWallDirection.Right:
                    render.transform.position += new Vector3(-offsetX, 0, 0);
                    render.transform.eulerAngles = new Vector3(0, 0, -90);
                    render.sortingOrder = 0;

                    decal.transform.position = render.transform.position + new Vector3(0.1f, 0, 0);
                    decal.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case BaseWallDirection.CornerTopLeft:
                    render.transform.position += new Vector3(offsetCorner, -offsetCorner, 0);
                    render.sortingOrder = 1;

                    decal.transform.position = render.transform.position + new Vector3(-0.15f, 0.15f, 0);
                    break;
                case BaseWallDirection.CornerTopRight:
                    render.transform.position += new Vector3(-offsetCorner, -offsetCorner, 0);
                    render.transform.eulerAngles = new Vector3(0, 0, -90);
                    render.sortingOrder = 1;

                    decal.transform.position = render.transform.position + new Vector3(0.15f, 0.15f, 0);
                    decal.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case BaseWallDirection.CornerBottomLeft:
                    render.transform.position += new Vector3(offsetCorner, offsetCorner, 0);
                    render.transform.eulerAngles = new Vector3(0, 0, 90);
                    render.sortingOrder = 1;

                    decal.transform.position = render.transform.position + new Vector3(-0.15f, -0.15f, 0);
                    decal.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case BaseWallDirection.CornerBottomRight:
                    render.transform.position += new Vector3(-offsetCorner, offsetCorner, 0);
                    render.transform.eulerAngles = new Vector3(0, 0, 180);
                    render.sortingOrder = 1;

                    decal.transform.position = render.transform.position + new Vector3(0.15f, -0.15f, 0);
                    decal.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
            }
        }
    }
}

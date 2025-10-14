using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Collections;
using DG.Tweening;

namespace CameraUtility
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineCamera defaultCam;
        [SerializeField] private CinemachineCamera battleCam;
        [SerializeField] private CinemachineCamera preparationCam;

        private CinemachineCamera activeCam;
        private CinemachineBrain camBrain;
        private Camera mainCamera;

        private Camera renderCam;
        private RenderTexture screenTex;

        private const string GlobalSceneTexName = "GlobalScreenTex";

        private Vector3 originalMergeCamOffset;

        CoroutineExcuter camTransitioner;

        private void Start()
        {
            camTransitioner = new CoroutineExcuter(this);

            defaultCam.gameObject.SetActive(true);
            battleCam.gameObject.SetActive(false);
            preparationCam.gameObject.SetActive(false);

            activeCam = defaultCam;

            mainCamera = Camera.main;
            camBrain = mainCamera.GetComponent<CinemachineBrain>();

            originalMergeCamOffset = preparationCam.transform.position;
        }

        public Vector2 WorldToScreenPos(Vector2 worldPos) => mainCamera.WorldToScreenPoint(worldPos);
        public Vector2 ScreenToWorldPos(Vector2 scrPos) => mainCamera.ScreenToWorldPoint(scrPos);

        #region BattleState
        //开始关卡时，先进入merge cam
        public void OnBattleEnter()
        {
            //每次开始时，重置BattleCamera
            TransitionToCamera(preparationCam);
        }
        //开始关卡时，先进入merge cam
        public void OnFightStart()
        {
            //每次开始时，重置BattleCamera
            TransitionToCamera(battleCam);
        }
        //结束战斗时，进入default camera
        public void OnBattleEnd()
        {
            //每次结束后，重置BattleCamera
            preparationCam.transform.position = originalMergeCamOffset;
            TransitionToCamera(defaultCam);
        }
        //进入合成界面时，过渡到merge cam，并执行一次完成callback
        public void OnPrepareStart(Action action = null)
        {
            //Transfer to Merge Cam
            TransitionToCamera(preparationCam);
            camTransitioner.Excute(coroutineMergeCamTransition(camBrain.DefaultBlend.Time, action));
        }
        #endregion

        #region RenderTexture Camera
        //获取渲染用摄像机
        public Camera GetRenderCam(int width, int height, bool matchMainCam = true)
        {
            if (renderCam == null)
            {
                //创建Render Texture
                var desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 1);
                screenTex = RenderTexture.GetTemporary(desc);
                //创建Render Camera
                renderCam = new GameObject("rt_camera").AddComponent<Camera>();
                renderCam.CopyFrom(mainCamera);
                //移除对UI层的渲染:https://docs.unity3d.com/6000.0/Documentation/Manual/layermask-remove.html
                renderCam.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
                renderCam.targetTexture = screenTex;
                //实时刷新Camera设置以对齐当前main camera
                if (matchMainCam)
                {
                    renderCam.gameObject.AddComponent<CameraMatching>().Init(mainCamera);
                }
                //设置全局贴图
                Shader.SetGlobalTexture(GlobalSceneTexName, screenTex);
            }
            return renderCam;
        }
        //移除渲染用摄像机
        public void CleanUpRenderCam()
        {
            Destroy(renderCam.gameObject);
            Shader.SetGlobalTexture(GlobalSceneTexName, null);
            RenderTexture.ReleaseTemporary(screenTex);
        }
        #endregion

        #region Camera Positioning
        public void AlignCamToPos(LevelCameraPos cameraPos)
        {
            switch (cameraPos)
            {
                case LevelCameraPos.Top:
                    AlignCamToPos(battleCam, new Vector3(0, 5));
                    break;
                case LevelCameraPos.Middle:
                    AlignCamToPos(battleCam, new Vector3(0, 0.5f));
                    break;
                case LevelCameraPos.Bottom:
                    AlignCamToPos(battleCam, new Vector3(0, -5));
                    break;
            }
            //Transfer to Battle Cam
            TransitionToCamera(battleCam);
        }
        void AlignCamToPos(CinemachineCamera targetCam, Vector2 worldPos)
        {
            Vector3 pos = worldPos;
            pos.z = targetCam.transform.position.z;
            targetCam.transform.position = pos;
        }
        public bool IsPointOnScreen(Vector2 worldPos)
        {
            Vector3 screenPos = WorldToScreenPos(worldPos);
            return screenPos.z >= 0 &&
                screenPos.x > 0 && screenPos.x < Screen.width &&
                screenPos.y > 200 && screenPos.y < Screen.height - 220;//战斗内气泡指示器防止被上下方信息栏遮挡
        }
        //执行一次camera shake，duration为时长，amp为强度，需要cinemachine camera有对应的组件才行
        public void ShakeScreen(float duration, float amp)
        {
            CinemachineBasicMultiChannelPerlin noiseControl = activeCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
            if (noiseControl == null) return;
            StartCoroutine(coroutineShake(noiseControl, duration, amp));
        }
        public void ResizeBattleCamera(float targetSize, float duration = 0.5f)
        {
            DOTween.Kill(battleCam);
            DOTween.To(() => battleCam.Lens.OrthographicSize, x => battleCam.Lens.OrthographicSize = x, targetSize, duration).SetEase(Ease.InOutQuad);
        }
        #endregion

        //过渡到新的camera
        void TransitionToCamera(CinemachineCamera targetVC, float duration = 0.5f)
        {
            camBrain.DefaultBlend.Time = duration;

            activeCam.gameObject.SetActive(false);
            activeCam = targetVC;
            targetVC.gameObject.SetActive(true);
        }
        IEnumerator coroutineShake(CinemachineBasicMultiChannelPerlin noiseControl, float duration, float amp)
        {
            noiseControl.AmplitudeGain = amp;
            yield return new WaitForLoop(duration, (t) =>
                noiseControl.AmplitudeGain = Mathf.Lerp(amp, 0, EasingFunc.Easing.SmoothInOut(t))
            );
        }
        IEnumerator coroutineMergeCamTransition(float transition, Action callback)
        {
            yield return new WaitForSeconds(transition);
            callback?.Invoke();
        }
        //平缓移动摄像头
        public void TweenActiveCam(Vector3 position, float duration) => activeCam.transform.DOMove(position, duration).SetEase(Ease.InOutQuad);
    }
}

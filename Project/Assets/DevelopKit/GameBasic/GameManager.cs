using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace GameBasic
{
    using Event;
    using RTSDemo.Game;

    //Please make sure "GameManager" is excuted before every custom script
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private int targetFrameRate = 60;
        [Header("Scene Transition")]
        [SerializeField] private CanvasGroup BlackScreenCanvasGroup;
        [SerializeField] private float transitionDuration = 1;

        [Header("Init")]
        [SerializeField] private string InitScene;
        [SerializeField] private Initiator initiator;

        [Header("Debug")]
        [SerializeField] private bool EnableDebugOption = true;
        [SerializeField] private bool loadInitSceneFromGameManager = false;
        [SerializeField] private GameLaunchSettings launchSettings;
        [SerializeField] private GameLaunchSettings debugSettings;
        [SerializeField] private InputActionMap debugActions;

        private static bool isPaused = false;

        public bool IsSwitchingScene { get; private set; } = false;
        public string lastScene { get; private set; } = string.Empty;
        public string currentScene { get; private set; } = string.Empty;

        protected override async void Awake()
        {
            base.Awake();
            Application.targetFrameRate = targetFrameRate;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            debugActions["restart"].performed += Debug_RestartLevel;

            if (EnableDebugOption) debugActions.Enable();
#endif
            if(initiator!=null)
                await initiator.Init();

#if UNITY_EDITOR
                //Load Level
                if (loadInitSceneFromGameManager)
                {
                    BlackScreenCanvasGroup.alpha = 1;
                    SwitchingScene(string.Empty, InitScene);
                }
                else
                {
                    LaunchSetting(debugSettings);
                    currentScene = SceneManager.GetActiveScene().name;
                }
#else
            //Since we don't have the saving system yet, the initiation should be done by loading the debug progress data.
            LaunchSetting(launchSettings);
            SwitchingScene(string.Empty, InitScene);
#endif
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            debugActions["restart"].performed -= Debug_RestartLevel;

            if (debugActions.enabled) debugActions.Disable();
#endif
        }

        #region Game Basic
        public void PauseTheGame()
        {
            if (isPaused) return;

            Time.timeScale = 0;
            AudioListener.pause = true;
            isPaused = true;
        }
        public void ResumeTheGame()
        {
            if (!isPaused) return;

            AudioListener.pause = false;
            Time.timeScale = 1;
            isPaused = false;
        }
        public void EndGame()
        {
            string currentLevel = SceneManager.GetActiveScene().name;
            StartCoroutine(EndGameCoroutine(currentLevel));
        }
        public void RestartLevel()
        {
            string currentLevel = SceneManager.GetActiveScene().name;
            StartCoroutine(RestartLevel(currentLevel));
        }
        #endregion

        #region Scene Transition
        public void SwitchingScene(string to)
        {
            string from = SceneManager.GetActiveScene().name;
            SwitchingScene(from, to);
        }
        void SwitchingScene(string from, string to)
        {
            if (!IsSwitchingScene) StartCoroutine(SwitchSceneCoroutine(from, to));
        }
        IEnumerator EndGameCoroutine(string level)
        {
            StartCoroutine(FadeInBlackScreen(1f));

            yield return new WaitForSeconds(1f);
            EventHandler.Call_BeforeUnloadScene();
            yield return SceneManager.UnloadSceneAsync(level);
            yield return new WaitForSeconds(1f);
            Application.Quit();
        }
        IEnumerator RestartLevel(string level)
        {
            yield return FadeInBlackScreen(3f);
            IsSwitchingScene = true;
            //TO DO: do something before the last scene is unloaded. e.g: call event of saving 
            EventHandler.Call_BeforeUnloadScene();

            yield return SceneManager.UnloadSceneAsync(level);
            yield return null;
            //TO DO: do something after the last scene is unloaded.
            yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));
            //TO DO: do something after the next scene is loaded. e.g: call event of loading
            yield return null;
            yield return FadeOutBlackScreen(transitionDuration);
            EventHandler.Call_AfterLoadScene();

            IsSwitchingScene = false;
        }
        IEnumerator SwitchSceneCoroutine(string from, string to)
        {
            IsSwitchingScene = true;
            if (from != string.Empty)
            {
                //TO DO: do something before the last scene is unloaded. e.g: call event of saving 
                lastScene = from;

                EventHandler.Call_BeforeUnloadScene();
                yield return FadeInBlackScreen(transitionDuration);
                yield return SceneManager.UnloadSceneAsync(from);
            }
            //TO DO: do something after the last scene is unloaded.
            yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(to));
            currentScene = to;

            //TO DO: do something after the next scene is loaded. e.g: call event of loading
            EventHandler.Call_AfterLoadScene();

            yield return null;
            yield return FadeOutBlackScreen(transitionDuration);

            IsSwitchingScene = false;
        }
        public IEnumerator FadeInBlackScreen(float fadeDuration)
        {
            float initAlpha = BlackScreenCanvasGroup.alpha;
            yield return new WaitForLoop(fadeDuration, (t) =>
            {
                BlackScreenCanvasGroup.alpha = Mathf.Lerp(initAlpha, 1, EasingFunc.Easing.QuadEaseOut(t));
            });
        }
        public IEnumerator FadeOutBlackScreen(float fadeDuration)
        {
            float initAlpha = BlackScreenCanvasGroup.alpha;
            yield return new WaitForLoop(fadeDuration, (t) =>
            {
                BlackScreenCanvasGroup.alpha = Mathf.Lerp(initAlpha, 0, EasingFunc.Easing.QuadEaseIn(t));
            });
        }
        #endregion

        #region Launching
        void LaunchSetting(GameLaunchSettings gameLaunchSettings) { }
        #endregion

        #region DEBUG ACTION
        void Debug_EndGame(InputAction.CallbackContext callback) => EndGame();
        void Debug_RestartLevel(InputAction.CallbackContext callback)
        {
            if (callback.ReadValueAsButton())
            {
                Debug.Log("Test Restart Level");
                RestartLevel();
            }
        }
        #endregion
    }
}
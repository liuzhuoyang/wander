using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using RTSDemo.Unit;
using BattleGear;
using BattleLaunch.Bullet;
using SimpleVFXSystem;
using RTSDemo.Building;

namespace RTSDemo.Game
{
    //以下Manager，都需要执行资源加载的异步初始化，因此需要通过Initiator进行
    public class RTSInitiator : MonoBehaviour
    {
        [Header("Init")]
        [SerializeField] private string InitScene;
        [SerializeField] private GameInitConfig gameConfig;
        
        public async void Start()
        {
            await gameConfig.GamePlaySetUp();
            await SceneManager.LoadSceneAsync(InitScene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(InitScene));
        }
    }
}

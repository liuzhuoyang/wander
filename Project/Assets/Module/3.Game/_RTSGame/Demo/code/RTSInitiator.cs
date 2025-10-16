using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

using RTSDemo.Building;

namespace RTSDemo.Game
{
    //以下Manager，都需要执行资源加载的异步初始化，因此需要通过Initiator进行
    public class RTSInitiator : MonoBehaviour
    {
        [Header("Init")]
        [SerializeField] private string InitScene;
        [SerializeField] private GameInitConfig gameConfig;
        [Header("Extra")]
        [SerializeField] private GameObject building;

        private GameAssetManagerGeneric gameAssetManagerGeneric; 
        
        public async void Start()
        {
            GameObject root = new GameObject("[GameplayManager]");
            await gameConfig.GamePlaySetUp(root.transform);
            await Instantiate(building).GetComponent<BuildingManager>().Init();
            await SceneManager.LoadSceneAsync(InitScene, LoadSceneMode.Additive);
            gameAssetManagerGeneric = new GameObject("GameAssetManagerGeneric").AddComponent<GameAssetManagerGeneric>();
            await gameAssetManagerGeneric.Init();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(InitScene));
        }
    }
}

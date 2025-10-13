using System.Collections.Generic;
using BattleGear;
using BattleLaunch.Bullet;
using Cysharp.Threading.Tasks;
using RTSDemo.Building;
using RTSDemo.Unit;
using SimpleVFXSystem;
using UnityEngine;

namespace RTSDemo.Game
{
    [CreateAssetMenu(fileName = "GameInitConfig", menuName = "RTS_Demo/Game/GameInitConfig")]
    public class GameInitConfig : ScriptableObject
    {
        [Header("Async Loading")]
        public GameObject unitManagerPrefab;
        public GameObject buildingManagerPrefab;
        public GameObject bulletManagerPrefab;
        public GameObject gearManagerPrefab;
        public GameObject vfxManagerPrefab;

        [Header("Direct Config")]
        public GameObject BuffManagerPrefab;
        public GameObject PlayerInputManager;
        public GameObject PoolManager;
        public GameObject AudioManager;

        public async UniTask GamePlaySetUp()
        {
            //直接实例化不需要异步加载的模组
            Instantiate(BuffManagerPrefab);
            Instantiate(PlayerInputManager);
            Instantiate(PoolManager);
            Instantiate(AudioManager);
            //初始化游戏必要模组
            //@todo应该有更好的方式来处理模组的初始化
            var building = Instantiate(buildingManagerPrefab).GetComponent<BuildingManager>();
            var unit = Instantiate(unitManagerPrefab).GetComponent<UnitManager>();
            var bullet = Instantiate(bulletManagerPrefab).GetComponent<BulletManager>();
            var gear = Instantiate(gearManagerPrefab).GetComponent<GearManager>();
            var vfx = Instantiate(vfxManagerPrefab).GetComponent<VFXManager>();

            List<UniTask> loadingTask = new List<UniTask>() { building.Init(), unit.Init(), bullet.Init(), gear.Init(), vfx.Init()};
            await UniTask.WhenAll(loadingTask);
        }
    }
}
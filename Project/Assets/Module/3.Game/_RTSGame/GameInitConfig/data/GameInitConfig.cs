using System.Collections.Generic;
using BattleGear;
using BattleLaunch.Bullet;
using Cysharp.Threading.Tasks;
using RTSDemo.Unit;
using SimpleVFXSystem;
using UnityEngine;

namespace RTSDemo.Game
{
    [CreateAssetMenu(fileName = "GameInitConfig", menuName = "RTS_Demo/Game/GameInitConfig")]
    public class GameInitConfig : ScriptableObject
    {
        [Header("异步加载模组")]
        public GameObject unitManagerPrefab;
        public GameObject bulletManagerPrefab;
        public GameObject gearManagerPrefab;
        public GameObject vfxManagerPrefab;

        [Header("直接加载模组")]
        public GameObject BuffManagerPrefab;
        public GameObject PlayerInputManager;
        public GameObject PoolManager;
        public GameObject AudioManager;
        public GameObject BattleFormatianManager;

        //加载战斗所需的必要模组，若有其他模组，可以考虑添加在这里
        public async UniTask GamePlaySetUp(Transform root)
        {
            //直接实例化不需要异步加载的模组
            Instantiate(BuffManagerPrefab, root);
            Instantiate(PlayerInputManager, root);
            Instantiate(PoolManager, root);
            Instantiate(AudioManager, root);
            Instantiate(BattleFormatianManager, root);
            //初始化游戏必要模组
            var unit = Instantiate(unitManagerPrefab, root).GetComponent<UnitManager>();
            var bullet = Instantiate(bulletManagerPrefab, root).GetComponent<BulletManager>();
            var gear = Instantiate(gearManagerPrefab, root).GetComponent<GearManager>();
            var vfx = Instantiate(vfxManagerPrefab, root).GetComponent<VFXManager>();
            

            List<UniTask> loadingTask = new List<UniTask>() { unit.Init(), bullet.Init(), gear.Init(), vfx.Init()};
            await UniTask.WhenAll(loadingTask);
        }
    }
}
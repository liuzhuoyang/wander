using System.Collections.Generic;
using BattleActor.Building;
using BattleActor.Unit;
using BattleGear;
using BattleLaunch.Bullet;
using Cysharp.Threading.Tasks;
using SimpleVFXSystem;
using UnityEngine;

namespace RTSDemo.Game
{
    //以下Manager，都需要执行资源加载的异步初始化，因此需要通过Initiator进行
    public class RTSInitiator : Initiator
    {
        [Header("Init")]
        [SerializeField] private string InitScene;
        [SerializeField] private GameObject unitManager;
        [SerializeField] private GameObject buildingManager;
        [SerializeField] private GameObject bulletManager;
        [SerializeField] private GameObject gearManager;
        [SerializeField] private GameObject vfxManager;

        public override async UniTask Init()
        {
            //初始化游戏必要模组
            //@todo应该有更好的方式来处理模组的初始化
            var building = Instantiate(buildingManager).GetComponent<BuildingManager>();
            var unit = Instantiate(unitManager).GetComponent<UnitManager>();
            var bullet = Instantiate(bulletManager).GetComponent<BulletManager>();
            var gear = Instantiate(gearManager).GetComponent<GearManager>();
            var vfx = Instantiate(vfxManager).GetComponent<VFXManager>();

            List<UniTask> loadingTask = new List<UniTask>() { building.Init(), unit.Init(), bullet.Init(), gear.Init(), vfx.Init()};
            await UniTask.WhenAll(loadingTask);
        }
    }
}

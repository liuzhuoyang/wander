using UnityEngine;

using BattleActor;
using BattleGear;
using BattleLaunch.Bullet;
using BattleSummon;
using RTSDemo.Unit;
using RTSDemo.Zone;
using RTSDemo.Building;
using SimpleAudioSystem;

namespace RTSDemo.Game
{
    public class DemoBattleController : MonoBehaviour
    {
        [SerializeField] private AudioRefData bgmData;
        void Awake()
        {
            new GameObject("Battle Behaviour Manager").AddComponent<BattleBehaviourManager>();
            new GameObject("BattleActor Scan System").AddComponent<BattleActorScanSystem>();
            new GameObject("Battle Summon Manager").AddComponent<BattleSummonManage>();

            UnitManager.Instance.StartBattle();
            BulletManager.Instance.StartBattle();
            BuildingManager.Instance.StartBattle();
            GearManager.Instance.StartBattle();
            BuffZoneManager.Instance.StartBattle();
        }
        void Start()
        {
            AudioManager.Instance.PlayBGM(bgmData.name);
        }
    }
}

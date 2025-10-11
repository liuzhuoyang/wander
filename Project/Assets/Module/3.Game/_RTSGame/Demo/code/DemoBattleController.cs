using BattleActor;
using BattleActor.Building;
using BattleActor.Unit;
using BattleGear;
using BattleLaunch.Bullet;
using BattleSummon;
using RTSDemo.Zone;
using SimpleAudioSystem;
using UnityEngine;

public class DemoBattleController : MonoBehaviour
{
    [SerializeField] private AudioRefData bgmData;
    void Awake()
    {
        new GameObject("Battle Behaviour Manager").AddComponent<BattleBehaviourManager>().Init();
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

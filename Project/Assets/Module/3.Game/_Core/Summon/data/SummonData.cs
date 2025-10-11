using BattleActor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleSummon
{
    [CreateAssetMenu(fileName = "SummonData", menuName = "Assets/Summon/SummonData")]
    public class SummonData : ScriptableObject
    {
        [BoxGroup("基本召唤参数")] public BattleActorData actorData;
        [BoxGroup("基本召唤参数")] public int summonLevelAdjustment; //等级修改值，生产单位的等级 = 传入等级+等级修改值
        [BoxGroup("基本召唤参数")] public int summonLimit; //已生产且存活的单位数量大于该数量时，暂停生产
    }
}
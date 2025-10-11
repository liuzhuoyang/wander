using System.Collections.Generic;
using BattleBuff;
using BattleLaunch;
using UnityEngine;

namespace BattleActor.Basement.Skill
{
    public class BasementLaunchSkill : BasementSkill
    {
        protected Transform launchTrans;
        protected AttackData attackData;
        protected BattleLaunchControl battleLaunchControl;
        protected BattleLaunchCommandData battleLaunchCommandData;

        public BasementLaunchSkill(LaunchConfig launchConfig, AttackData attackData, int launchLayerIndex,
        string buffTypeID, string[] buffIDs, float areaRadius, TeamMask areaTeamMask)
            : base(buffTypeID, buffIDs, areaRadius, areaTeamMask)
        {
            this.attackData = attackData;
            battleLaunchCommandData = new BattleLaunchCommandData(launchConfig);
        }

        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            battleLaunchControl = host.GetComponent<BattleLaunchControl>();
            if (battleLaunchControl == null)
            {
                battleLaunchControl = host.gameObject.AddComponent<BattleLaunchControl>();
            }
        }
        public override void UpdateBuff()
        {
            base.UpdateBuff();
            battleLaunchControl.UpdateLaunching();
        }
        public override void ExcuteBasementAbilityOnNearPos(Vector2 targetPos)
        {
            BattleLaunchCommand_Batch launchCommand = new BattleLaunchCommand_Batch(battleLaunchCommandData, attackData, host.transform);
            launchCommand.ExcludeTeam(BattleActorService.GetOppositeTeam(areaTeamMask));
            launchCommand.AssignTargets(new List<Vector2> { targetPos });
            battleLaunchControl.AddLaunch(launchCommand);
        }
    }
}
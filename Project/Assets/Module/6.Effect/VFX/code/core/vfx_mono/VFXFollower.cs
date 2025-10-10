using UnityEngine;
using UnityEngine.Animations;

namespace SimpleVFXSystem
{
    //跟随目标移动，通过VFXManager赋予
    public class VFXFollower : VFXMono
    {
        private PositionConstraint positionConstraint;

        protected override void VFXBegin()
        {
            //创建或获取已有的Constraints
            positionConstraint = GetComponent<PositionConstraint>();
            if (positionConstraint == null) positionConstraint = gameObject.AddComponent<PositionConstraint>();

            positionConstraint.constraintActive = false;
            var source = new ConstraintSource()
            {
                weight = 1,
                sourceTransform = controlObjects[0].transform
            };

            if (positionConstraint.sourceCount == 0)
                positionConstraint.AddSource(source);
            else
                positionConstraint.SetSource(0, source);

            positionConstraint.translationOffset = Vector2.zero;
            positionConstraint.locked = true;
            positionConstraint.constraintActive = true;
        }
    }
}
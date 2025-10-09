using UnityEngine;

public class CircleMovement
{
    public float rotatingRadius;
    public float rotatingPhaseOffset;
    public bool rotatingClockWise = true;
    public Vector2 rotatingCenter;
    private float currentPhase;
    public float GetPhase()=>currentPhase;
    public Vector2 UpdatePointOnCircle(float lineSpeed)=>UpdatePointOnCircle(lineSpeed, rotatingRadius);
    public Vector2 UpdatePointOnCircle(float lineSpeed, float radius)
    {
        currentPhase += Time.deltaTime * lineSpeed/radius;
        return GeometryUtil.GetCirclingPointPos(rotatingCenter, 
                                                (rotatingClockWise?-1:1)*currentPhase + rotatingPhaseOffset, 
                                                radius);        
    }
    public void EnterCirclingWithGivenCenter(Vector2 pos, Vector2 center, float maxRadius, Vector2 vel, float phaseShift = 0.2f)
    {
        currentPhase = 0;
        rotatingCenter = center;
        Vector2 normal = pos - center;
        rotatingRadius = Mathf.Max(maxRadius, normal.magnitude);
        rotatingClockWise = Vector3.Cross(vel, normal).z>0;
        rotatingPhaseOffset = Mathf.Atan2(normal.y, normal.x)+(rotatingClockWise?-1:1)*phaseShift; //0.2为提前量，使目标点提前与当前位置
    }
    public void EnterCirclingWithGivenRadius(Vector2 pos, Vector2 velocity, Vector2 refPos, float radius, float phaseShift = 0.2f)
    {
        currentPhase = 0;
        rotatingClockWise = Vector3.Cross(velocity, pos-refPos).z>0;
        Vector2 normal = Vector3.Cross(velocity, rotatingClockWise?Vector3.forward:Vector3.back);
        rotatingRadius = radius;
        rotatingCenter = pos + normal*rotatingRadius;
        rotatingPhaseOffset = Mathf.PI+Mathf.Atan2(normal.y, normal.x)+(rotatingClockWise?-1:1)*phaseShift; //0.2为提前量，使目标点提前与当前位置
    }
}
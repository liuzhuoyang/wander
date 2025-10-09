using UnityEngine;

//几何工具，目前用来做一些轨迹判定
public static class GeometryUtil 
{
    #region Circle
    public static float DeformedRadius(float circleRadius, float eclipseValue, float rad){
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        if(eclipseValue>1){
            Debug.LogAssertion("Eclipse Value bigger than 1(hyperbola), return 0");
            return 0;
        }
        return circleRadius * Mathf.Sqrt(eclipseValue*cos*cos+sin*sin);
    }
    //获取大圆小圆内随机点
    public static Vector2 RandomPointInCircle(Vector2 center, float minR, float maxR)
    {
        Vector2 point = center + Random.insideUnitCircle.normalized * maxR; //取外圆边缘的随机点
        return point - point.normalized * Random.Range(0, maxR - minR); //随机向内收缩
    }
    //判定点是否在圆内，即两点距离小于半径
    public static bool IsPointInCircle(Vector2 point, Vector2 circleCenter, float radius)
    {
        return (point-circleCenter).sqrMagnitude <= radius*radius;
    }
    //根据位置，方向与半径，获取旋转中心
    public static Vector2 GetRotateCenterOnPoint(Vector2 point, Vector2 dir, float radius, bool clockWise = false)
    {
        Vector2 normal = Vector3.Cross(dir, Vector3.forward).normalized;
        return point + (clockWise ? 1 : -1) * normal * radius;
    }
    //按时间与线速度，获取圆周上特定弧度的点
    public static Vector2 GetCirclingPointPos(Vector2 center, float speed, float time, float phaseOffset, float radius, bool clockwise = true){
        return GetCirclingPointPos(center, (clockwise?-1:1)*time*speed/radius + phaseOffset, radius);
    }
    //获取圆周上特定弧度的点
    public static Vector2 GetCirclingPointPos(Vector2 center, float phase, float radius){
        Vector2 surroundPos = Vector2.zero;
        surroundPos.x = Mathf.Cos(phase)*radius;
        surroundPos.y = Mathf.Sin(phase)*radius;
        return center+surroundPos;
    }
#endregion

#region misc
    public static Vector2 SmoothVec(Vector2 vec){
        return vec*vec*(3*Vector2.one-2f*vec);
    }
    public static float GetWorldRND(Vector2 pos, float seed, float scale = 0.1f)
    {
        float rnd = Mathf.PerlinNoise(pos.x*scale+seed, pos.y*scale);
        return rnd*2-1;
    }
#endregion
}
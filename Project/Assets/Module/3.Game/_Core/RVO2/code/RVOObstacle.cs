
namespace SimpleRVO
{
    using Unity.Mathematics;
    using UnityEngine;

    internal class RVOObstacle : MonoBehaviour
    {
        private const float size = 0.5f;
        private int obstacleId;

        private void OnEnable()
        {
            var vertices = CalculateBoundingSprite();
            this.obstacleId = RVOSimManager.AddObstacle(vertices);
        }
        private void OnDisable()
        {
            if (!RVOSimManager.IsValid())
                return;
            RVOSimManager.RemoveObstacle(this.obstacleId);
            this.obstacleId = default;
        }
        float2[] CalculateBoundingSprite()
        {
            var center = transform.position;
            float2[] vertices = new float2[4];
            vertices[0] = new float2(center.x-size, center.y-size);
            vertices[1] = new float2(center.x-size, center.y+size);
            vertices[2] = new float2(center.x+size, center.y+size);
            vertices[3] = new float2(center.x+size, center.y-size);
            return vertices;
        }
    }
}

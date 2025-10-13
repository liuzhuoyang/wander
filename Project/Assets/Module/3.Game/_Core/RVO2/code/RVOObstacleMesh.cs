using UnityEngine;

namespace SimpleRVO
{
    internal class RVOObstacleComp : MonoBehaviour
    {
        private int obstacleId;

        private void OnEnable()
        {
            var meshFilter = this.GetComponent<MeshFilter>();
            var vertices = ObstacleHelper.CalculateBoundingPolygon(meshFilter);
            this.obstacleId = RVOSimManager.AddObstacle(vertices);
        }

        private void OnDisable()
        {
            if (!RVOSimManager.IsValid())
                return;
            RVOSimManager.RemoveObstacle(this.obstacleId);
            this.obstacleId = default;
        }
    }
}

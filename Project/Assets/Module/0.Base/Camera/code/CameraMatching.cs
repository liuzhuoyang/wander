using UnityEngine;

namespace CameraUtility
{
    //一个简单的工具，让camera能够对齐另一个camera的OrthScale
    [RequireComponent(typeof(Camera))]
    public class CameraMatching : MonoBehaviour
    {
        private Camera self;
        private Camera target;
        public void Init(Camera targetCam)
        {
            target = targetCam; 
        }
        void Awake()
        {
            self = GetComponent<Camera>();
        }
        void Update()
        {
            self.orthographicSize = target.orthographicSize;
        }
    }
}

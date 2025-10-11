using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RTSDemo.Game
{
    public abstract class Initiator : MonoBehaviour
    {
        public abstract UniTask Init();
    }
}

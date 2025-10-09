using UnityEngine;

namespace SimpleAudioSystem
{
    public class PlaySFXOnEnable : MonoBehaviour
    {
        //被打开时候播放的音效，比如页面打开
        public AudioData_SO audioAwake;

        public void OnEnable()
        {
            if (audioAwake!=null)
            {
                AudioManager.Instance.PlaySFX(audioAwake.name);
            }
        }
    }
}
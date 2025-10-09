using UnityEngine;

public class AudioHandlerBase : MonoBehaviour
{
    //被打开时候播放的音效，比如页面打开
    public string audioAwake;
    
    public void OnEnable()
    {
        if(!string.IsNullOrEmpty(audioAwake))
        {
            AudioControl.Instance.PlaySFX(audioAwake);
        }
    }
}

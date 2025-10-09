using UnityEngine;

public class AudioEndHandler : AudioHandlerBase
{
    public void OnSFXVictory()
    {
        AudioControl.Instance.PlaySFX("sfx_ui_end_victory");
    }
    
    public void OnSFXDefeat()
    {
        AudioControl.Instance.PlaySFX("sfx_ui_end_defeat");
    }
}

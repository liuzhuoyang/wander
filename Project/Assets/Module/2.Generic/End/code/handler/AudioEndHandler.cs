using SimpleAudioSystem;

public class AudioEndHandler : AudioHandlerBase
{
    public void OnSFXVictory()
    {
        AudioManager.Instance.PlaySFX("sfx_ui_end_victory");
    }
    
    public void OnSFXDefeat()
    {
        AudioManager.Instance.PlaySFX("sfx_ui_end_defeat");
    }
}

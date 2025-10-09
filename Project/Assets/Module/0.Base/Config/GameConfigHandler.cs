
public class GameConfigHandler : Singleton<GameConfigHandler>
{
    public GameConfigAsset gameConfig;

    protected override void Awake()
    {
        base.Awake();
        GameConfig.Init();
    }
}

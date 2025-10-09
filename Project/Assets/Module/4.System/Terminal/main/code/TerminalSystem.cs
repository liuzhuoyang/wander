public class TerminalSystem : Singleton<TerminalSystem>
{
    public void Init()
    {
    }

    public async void Open()
    {
        await UIMain.Instance.OpenUI("terminal", UIPageType.Overlay);
    }
}

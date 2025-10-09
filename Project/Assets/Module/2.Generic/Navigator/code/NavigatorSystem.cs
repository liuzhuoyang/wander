using UnityEngine;

public class NavigatorSystem : Singleton<NavigatorSystem>
{
    //导航到对象的功能
    public void OnNavigator(string navigatorName)
    {
        NavigatorData data = AllNavigator.dictData[navigatorName];
    }
}

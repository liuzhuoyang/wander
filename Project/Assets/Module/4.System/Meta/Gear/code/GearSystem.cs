using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GearSystem : Singleton<GearSystem>
{

    public async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("gear", "home");
        //添加功能事件，点击按钮后开启战斗功能
        FeatureUtility.OnAddFeature(FeatureType.Gear, () =>
        {
            Open();
        });
    }

    public void Open()
    {
        ModeHomeControl.OnOpen("gear");
        HeaderControl.OnShowMainHideHub(new List<string>() { "gem" });
        FooterControl.OnSelect("gear");

        Refresh();
    }
    
    public void Refresh()
    {
        EventManager.TriggerEvent<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, new UIGearArgs());
    }

}

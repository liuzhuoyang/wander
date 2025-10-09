using UnityEngine;
using TMPro;

public class UIEnergy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textEnergy;
    [SerializeField] GameObject objHubEnergyTimer;

    //刷新体力
    public void RefreshEnergy()
    {
        int energy = ItemSystem.Instance.GetItemNum(ConstantItem.ENERGY);
        int maxEnergy = EnergySystem.Instance.GetUserMaxEnergy();
        textEnergy.text = energy.ToString() + "/" + maxEnergy.ToString();
        
        //显示体力恢复时间
        //被打开后，上面挂的脚本UIEnergyTimerHandler会自动注册读秒事件
        objHubEnergyTimer.SetActive(energy < maxEnergy);
    }


    public void OnAddEnergy()
    {
        EnergySystem.Instance.OnPopupAddEnergy();
    }
}

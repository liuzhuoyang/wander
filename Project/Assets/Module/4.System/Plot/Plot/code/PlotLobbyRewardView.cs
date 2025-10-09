using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlotLobbyRewardView : MonoBehaviour
{
    [SerializeField] Image iconItem;
    [SerializeField] TextMeshProUGUI textNum;
    [SerializeField] GameObject objShard;


    public void Init(string rewardName, int rewardNum)
    {
        objShard.SetActive(false);
        if (rewardName.Contains("shard"))
        {
            objShard.SetActive(true);
        }

        string iconName = AllItem.dictData[rewardName].iconName;
        GameAssetControl.AssignSpriteUI(iconName, iconItem);
        textNum.text = "x " + rewardNum.ToString();
    }

}

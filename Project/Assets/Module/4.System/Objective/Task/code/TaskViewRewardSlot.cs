using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskViewRewardSlot : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] GameObject objOff,objPin;
    [SerializeField] TextMeshProUGUI textPoint,textNum;

    public void Init(TaskRewardItem rewardItem)
    {
        textPoint.text = rewardItem.pointNeeded.ToString();
        GameAssetControl.AssignIcon(rewardItem.reward, imgIcon);
        textNum.text = "x" + rewardItem.rewardNum;

        if (rewardItem.isClaimed)
        {
            objPin.SetActive(false);
            objOff.SetActive(true);
        }
        else
        {
            objOff.SetActive(false);
            objPin.SetActive(rewardItem.isClaimable);
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskViewSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textInfo, textBar, textRewardNum;
    [SerializeField] GameObject objBtnClaim, objTick;
    [SerializeField] SlicedFilledImage imgBar;
    [SerializeField] GameObject objPin;

    TaskViewArgs viewArgs;
    public void Init(TaskViewArgs viewArgs)
    {
        this.viewArgs = viewArgs;

        textInfo.text = UtilityLocalization.GetLocalization(viewArgs.displayName, viewArgs.targetNum.ToString());
        objBtnClaim.GetComponent<Image>().color = viewArgs.isClaimable ? Color.white : Color.gray;
        objPin.SetActive(viewArgs.isClaimable);
        objBtnClaim.SetActive(!viewArgs.isClaimed);

        objTick.SetActive(false);
        if (viewArgs.isClaimed)
        {
            objBtnClaim.GetComponent<Image>().color = Color.white;
            objTick.SetActive(true);

        }

        int doneNum = viewArgs.doneNum;
        if (doneNum >= viewArgs.targetNum)
            doneNum = viewArgs.targetNum;

        imgBar.fillAmount = (float)viewArgs.doneNum / (float)viewArgs.targetNum;
        textBar.text = doneNum + "/" + viewArgs.targetNum;

        textRewardNum.text = viewArgs.rewardPoint.ToString();
    }

    public void OnClaim()
    {
        TaskSystem.Instance.OnCompleteTask(viewArgs.taskName, viewArgs.rewardPoint, this.transform);
    }
}

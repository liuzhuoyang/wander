using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskViewReward : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPoint;
    [SerializeField] SlicedFilledImage imgBar;
    [SerializeField] List<TaskViewRewardSlot> listViewRewardSlot;
    [SerializeField] Image imgBtnClaim;
    [SerializeField] GameObject objPin;

    TaskType taskType;
    bool canClaim = false;
    public void Init(int point, int maxPoint, List<TaskRewardItem> listRewardArgs, TaskType taskType)
    {
        textPoint.text = point.ToString();
        imgBar.fillAmount = (float)point / (float)maxPoint;
        this.taskType = taskType;
        canClaim = false;
        for (int i = 0; i < listViewRewardSlot.Count; i++)
        {
            listViewRewardSlot[i].Init(listRewardArgs[i]);
            if(listRewardArgs[i].isClaimed)
                continue;
            if (listRewardArgs[i].isClaimable)
                canClaim = true;
        }
        imgBtnClaim.color = canClaim ? Color.white : Color.gray;
        objPin.SetActive(canClaim);
    }

    public void OnClaim()
    {
        if(!canClaim)
            return;
        TaskSystem.Instance.OnCompleteProgressReward(taskType);
    }
}

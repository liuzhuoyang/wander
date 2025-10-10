using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using SimpleAudioSystem;

public class UIEnd : UIBase
{
    public GameObject objWin;
    public GameObject objLose;

    public GameObject prefabItemViewSlot;
    public Transform groupItem;

    [SerializeField] GameObject objBtnClaim;
    [SerializeField] GameObject objBtnClaimAd;
    [SerializeField] GameObject objInfo;
    [SerializeField] GameObject objBtnBattleStats;
    bool isOpenInfo;

    private void Awake()
    {
        EventManager.StartListening<UIEndArgs>(EndEventName.EVENT_END_INIT_UI, OnInitUI);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<UIEndArgs>(EndEventName.EVENT_END_INIT_UI, OnInitUI);
    }

    async void OnInitUI(UIEndArgs args)
    {
        isOpenInfo = false;
        objInfo.SetActive(false);
        objWin.SetActive(args.isWin);
        objLose.SetActive(!args.isWin);

        objBtnClaimAd.SetActive(false);
        objBtnClaim.SetActive(false);

        if (args.isWin)
        {
            AudioManager.Instance.PlaySFX("sfx_ui_end_victory");
        }
        else
        {
            AudioManager.Instance.PlaySFX("sfx_ui_end_defeat");
        }

        foreach (Transform child in groupItem)
        {
            Destroy(child.gameObject);
        }

        float delay = 0;
        foreach (RewardArgs rewardArgs in args.listRewardArgs)
        {
            GameObject obj = Instantiate(prefabItemViewSlot, groupItem);
            ItemData itemArgs = AllItem.dictData[rewardArgs.reward];
            obj.GetComponent<ItemViewSlot>().Init(rewardArgs.reward, rewardArgs.num);//, itemArgs.rarity);

            delay += 0.1f;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }

        objBtnClaimAd.SetActive(args.canTakeAD);
        objBtnBattleStats.SetActive(args.canTakeAD);

        await UniTask.Delay(1000);

        objBtnClaim.SetActive(true);
    }

    public void OnClaim()
    {
        EndSystem.Instance.OnClaim(1, CloseUI);
    }

    public void OnAdClaim()
    {
        AdControl.Instance.OnVideoAdSkippable(AllAd.dictData[AdType.BattleEndBonus], () =>
        {
            //2倍奖励
            EndSystem.Instance.OnClaim(2, CloseUI);
        }, () =>
        {

        });
    }

    public void OnClickStats()
    {
        objInfo.SetActive(true);
        if (isOpenInfo)
        {
            return;
        }
        //uiBattleStats.RefreshUI();
        isOpenInfo = true;
    }

    public void OnCloseStats()
    {
        objInfo.SetActive(false);
        isOpenInfo = false;
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PopupDungeonInfoArgs : PopupArgs
{
    public DungeonData dungeonData;
    public Action sweepCallback;
}

public class PopupDungeonInfo : PopupBase
{
    [SerializeField] TextMeshProUGUI textName, textDifficulty, textCount;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameObject objPrefab;
    [SerializeField] Image imgKey;
    [SerializeField] GameObject objBtnSweep, objBtnAd, objBtnPlay;
    [SerializeField] TextMeshProUGUI textRemain;

    PopupDungeonInfoArgs args;
    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        this.args = args as PopupDungeonInfoArgs;
        Refresh();
    }

    void Refresh()
    {
        //名称
        textName.text = UtilityLocalization.GetLocalization(args.dungeonData.displayName);
        //难度
        int level = GameData.userData.userDungeon.dictDungeonLevel[args.dungeonData.dungeonName];
        textDifficulty.text = level.ToString();
        //数量
        RefreshCount();
        //钥匙
        GameAssetControl.AssignIcon(args.dungeonData.costItemName, imgKey);
        //奖励
        List<RewardArgs> rewardList = DungeonSystem.Instance.GetDungeonReward(args.dungeonData.dungeonName, level);
        foreach (Transform child in rectTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (RewardArgs reward in rewardList)
        {
            GameObject obj = Instantiate(objPrefab, rectTransform);
            obj.GetComponent<ItemViewSlot>().Init(reward.reward, reward.num);
        }
        objBtnSweep.SetActive(level > 1);
    }

    void RefreshCount()
    {
        
    }

    public void OnClickSweep()
    {
        DungeonSystem.Instance.OnSweepDungeon(args.dungeonData, () =>
        {
            args.sweepCallback?.Invoke();
            RefreshCount();
        });
    }

    public void OnClickPlay()
    {
        DungeonSystem.Instance.OnPlayDungeon(args.dungeonData, () =>
        {
            OnClose();
        });
    }
}
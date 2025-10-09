using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 邮件系统
/// </summary>
public class MailSystem : Singleton<MailSystem>
{
    //邮件数据
    List<MailArgs> listMailArgs;

    public void Init()
    {
        listMailArgs = new List<MailArgs>();
        //离线模式不请求邮件数据
        if (GameConfig.main.productMode == ProductMode.DevOffline)
        {
            return;
        }
        RequestMailData();
    }

    // public async void Open(GameObject parent)
    // {
    //     // 打开邮件界面
    //     GameObject prefab = await GameAssets.GetPrefabAsync("ui_mail");
    //     if (prefab == null)
    //     {
    //         Debug.LogError($"=== MailSystem: Open Prefab not found: ui_mail ===");
    //         return;
    //     }

    //     GameObject page = Instantiate(prefab, parent.transform, false);
    //     page.name = "ui_mail";
    //     MailListArgs mailListArgs = new MailListArgs()
    //     {
    //         mailArgs = listMailArgs
    //     };
    //     EventManager.TriggerEvent(EventNameMail.EVENT_MAIL_ON_REFRESH_UI, mailListArgs);
    // }

    public async void Open()
    {
        await UIMain.Instance.OpenUI("mail", UIPageType.Overlay);
        MailListArgs mailListArgs = new MailListArgs()
        {
            mailArgs = listMailArgs
        };
        EventManager.TriggerEvent(EventNameMail.EVENT_MAIL_ON_INIT_UI, mailListArgs);
    }

    public async void RequestMailData()
    {
        listMailArgs.Clear();
        //请求玩家邮件数据
        string languageType = PlayerPrefs.GetString("loc", "en");
        int userID = GameData.userData.userAccount.userID;
        if (GameConfig.main.productMode == ProductMode.DevOffline) return;
        await CloudMail.Instance.OnGetMail(userID, languageType,
            (string stream) =>
            {
                listMailArgs = JsonConvert.DeserializeObject<List<MailArgs>>(stream);
                List<MailArgs> listGMailArgs = new List<MailArgs>();
                //处理功能性邮件
                foreach (MailArgs mailArgs in listMailArgs)
                {
                    if (!mailArgs.title.StartsWith("GM")) continue;
                    if (mailArgs.status == 3)
                    {
                        listGMailArgs.Add(mailArgs);
                        continue;
                    }
                    ;
                    mailArgs.isGM = true;
                    switch (mailArgs.title)
                    {
                        case "GM_ChangeChapter":
                            mailArgs.functionType = MailFunctionType.ChangeChapter;
                            break;
                    }
                }
                listMailArgs.RemoveAll(x => listGMailArgs.Contains(x));
                getMailData();
            });
    }

    public async void OpenDetail(MailArgs mailArgs)
    {
        //判断是否为功能性邮件
        if (mailArgs.title == "Functional Mail")
        {
            EventManager.TriggerEvent(EventNameMail.EVENT_MAIL_OPEN_FUNCTION_DETAIL_UI, mailArgs);
            return;
        }
        EventManager.TriggerEvent(EventNameMail.EVENT_MAIL_OPEN_DETAIL_UI, mailArgs);
        //邮件已打开无需操作
        if (mailArgs.status != 0)
        {
            return;
        }
        //有奖励status更新
        await CloudMail.Instance.OnChangeMailStatus(GameData.userData.userAccount.userID, mailArgs.id, string.IsNullOrEmpty(mailArgs.reward) == true ? 2 : 1,
            (string stream) =>
            {
                listMailArgs.Find(x => x.id == mailArgs.id).status = int.Parse(stream);
                getMailData();
            });
    }
    void getMailData()
    {
        // PinSystem.Instance.CheckMailPin();
        //排序
        listMailArgs.Sort((x, y) =>
        {
            if (x.status != y.status)
            {
                return x.status.CompareTo(y.status);
            }
            return x.time.CompareTo(y.time);
        });

        MailListArgs mailListArgs = new MailListArgs()
        {
            mailArgs = listMailArgs
        };
        EventManager.TriggerEvent(EventNameMail.EVENT_MAIL_ON_REFRESH_UI, mailListArgs);
    }

    public async void OnDelete()
    {
        if (listMailArgs.Count <= 0)
        {
            return;
        }
        await CloudMail.Instance.OnDeleteMail(GameData.userData.userAccount.userID,
            (string stream) =>
            {
                int[] ids = JsonConvert.DeserializeObject<int[]>(stream);
                if (ids.Length <= 0)
                {
                    return;
                }
                listMailArgs.RemoveAll(args => ids.Contains(args.id));
                getMailData();
            });
    }

    public async void OnReceive(int mailID)
    {
        if (listMailArgs.Count <= 0)
        {
            return;
        }
        List<int> mailIDs = new List<int>();
        if (mailID == -1)
        {
            foreach (MailArgs args in listMailArgs)
            {
                if (args.status == 2 || args.status == 3)
                {
                    continue;
                }
                mailIDs.Add(args.id);
            }
        }
        else
        {
            mailIDs.Add(mailID);
        }
        if (mailIDs.Count <= 0)
        {
            return;
        }
        await CloudMail.Instance.OnReceiveMail(GameData.userData.userAccount.userID, mailIDs.ToArray(),
        (string stream) =>
        {
            List<int> receiveMailIDs = JsonConvert.DeserializeObject<List<int>>(stream);
            if (receiveMailIDs.Count <= 0)
            {
                return;
            }
            List<RewardArgs> listRewardArgs = new List<RewardArgs>();
            foreach (int id in receiveMailIDs)
            {
                var mailArgs = listMailArgs.Find(x => x.id == id);
                if (mailArgs == null)
                {
                    continue;
                }
                mailArgs.status = 2;
                List<RewardArgs> result = getReward(mailArgs.reward);
                listRewardArgs.AddRange(result);
            }
            RewardSystem.Instance.OnReward(listRewardArgs);
            getMailData();
            // GameSaver.Instance.OnSave();
        });
    }

    public List<RewardArgs> getReward(string data)
    {
        List<RewardArgs> listRewardArgs = new List<RewardArgs>();
        if (string.IsNullOrEmpty(data))
        {
            return listRewardArgs;
        }
        string[] row = data.Split('&');
        foreach (string str in row)
        {
            string[] reward = str.Split('^');
            RewardArgs rewardArgs = new RewardArgs();
            rewardArgs.reward = reward[0];
            rewardArgs.num = int.Parse(Regex.Replace(reward[1].Trim(), @"[^\d]", ""));
            listRewardArgs.Add(rewardArgs);
        }
        return listRewardArgs;
    }

    public int OnCheckPinNum()
    {
        foreach (MailArgs args in listMailArgs)
        {
            if (args.status == 0)
            {
                return 1;
            }
            if (string.IsNullOrEmpty(args.reward) || args.status == 2)
            {
                continue;
            }
            return 1;
        }
        return 0;
    }

    #region 功能性邮件
    public async void OnConfirm(int mailID)
    {
        if (listMailArgs.Count <= 0)
        {
            return;
        }
        MailArgs mailArgs = listMailArgs.Find(x => x.id == mailID);
        if (mailArgs == null || mailArgs.isGM == false || mailArgs.status == 3)
        {
            return;
        }
        //判断功能
        MessageManager.Instance.OnLoading();

        await CloudMail.Instance.OnChangeMailStatus(GameData.userData.userAccount.userID, mailArgs.id, 3,
            (string stream) =>
            {
                switch (mailArgs.functionType)
                {
                    case MailFunctionType.ChangeChapter:
                        OnChangeChapter(mailArgs.content.Split('-'));
                        break;
                    default:
                        break;
                }
            });
    }

    async void OnChangeChapter(string[] content)
    {
        int level = int.Parse(content[0]);
        GameData.userData.userLevel.levelProgressMain.levelIndex = level;
        EventManager.TriggerEvent(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.MailDataUpdate });
        await Task.Delay(1000);
        Game.Instance.Restart();
    }
    #endregion
}

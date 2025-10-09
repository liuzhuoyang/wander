#if UNITY_EDITOR

using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using System;
using Sirenix.Utilities;

namespace onicore.editor
{
    public class SeasonEditor : OdinWindowBase
    {
        //[MenuItem("Arrow Editor/Season Editor")]
        public static void OpenWindow()
        {
            GetWindow<SeasonEditor>().Show();
        }

        public override void Reset()
        {
            base.Reset();
        }

        [BoxGroup("黄金令牌基础信息", Order = 0)]
        /*
        [ValueDropdown("seasonIDArray")]
        public string seasonID;*/

        [BoxGroup("黄金令牌基础信息")]
        public string seasonName;

        /*
        [TableList]
        public List<SeasonRewardSlot> rewardList;
        */

        [BoxGroup("ButtonSeason", ShowLabel = false, Order = 5)]
        [HorizontalGroup("ButtonSeason/Split")]
        [Button("读取令牌数据", ButtonHeight = 40)]
        public void LoadSeasonData()
        {

            /*
            string seasonDataStream = ReadWrite.ReadSeasonDataEDITOR(seasonID);
            if (seasonDataStream.IsNullOrWhitespace()) return;

            SeasonData seasonData = JsonConvert.DeserializeObject<SeasonData>(seasonDataStream);
            seasonName = seasonData.seasonName;
            */
            ////普通奖励
            //rewardList = new List<SeasonRewardSlot>();    //新，添加普通奖励
            //for (int i = 0; i < seasonData.rewardList.Count; i++)
            //{
            //    SeasonRewardSlot rewardSlot = new SeasonRewardSlot();

            //    string[] args = seasonData.rewardList[i].Split("^");                         //奖励写入
            //    if (ControllerEditor.InventoryAssets.AllItemAssets.ContainsKey(args[0]))
            //    {
            //        InventorySlot normalReward = new InventorySlot()
            //        {
            //            item = ControllerEditor.InventoryAssets.AllItemAssets[args[0]].item,
            //            count = int.Parse(args[1])
            //        };
            //        rewardSlot.reward = normalReward;
            //    }
            //    else if (ControllerEditor.SpecialItemAssets.SpecialAssets.ContainsKey(args[0]))
            //    {
            //        Debug.LogError("should not have perk in normal reward");
            //    }

            //    string[] vipArgs = seasonData.rewardVIPList[i].Split("^");                         //奖励写入
            //    if (ControllerEditor.InventoryAssets.AllItemAssets.ContainsKey(vipArgs[0]))
            //    {
            //        InventorySlot vipReward = new InventorySlot()
            //        {
            //            item = ControllerEditor.InventoryAssets.AllItemAssets[vipArgs[0]].item,
            //            count = int.Parse(vipArgs[1])
            //        };
            //        rewardSlot.rewardVIP = vipReward;
            //    }
            //    else if (ControllerEditor.SpecialItemAssets.SpecialAssets.ContainsKey(vipArgs[0]))
            //    {
            //        InventorySlot perkReward = new InventorySlot()
            //        {
            //            item = ControllerEditor.SpecialItemAssets.SpecialAssets[vipArgs[0]].item,
            //            count = int.Parse(vipArgs[1])
            //        };
            //        rewardSlot.rewardVIP = perkReward;
            //    }
            //    rewardList.Add(rewardSlot);
            //}
        }

        /*
        [BoxGroup("ButtonSeason", ShowLabel = false)]
        [HorizontalGroup("ButtonSeason/Split")]
        [Button("保存令牌数据", ButtonHeight = 40)]
        public void SaveSeasonData()
        {
            SeasonData seasonData = new SeasonData();
            seasonData.seasonID = seasonID;
            seasonData.seasonName = seasonName;
            //普通奖励写入
            seasonData.rewardList = new List<string>();
            for (int i = 0; i < rewardList.Count; i++)
            {
                string rewardArgs = rewardList[i].reward.item.name + "^" + rewardList[i].reward.count; //任务奖励
                seasonData.rewardList.Add(rewardArgs);
            }
            //氪金奖励
            seasonData.rewardVIPList = new List<string>();
            for (int i = 0; i < rewardList.Count; i++)
            {
                string rewardArgs = rewardList[i].rewardVIP.item.name + "^" + rewardList[i].rewardVIP.count;
                seasonData.rewardVIPList.Add(rewardArgs);
            }
            ReadWrite.WriteSeasonDataEditor(seasonData, seasonID);
        }
        [BoxGroup("ButtonSeason", ShowLabel = false)]
        [HorizontalGroup("ButtonSeason/Split")]
        [Button("清空令牌数据", ButtonHeight = 40)]
        public void ResetData()
        {
            seasonID = "";
            seasonName = "";
            rewardList = new List<SeasonRewardSlot>();
        }*/
    }

    /*
    [Serializable]
    public class SeasonRewardSlot
    {
        public InventorySlot reward;
        public InventorySlot rewardVIP;
    }

    /
    [Serializable]
    public struct InventorySlot
    {
        [HideLabel, PreviewField(55)]
        [BoxGroup("Info", LabelText = "基础信息", Order = 0)]
        [HorizontalGroup("Info/Split", 55, LabelWidth = 67)]
        public Texture2D item;

        [HideLabel]
        [HorizontalGroup("Info/Split")]
        [SuffixLabel("num", Overlay = true)]
        public int count;

        public string GetDropArgs()
        {
            return item.name + "^" + count;
        }
    }*/
}
#endif
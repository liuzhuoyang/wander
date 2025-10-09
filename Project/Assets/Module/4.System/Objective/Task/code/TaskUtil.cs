using System.Collections.Generic;

public static class TaskUtil
{
     private static List<TaskRewardItem> listDailyRewardItem;
     private static List<TaskRewardItem> listWeeklyRewardItem;

     public static void InitData()
     {
          listDailyRewardItem = new List<TaskRewardItem>();
          listDailyRewardItem.Add(new TaskRewardItem { pointNeeded = 20, reward = "item_token_tavern", rewardNum = 1 });
          listDailyRewardItem.Add(new TaskRewardItem { pointNeeded = 40, reward = "item_energy", rewardNum = 5 });
          listDailyRewardItem.Add(new TaskRewardItem { pointNeeded = 60, reward = "item_loot_box_1", rewardNum = 3 });
          listDailyRewardItem.Add(new TaskRewardItem { pointNeeded = 80, reward = "item_loot_box_2", rewardNum = 1 });
          listDailyRewardItem.Add(new TaskRewardItem { pointNeeded = 100, reward = "item_gem", rewardNum = 50 });

          listWeeklyRewardItem = new List<TaskRewardItem>();
          listWeeklyRewardItem.Add(new TaskRewardItem { pointNeeded = 140, reward = "item_loot_box_2", rewardNum = 2 });
          listWeeklyRewardItem.Add(new TaskRewardItem { pointNeeded = 280, reward = "item_token_tavern", rewardNum = 2 });
          listWeeklyRewardItem.Add(new TaskRewardItem { pointNeeded = 420, reward = "item_token_star", rewardNum = 3 });
          listWeeklyRewardItem.Add(new TaskRewardItem { pointNeeded = 560, reward = "item_loot_box_3", rewardNum = 2 });
          listWeeklyRewardItem.Add(new TaskRewardItem { pointNeeded = 700, reward = "item_gem", rewardNum = 150 });
     }

     public static List<TaskRewardItem> GetDailyRewardList()
     {
          return listDailyRewardItem;
     }

     public static List<TaskRewardItem> GetWeeklyRewardList()
     {
          return listWeeklyRewardItem;
     }

     public static int GetNextDailyPoint(int point)
     {
          if (point == 0)
          {
               return listDailyRewardItem[0].pointNeeded;
          }
          for (int i = 0; i < listDailyRewardItem.Count - 1; i++)
          {
               if (point == listDailyRewardItem[i].pointNeeded)
               {
                    return listDailyRewardItem[i + 1].pointNeeded;
               }
          }
          return int.MaxValue;
     }

     public static int GetNextWeeklyPoint(int point)
     {
          if (point == 0)
          {
               return listWeeklyRewardItem[0].pointNeeded;
          }
          for (int i = 0; i < listWeeklyRewardItem.Count - 1; i++)
          {
               if (point == listWeeklyRewardItem[i].pointNeeded)
               {
                    return listWeeklyRewardItem[i + 1].pointNeeded;
               }
          }
          return int.MaxValue;
     }
}
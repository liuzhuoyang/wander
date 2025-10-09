using System.Collections.Generic;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public static class GameData
{
    public static UserData userData;
    #region 对外方法

    #endregion
    public static AllLocalization allLocalization;

    public static async UniTask Init()
    {
        Reset();
        
        // 并行加载所有数据，加快加载速度
        var tasks = new UniTask[]
        {
            #region Shared 共用数据
            //allChapter.Init(),
            //allChest.Init(),
            //allTheme.Init(),
            #endregion
        };

        // 合批处理所有 Init 方法
        await UniTask.WhenAll(tasks);

        // 这个地方如果不并行，会大幅增加开始页面loading时间
        await UniTask.WhenAll(
            
        );

        // 并行加载战斗相关资源
        await UniTask.WhenAll(
            
        );

        InitUserdata();
    }

    public static void Reset()
    {
        userData = null;
    }
    
    //初始化用户数据
    static void InitUserdata()
    {
        // 读取用户数据
        string userDataStream = ReadWrite.ReadUserData();

        // 创建默认的新用户数据
        UserData defaultUserData = new UserData();
        defaultUserData.Init();

        // 如果用户数据为空，使用默认数据
        if (string.IsNullOrEmpty(userDataStream))
        {
            userData = defaultUserData;
            return;
        }

        // 读取用户数据
        UserData savedUserData = JsonConvert.DeserializeObject<UserData>(userDataStream);

        // 检查老用户数据
        userData = MergeUserData(savedUserData, defaultUserData);
    }


    //检查老用户数据，如果出现新的字段，应用默认数据的字段，避免我们更新数据而老用户数据没有导致的崩溃
    static UserData MergeUserData(UserData savedUserData, UserData defaultUserData)
    {
        // 获取 UserData 类型的所有字段
        var fields = typeof(UserData).GetFields();
        UserData mergedUserData = new UserData();

        foreach (var field in fields)
        {
            // 获取保存的数据和默认数据的字段值
            var savedValue = field.GetValue(savedUserData);
            var defaultValue = field.GetValue(defaultUserData);

            // 如果默认数据为 null，说明这个数据类没有，直接跳过
            if(defaultValue == null)
            {
                continue;
            }

            if (savedValue == null)
            {
                // 如果保存的值为 null，说明整个数据类都没有，使用默认值
                field.SetValue(mergedUserData, defaultValue);
            }
            else
            {
                // 如果保存的值不是 null，检查其内部字段
                var subFields = savedValue.GetType().GetFields();
                foreach (var subField in subFields)
                {
                    var subFieldValue = subField.GetValue(savedValue);
                    var defaultSubFieldValue = subField.GetValue(defaultValue);

                    if (subFieldValue == null)
                    {
                        // 如果子字段为 null，使用默认数据的子字段值
                        subField.SetValue(savedValue, defaultSubFieldValue);
                    }
                }
                // 设置合并后的值
                field.SetValue(mergedUserData, savedValue);
            }
        }
        return mergedUserData;
    }

    //检查时间,
    public static void CheckDateTime()
    {
        if (TimeManager.Instance.CheckIsNewDay())
        {
            UnityEngine.Debug.Log(" === GameData: New Day ===");
            //新的一天
            userData.userAnalytics.ResetDaily();//重置打点
            userData.userItem.OnResetDaily();
        }

        if (TimeManager.Instance.CheckIsNewWeek())
        {
            userData.userItem.OnResetWeekly();
            UnityEngine.Debug.Log(" === GameData: New Week ===");
            //新的一周
        }

        if (TimeManager.Instance.CheckIsNewMonth())
        {
            userData.userItem.OnResetMonthly();
            UnityEngine.Debug.Log(" === GameData: New Month ===");
            //新的一个月
        }

        TimeManager.Instance.UpdateLoginTime();
    }

}

using System.Collections.Concurrent;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class GameSaver : Singleton<GameSaver>
{
    // 保存队列和防抖机制
    private ConcurrentQueue<UserData> saveQueue = new ConcurrentQueue<UserData>();
    private float lastSaveTime = 0f;
    private float saveDelay = 1f; // 1秒冷却时间
    private CancellationTokenSource delayedSaveCancellation = new CancellationTokenSource();
    
    // 日志记录
    private ActionType currentDelayedAction = ActionType.None; // 当前延迟保存的事件
    private int cancelledSaveCount = 0; // 被取消的保存次数
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.StopListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnActionSave);
        delayedSaveCancellation?.Cancel();
        delayedSaveCancellation?.Dispose();
    }

    public void Init()
    {
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnActionSave);
    }

    void OnActionSave(ActionArgs e)
    {
        if (!Game.Instance.CheckIsGameInit())
        {
            return;
        }
        
        // 检查是否需要保存
        if (CheckIsOnSave(e.action))
        {
            RequestSave(e.action);
        }
    }
    
    /// <summary>
    /// 判断是否需要保存
    /// </summary>
    private bool CheckIsOnSave(ActionType action)
    {
        switch (action)
        {
            //!有必要执行保存，不要全部往里加，控制触发数量
            case ActionType.CompleteOpeningBattle:
            case ActionType.ChangeProfile:
            case ActionType.LevelStart:
            case ActionType.DungeonStart:
            case ActionType.ArenaStart:
            case ActionType.TowerStart:
            case ActionType.GearUpgrade:
            case ActionType.GearUpdate:
            case ActionType.TalentUpgrade:
            case ActionType.UnlockChapter:
            case ActionType.SummonGearSuccess:
            case ActionType.RankUpdate:
            case ActionType.ItemReward:
            case ActionType.IAPComplete:
            case ActionType.FundClaimed:
            case ActionType.ChallengeClaimed:
            case ActionType.MarketBuy:
            case ActionType.ShopBuy:
            case ActionType.BattleBaseUnlock:
            case ActionType.RelicUpgrade:
            case ActionType.RelicUnlock:
            case ActionType.TaskClaimed:
            case ActionType.EnergyBuy:
            case ActionType.Login:
            case ActionType.Register:
            case ActionType.TavernRefresh:
            case ActionType.UseItem:
            case ActionType.UpdatePromo:
            case ActionType.UpdateLiveEvent:
            case ActionType.OnSeqTaskComplete:
            case ActionType.RankingLike:
            case ActionType.ArenaDataUpdate:
            case ActionType.EnergyRecover:
            case ActionType.MailDataUpdate:
            case ActionType.TowerDataUpdate:
                return true;
            default:
                return false;
        }
    }
    
    /// <summary>
    /// 判断是否为高优先级事件（立即保存，不防抖）
    /// </summary>
    bool IsHighPriorityAction(ActionType action)
    {
        switch (action)
        {
            case ActionType.IAPComplete:            // 内购完成 - 最重要
            case ActionType.Login:                  // 登录
            case ActionType.Register:               // 注册
            case ActionType.CompleteOpeningBattle:  // 完成开场战斗
                return true;
            default:
                return false;
        }
    }
    
    /// <summary>
    /// 请求保存数据
    /// </summary>
    private void RequestSave(ActionType action)
    {
        // 清空队列，只保留最新的数据
        while (saveQueue.TryDequeue(out _)) { }
        saveQueue.Enqueue(GameData.userData);
        
        // 检查是否为高优先级事件（立即保存）
        if (IsHighPriorityAction(action))
        {
            // 如果有延迟保存任务，记录被取消的事件
            if (currentDelayedAction != ActionType.None)
            {
                cancelledSaveCount++;
                Debug.Log($"=== GameSaver: 高优先级事件取消延迟保存 - 被取消事件: {currentDelayedAction}, 当前事件: {action}, 累计取消: {cancelledSaveCount} ===");
            }
            
            // 取消之前的延迟保存任务
            delayedSaveCancellation.Cancel();
            delayedSaveCancellation = new CancellationTokenSource();
            
            Debug.Log($"=== GameSaver: 高优先级立即保存 - 事件: {action} ===");
            ProcessSaveQueue(action);
        }
        else
        {
            // 防抖机制：如果距离上次保存时间太短，延迟保存
            float currentTime = Time.time;
            if (currentTime - lastSaveTime < saveDelay)
            {
                // 如果有延迟保存任务，记录被取消的事件
                if (currentDelayedAction != ActionType.None)
                {
                    cancelledSaveCount++;
                    Debug.Log($"=== GameSaver: 防抖取消保存 - 事件: {currentDelayedAction}, 累计取消: {cancelledSaveCount} ===");
                }
                
                // 取消之前的延迟保存任务
                delayedSaveCancellation.Cancel();
                delayedSaveCancellation = new CancellationTokenSource();
                
                // 记录新的延迟保存事件
                currentDelayedAction = action;
                Debug.Log($"=== GameSaver: 触发防抖保存 - 事件: {action}, 延迟时间: {saveDelay}s ===");
                
                // 启动新的延迟保存任务
                DelayedSave(delayedSaveCancellation.Token, action);
            }
            else
            {
                Debug.Log($"=== GameSaver: 立即保存 - 事件: {action} ===");
                ProcessSaveQueue(action);
            }
        }
    }
    
    /// <summary>
    /// 延迟保存
    /// </summary>
    private async void DelayedSave(CancellationToken cancellationToken, ActionType action)
    {
        try
        {
            // 等待冷却时间，支持取消
            await UniTask.Delay((int)(saveDelay * 1000), cancellationToken: cancellationToken);
            
            // 如果没被取消，执行保存
            Debug.Log($"=== GameSaver: 延迟保存执行 - 事件: {action} ===");
            ProcessSaveQueue(action);
        }
        catch (System.OperationCanceledException)
        {
            // 任务被取消，记录日志
            Debug.Log($"=== GameSaver: 延迟保存被取消 - 事件: {action} ===");
        }
    }
    
    /// <summary>
    /// 处理保存队列
    /// </summary>
    private async void ProcessSaveQueue(ActionType action)
    {
        // 只保存最新的一个数据
        if (saveQueue.TryDequeue(out UserData userData))
        {
            try
            {
                // 更新保存时间
                userData.userAccount.saveTime = TimeManager.Instance.GetCurrentTimeSpan();
                
                Debug.Log($"=== GameSaver: 开始保存数据 - 事件: {action}, 队列剩余: {saveQueue.Count} ===");
                
                // 异步保存
                await ReadWrite.WriteUserdataAsync(userData);
                lastSaveTime = Time.time;
                
                // 重置延迟保存状态
                currentDelayedAction = ActionType.None;
                
                Debug.Log($"=== GameSaver: 保存完成 - 事件: {action}, 耗时: {Time.time - lastSaveTime:F2}s ===");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"=== GameSaver: 保存失败 - 事件: {action}, 错误: {ex.Message} ===");
            }
        }
        else
        {
            Debug.LogWarning($"=== GameSaver: 队列为空，无法保存 - 事件: {action} ===");
        }
    }
    
    /// <summary>
    /// 强制保存（用于游戏退出等关键时机）
    /// </summary>
    public void ForceSave()
    {
        if (GameData.userData != null)
        {
            GameData.userData.userAccount.saveTime = TimeManager.Instance.GetCurrentTimeSpan();
            ReadWrite.WriteUserdata(GameData.userData);
            Debug.Log("=== GameSaver: Force save completed ===");
        }
    }
}

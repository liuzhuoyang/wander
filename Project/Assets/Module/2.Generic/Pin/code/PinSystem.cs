using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PinSystem : Singleton<PinSystem>
{
    public bool TestPinState = false;
    public List<PinData> testPinNodes;
//PinNode地图，用于UI层获取到对应的pinNode
    public Dictionary<string, PinNode> pinGraph{get; private set;}

    public void Init()
    {
        pinGraph = new Dictionary<string, PinNode>();
        //首先创建所有的PinNode，并添加进字典
        foreach(var item in AllPin.dictData)
        {
            pinGraph.Add(item.Key, CreatePin(item.Value));
        }
        //建立各个PinNode之间的父子关系
        foreach(var item in AllPin.dictData)
        {
            PinData arg = item.Value;
            PinNode node = pinGraph[item.Key];
            //添加父节点
            if(!item.Value.isRoot)
                node.parentNode = pinGraph[item.Value.parentNode.name];
            //添加子节点
            if(!item.Value.isLeaf)
            {
                node.childNodes = new HashSet<PinNode>();
                foreach(var key in arg.childNodes)
                {
                    node.childNodes.Add(pinGraph[key.name]);
                }
            }
        }

        EventManager.StartListening<PinUpdateArgs>(EventNamePin.EVENT_ON_UPDATE_PIN, OnPinUpdate); //监听对Pin Node的开启
        //刷新所有已加载UI的pin
        EventManager.TriggerEvent<PinRestArgs>(EventNamePin.EVENT_ON_REST_PIN, null);
    }
    private void OnDestroy()
    {
        EventManager.StopListening<PinUpdateArgs>(EventNamePin.EVENT_ON_UPDATE_PIN, OnPinUpdate); //监听对Pin Node的开启
    }


    void OnPinUpdate(PinUpdateArgs pinUpdateArgs)
    {
        string id = pinUpdateArgs.pinID;
        pinGraph[id].SetNewPinState(pinUpdateArgs.newState);
    }

    PinNode CreatePin(PinData pinNodeData)
    {
        PinNode node = new PinNode();
        node.isPined = true;
        node.selfResolve = pinNodeData.selfResolve;

        return node;
    }
    [Button("Test Pin")]
    public void SwitchTestPin()
    {
        TestPinState = !TestPinState;
        foreach (var pin in pinGraph)
        {
            if (testPinNodes.Find(x=>x.name == pin.Key))
            {
                pin.Value.SetNewPinState(TestPinState);
            }
        }
    }
    
#region Obsolete 方法
    //事件注册，用于注册触发PinNode更新
    private event Action<bool> OnTaskPinCheck;
    private event Action<bool> OnDefaultPinCheck;
    void CheckGearLevelPin()
    {
        //这些部分需要进入到各自模块
        /*
        //装备解锁判断
        foreach (var item in GameData.userData.userGear.dictGear)
        {
            //解锁与升级所需碎片数量
            int shardCost = MetaFormula.GetGearUpgradeShardNeeded(item.Value.level);
            string shardName = ConstantItem.SHARD_GEAR_HEAD + item.Key;
            //还未满级，且碎片数足够
            if (item.Value.level < 100 && ItemSystem.Instance.GetItemNum(shardName) >= shardCost)
            {
                OnGearPinCheck?.Invoke(true);
                return;
            }
        }
        //装备升级判断
        OnGearPinCheck?.Invoke(false);*/
    }
    void CheckGearStartPin()
    {
        /*
        foreach (var item in GameData.userData.userGear.dictGear)
        {
            //升级所需金币数量
            int coinCost = MetaFormula.GetStarNumByStar(item.Value.star);
            //可以升星，且金币数足够
            if (item.Value.star > 0 && item.Value.star < EventNameGear.MAX_GEAR_STAR && ItemSystem.Instance.GetItemNum(ConstantItem.COIN) >= coinCost)
            {
                OnGearPinCheck?.Invoke(true);
                return;
            }
        }*/
    }
    void CheckDefaultPin()
    {
        OnDefaultPinCheck?.Invoke(false);
    }
    void CheckTaskPin()
    {
        //这些部分需要进入到各自模块
        int dailyTask = 0;
        foreach (var item in GameData.userData.userTask.dictUserTask)
        {
            if (item.Value.isClaim) continue;
            if (item.Value.doneNum >= AllTask.dictData[item.Key].targetNum)
            {
                dailyTask++;
            }
        }
        OnTaskPinCheck?.Invoke(dailyTask>0);
    }
#endregion
}
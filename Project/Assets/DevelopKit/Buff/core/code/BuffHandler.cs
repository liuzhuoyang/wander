using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleBuff
{
    public class BuffHandler : BattleBehaviour
    {
        private Dictionary<string, Buff> buffDict;
        private Action<Buff> onBuffCreated;
        public Buff GetBuff(string buffKey) => buffDict[buffKey];
        public override void BattleLateUpdate()
        {
            if (buffDict == null) return;
            foreach (var buff in buffDict.Values.ToList())
            {
                if (buff.IsPending)
                    buff.ChangeBuffState(BuffState.Working);
                if (buff.IsDone)
                    HandleBuffComplete(buff);
                else
                {
                    buff.UpdateBuff();
                    if (buff.IsDone)
                        HandleBuffComplete(buff);
                }
            }
        }
        public void CleanUpAllBuff()
        {
            if (buffDict == null) return;
            foreach (var buff in buffDict.Values.ToList())
            {
                HandleBuffRemove(buff);
            }
            buffDict.Clear();
        }
        public bool TryAddBuff(string buffID)
        {
            var buff = BuffDataManager.Instance.GetBuff(buffID);
            return TryAddBuffRaw(buff);
        }
        public bool TryAddBuffRaw(Buff buff)
        {
            buff.Initialize(this);
            //Todo：通过Buff Manager创建Buff，并调用Buff Awake
            if (buffDict == null)
                buffDict = new Dictionary<string, Buff>();
            //若存在免疫此buff的Tag，则不添加buff
            foreach (var go in buffDict.Values)
            {
                if ((go.m_buffImmuneTag & buff.m_buffTag) != BuffTag.None)
                {
                    return false;
                }
            }

            if (buffDict.ContainsKey(buff.m_buffTypeID))
            {
                buff.RefreshBuff();
                //Todo:同类型buff，触发刷新buff事件
            }
            else
            {
                buffDict.Add(buff.m_buffTypeID, buff);
                foreach (var go in buffDict.Values.ToHashSet())
                {
                    //若存在buff被此tag免疫，则移除该buff
                    if ((buff.m_buffImmuneTag & go.m_buffTag) != BuffTag.None)
                    {
                        HandleBuffRemove(go);
                    }
                }
                buff.ChangeBuffState(BuffState.Pending);
            }
            onBuffCreated?.Invoke(buff);
            return true;
        }
        public void RemoveBuff(string id)
        {
            if (buffDict.ContainsKey(id))
            {
                HandleBuffRemove(buffDict[id]);
            }
        }
        void HandleBuffRemove(Buff buff)
        {
            buffDict.Remove(buff.m_buffTypeID);
            buff.ChangeBuffState(BuffState.Detached);
        }
        void HandleBuffComplete(Buff buff)
        {
            buffDict.Remove(buff.m_buffTypeID);
            buff.ChangeBuffState(BuffState.Detached);
        }
        //强制执行 匿名Buff，不纳入生命管理
        public static void ExcuteBuff(AnnonomusBuff buff)
        {
            buff.Initialize(null);
            buff.ExcuteBuff();
        }
    }
}
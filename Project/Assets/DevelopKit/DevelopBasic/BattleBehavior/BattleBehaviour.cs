using UnityEngine;

//战斗行为基类，战场中的所有动态对象的基类
public abstract class BattleBehaviour : MonoBehaviour
{
    protected bool cleanUpMarker = false;
    public bool NeedCleanUp => cleanUpMarker;
    protected virtual void Start()
    {
        BattleBehaviourManager.Instance.RegisterBehaviour(this);
    }

    protected virtual void OnDestroy()
    {
        if(BattleBehaviourManager.Instance!=null)
           BattleBehaviourManager.Instance.UnregisterBehaviour(this);
    }

    //标记为待回收，在manager class中会根据次marker进行回收，回收之后恢复标记
    public void MarkCleanUp(bool needCleanUp) => cleanUpMarker = needCleanUp;

    public virtual void BattleUpdate() { }
    public virtual void BattleFixedUpdate() { }
    public virtual void BattleLateUpdate() { }
}
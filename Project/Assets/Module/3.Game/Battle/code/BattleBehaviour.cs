using UnityEngine;

public class BattleBehaviour : MonoBehaviour
{
    protected bool cleanUpMarker = false;
    public bool NeedCleanUp => cleanUpMarker;
    private void Start()
    {
        //BattleBehaviourManager.Instance.RegisterBehaviour(this);
    }

    private void OnDestroy()
    {
        //BattleBehaviourManager.Instance.UnregisterBehaviour(this);
    }

    public void MarkCleanUp(bool needCleanUp)=>cleanUpMarker = needCleanUp;

    public virtual void BattleUpdate() { }
    public virtual void BattleFixedUpdate(){}
    public virtual void BattleLateUpdate(){}
}

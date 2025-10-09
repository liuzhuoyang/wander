using UnityEngine;

public class AnimatorControl
{
    protected Animator animator;
#region AnimatorKey
    protected static readonly int IdleTrigger   = Animator.StringToHash("idle");
    protected static readonly int AttackTrigger = Animator.StringToHash("attack");
    protected static readonly int MoveTrigger   = Animator.StringToHash("move");
    protected static readonly int DieTrigger    = Animator.StringToHash("die");
    protected static readonly int MoveSpeed    = Animator.StringToHash("moveSpeed");
    protected static readonly int AttackSpeed   = Animator.StringToHash("attackSpeed");
    protected static readonly int IdleOffsetFloat = Animator.StringToHash("idleoffset");
    protected static readonly int IsMoveBool = Animator.StringToHash("isMove");
    protected const string IDLE_STATE = "idle";
    protected const string RUN_STATE = "run";
    protected const string DIE_STATE = "die";
    protected const string SPAWN_STATE = "spawn";
#endregion
    protected const float MAX_DEATH_TIME = 2f;

    public AnimatorControl Init(Animator animator){
        this.animator = animator;
        animator.SetFloat(IdleOffsetFloat, Random.Range(0f, 1f));
        return this;
    }
    public void PlayCustomTrigger(string triggerName)=>animator.SetTrigger(triggerName);
    public void PlayIdle(){
        if(IsState(IDLE_STATE) || IsNextState(IDLE_STATE)) return;
        animator.SetFloat(IdleOffsetFloat, Random.Range(0f, 1f));
        animator.SetBool(IsMoveBool, false);
    }
    public void PlayRun(){
        if(IsState(RUN_STATE) || IsNextState(IDLE_STATE)) return;
        animator.SetBool(IsMoveBool, true);
    }
    public void PlayAttack()=>animator.SetTrigger(AttackTrigger);
    public void PlayDie()=>animator.SetTrigger(DieTrigger);
    
    bool IsState(string stateName){
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(stateName);
    }
    bool IsNextState(string stateName){
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);
        return nextState.IsName(stateName);        
    }
    public bool IsDieDone()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if(currentState.IsName(DIE_STATE))
        {
            return currentState.normalizedTime >= 1;
        }
        return false;
    }
    public bool IsSpawnDone()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if(!currentState.IsName(SPAWN_STATE))
        {
            return true;
        }
        return false;
    }
    public void SetAttackSpeed(float speed)=>animator.SetFloat(AttackSpeed, speed);
    public void SetMoveSpeed(float speed)=>animator.SetFloat(MoveSpeed, speed);
    public void OnKill()=>animator.enabled = false;
}

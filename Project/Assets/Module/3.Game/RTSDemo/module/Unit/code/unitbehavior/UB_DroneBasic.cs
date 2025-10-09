using UnityEngine;
using BattleActor;
using BattleActor.Unit;

public class UB_DroneBasic : MonoBehaviour, IUnitBehaviour
{
    public enum DroneState
    {
        Empty, //无效状态
        Hang, //挂起隐藏
        TakeOff, //起飞
        Cruise, //巡航
        Approach, //接敌
        SurroundTarget, //环绕敌人
        PreReturn, //准备返航
        Return, //返航
        Landing, //降落
        FlyOut, //飞向地图外
    }
[Header("Drone Basic")]
    [SerializeField] protected DroneState droneState = DroneState.Hang;
    [SerializeField] protected Vector2 cruiseRange = new Vector2(4,5.5f);
    [SerializeField] protected float hangTime = 1; //保持隐藏的时长
    [SerializeField] protected float rotateLerpSpeed = 2;
    [SerializeField] protected float takeOffTime = 0.5f;
    [SerializeField] protected float PreReturnTime = 2f;
    [SerializeField] protected float searchRadius = 10;
    [SerializeField] protected float flyoutSpeed = 2;
    [SerializeField] protected float scanFreqMulti = 1;

    protected float scanTime = 0;
    protected float stateTimer = 0;
    protected Vector2 launchPos;
    protected Vector2  target;
    protected UnitBase self;
    protected IBattleActor potentialTarget;
    protected CircleMovement circleMovement;

    public void Init(UnitBase unit)
    {
        this.self = unit;
    //参数初始化
        launchPos = transform.position;
        stateTimer = 0;
    //组件初始化
        circleMovement = new CircleMovement();
    //添加攻击方法
        self.OnUnitAttackExcute += DroneAttack;
    //状态初始化
        unit.SwitchBody(false); //初始化时隐藏单位外观
        unit.SwitchUnitBehavior(false); //起飞阶段关闭单位行为
        ChangeDroneState(DroneState.Hang);
    //无人机不会受到索敌，也无法被攻击
        unit.DisableTriggerBox();
    }
    public void CleanUp(){
        self.OnUnitAttackExcute -= DroneAttack;
    }
    public void UnitUpdate()
    {
        DroneState nextState = DroneState.Empty;
        switch (droneState)
        {
            case DroneState.Hang:
                nextState = HangUpdate();
                break;
            case DroneState.TakeOff:
                nextState = TakeOffUpdate();
                break;
            case DroneState.Cruise:
                nextState = CruiseUpdate();
                break;
            case DroneState.Approach:
                nextState = ApproachUpdate();
                break;
            case DroneState.SurroundTarget:
                nextState = SurroundTargetUpdate();
                break;
            case DroneState.PreReturn:
                nextState = PreReturnUpdate();
                break;
            case DroneState.Return:
                nextState = ReturnUpdate();
                break;
            case DroneState.Landing:
                nextState = LandingUpdate();
                break;
            case DroneState.FlyOut:
                nextState = FlyOutUpdate();
                break;
        }
        if (nextState != DroneState.Empty)
        {
            ChangeDroneState(nextState);
        }
    }
    public void SendUnitBack()
    {
        ChangeDroneState(DroneState.FlyOut);
    }
    protected void ChangeDroneState(DroneState droneState)
    {
        if(this.droneState == droneState) return;
        if(this.droneState == DroneState.Hang) self.SwitchBody(true);//挂起结束后，显示单位
        if(this.droneState == DroneState.SurroundTarget) self.StopAttack();
        if(this.droneState == DroneState.TakeOff) self.SwitchUnitBehavior(true); //起飞后才开启单位行为

        this.droneState = droneState;
        stateTimer = 0;
        switch(droneState)
        {
            case DroneState.Cruise:
                scanTime = 0;
                circleMovement.EnterCirclingWithGivenCenter(
                    transform.position,
                    GeometryUtil.RandomPointInCircle(Vector2.zero, 0.5f, 1f),
                    Random.Range(cruiseRange.x, cruiseRange.y),
                    self.unitMovement.GetVelocity()
                );
                break;
            case DroneState.SurroundTarget:
                self.StartAttack();

                circleMovement.EnterCirclingWithGivenCenter(
                    transform.position,
                    potentialTarget.position,
                    self.currentAttackRange*UnitService.UNIT_ATTACK_RANGE_SHRINK,
                    self.unitMovement.GetVelocity(),
                    0.5f
                );
                circleMovement.rotatingRadius = Mathf.Max(circleMovement.rotatingRadius, 1);
                break;
            case DroneState.PreReturn:
                circleMovement.EnterCirclingWithGivenRadius(
                    transform.position,
                    self.unitMovement.GetVelocity(),
                    Vector2.zero,
                    Random.Range(10,12)
                );
                break; 
            case DroneState.Landing:
                transform.position = launchPos + Vector2.up*3;
                self.unitMovement.SetVelocityVector(Vector3.down);
                break;
        }
    }
#region 状态更新
    protected virtual DroneState TakeOffUpdate()
    {
        return DroneState.Empty;
    }
    protected virtual DroneState CruiseUpdate()
    {
        return DroneState.Empty;
    }
    protected virtual DroneState ApproachUpdate()
    {
        return DroneState.Empty;
    }
    protected virtual DroneState SurroundTargetUpdate()
    {
        return DroneState.Empty;
    }
    protected DroneState HangUpdate()
    {
        stateTimer += Time.deltaTime;
        if(stateTimer>hangTime)
        {
            return DroneState.TakeOff;
        }
        return DroneState.Hang;
    }
    protected DroneState PreReturnUpdate()
    {
        stateTimer += Time.deltaTime;
        target = Vector2.Lerp(circleMovement.UpdatePointOnCircle(self.currentMoveSpeed), launchPos+Vector2.up*3, stateTimer/PreReturnTime);
        self.unitMovement.SlerpVelocity(target-(Vector2)transform.position, rotateLerpSpeed);

        if(stateTimer>PreReturnTime)
            return DroneState.Return;

        return DroneState.Empty;
    }
    protected DroneState LandingUpdate()
    {
        target = launchPos;
        self.unitMovement.SetVelocityVector(Vector2.down);
        if(self.IsPosInRange(target, 0.25f)){
            UnitManager.Instance.RemoveUnitImmediately(self);
        }
        return DroneState.Empty; 
    }
    protected DroneState ReturnUpdate()
    {
        target = launchPos+Vector2.up*3;
        self.unitMovement.SlerpVelocity(target-(Vector2)transform.position, 7);
        if(self.IsPosInRange(target, 0.25f)){
            return DroneState.Landing;
        }
        return DroneState.Empty;
    }
    protected DroneState FlyOutUpdate()
    {
        self.unitMovement.SlerpVelocity(self.unitMovement.GetVector().normalized*flyoutSpeed, 7, false);
        if(!self.m_unitViewBasic.IsVisible())
        {
            UnitManager.Instance.RemoveUnitImmediately(self);
        }
        return DroneState.FlyOut;
    }
#endregion
    protected virtual void DroneAttack(){
        if(self.needReload) return;
        if(IBattleActor.IsInvalid(potentialTarget)) return;
        self.AttackAtActor(potentialTarget);
    }
}
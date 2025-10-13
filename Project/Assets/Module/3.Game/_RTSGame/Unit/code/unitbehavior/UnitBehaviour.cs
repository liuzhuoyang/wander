namespace RTSDemo.Unit
{
    public interface IUnitBehaviour
    {
        void Init(UnitBase unit);
        void UnitUpdate();
        void CleanUp();
        void SendUnitBack() { } //遣返单位的方法
        protected const float MAX_ATTACK_RANGE_PENDING = 2f; //远程敌人最大的首次攻击停顿为2秒
        protected const float MAX_ATTACK_MELEE_PENDING = 1f; //近战敌人最大的首次攻击停顿为1秒
    }
}
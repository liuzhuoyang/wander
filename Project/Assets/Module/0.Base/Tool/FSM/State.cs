namespace SimpleFSM
{
//状态基础类
    public abstract class State<T>
    {
        public virtual string StateName{get;}
        // public virtual void Init(T context){}
        public virtual void EnterState(T context){} //进入状态
        public virtual State<T> InterruptState(T context, State<T> nextState){return null;} //中断状态，返回值为下一状态
        public virtual State<T> UpdateState(T context){return null;} //状态Update。
        // public virtual void CleanUp(T context){}
    }

//状态机
    public interface IStateMachine<T>
    {
        State<T> m_currentState{get;} //当前状态
        void GoToState(State<T> nextState); //状态切换的外部调用方法
        void UpdateState(); //状态机更新方法
    }
}
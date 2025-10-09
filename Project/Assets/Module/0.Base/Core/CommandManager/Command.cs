using System;
using UnityEngine;

public enum CommandStatus{Detached, Pending, Working, Success, Fail, Aborted}
public abstract class Command<T>{
	private event Action onCompleteCallBack;
    public CommandStatus Status = CommandStatus.Detached;
    public bool IsDetached => Status == CommandStatus.Detached;
    public bool IsPending => Status == CommandStatus.Pending;
    public bool IsWorking => Status == CommandStatus.Working;
    public bool IsSuccess => Status == CommandStatus.Success;
    public bool IsFail => Status == CommandStatus.Fail;
    public bool IsAborted => Status == CommandStatus.Aborted;  
    public bool IsDone{get{return Status == CommandStatus.Fail || Status == CommandStatus.Success || Status == CommandStatus.Aborted;}}
    protected Command<T> nextCommand = null; //考虑之后用Queue来实现生产管线，而非链表
	public Command<T> AppendCommand(Command<T> command){
		if(nextCommand==null){
			nextCommand = command;
		}
		else{
			nextCommand.AppendCommand(command);
		}
		return command;
	}
	public Command<T> GetNextCommand(){return nextCommand;}
	public Command<T> OnCommandComplete(Action callback){
		onCompleteCallBack = callback;
		return this;
	}
    internal void SetStatus(CommandStatus newStatus){
		if (Status == newStatus) return;

		Status = newStatus;

		switch (newStatus){
			case CommandStatus.Working:
				Init();
				break;
			case CommandStatus.Success:
				OnSuccess();
				CleanUp();
				break;
			case CommandStatus.Aborted:
				OnAbort();
				CleanUp();
				break;
			case CommandStatus.Fail:
				OnFail();
				CleanUp();
				break;
			case CommandStatus.Detached:
			case CommandStatus.Pending:
				break;
			default:
				Debug.Log("None status is found");
				Debug.Assert(false);
				break;
		}
	}
	internal virtual void CommandUpdate(T context){}
	protected virtual void Init(){}
	protected virtual void OnAbort(){}
	protected virtual void OnSuccess(){onCompleteCallBack?.Invoke();}
	protected virtual void OnFail(){}
	protected virtual void CleanUp(){}
}

public class C_Wait<T>:Command<T>{
	public float waitTime = 0;
	public C_Wait(float _time){
		waitTime = _time;
	}
    internal override void CommandUpdate(T context)
    {
        base.CommandUpdate(context);

		waitTime -= Time.deltaTime;
		if(waitTime<=0) SetStatus(CommandStatus.Success);
    }
}
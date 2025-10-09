using System.Collections.Generic;

//多列表生产管理
public class CommandManager<T>
{
    public bool isBusy => commandList!=null && commandList.Count > 0;
    public T context;
    protected List<Command<T>> commandList;
    public void UpdateCommand()
    {
        if(!isBusy) return;
        for(int i=commandList.Count-1; i>=0; i--){
        //Note: The Command in the list might be swapped during Adding Command or Aborting Command.
        //Becareful not to Add Command while complete a command inside a command
            var command = commandList[i];
            if(command.IsPending) command.SetStatus(CommandStatus.Working);
            if(command.IsDone)
                HandleCompletion(command);
            else {
                command.CommandUpdate(context);
                if(command.IsDone) HandleCompletion(command);
            }
        }
    }
    public void AbortAllCommands(){
        for(int i=commandList.Count-1; i>=0; i--){
            var command = commandList[i];
            command.SetStatus(CommandStatus.Aborted);
            HandleCompletion(command);
        }
        commandList.Clear();
    }
    protected void HandleCompletion(Command<T> command){
        commandList.Remove(command);
        var nextCommand = command.GetNextCommand();
        if(nextCommand != null && command.IsSuccess){
            AddCommand(nextCommand);
        }
        command.SetStatus(CommandStatus.Detached);
    }
    public void AddCommand(Command<T> command){
        if(commandList==null) commandList = new List<Command<T>>();
        commandList.Add(command);
        command.SetStatus(CommandStatus.Pending);
    }
}
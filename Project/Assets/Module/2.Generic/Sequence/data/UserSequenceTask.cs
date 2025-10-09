using System.Collections.Generic;

public class UserSequenceTask
{
    public List<SequenceTaskArgs> listSeqTask;

    public UserSequenceTask()
    {
        UpdateOrInitializeData();
    }

    public void UpdateOrInitializeData()
    {
        listSeqTask = new List<SequenceTaskArgs>();
    }

    //加入剧情序列到用户数据，后面根据这个数据，进入大厅后判断播放序列
    public void OnAddSeqTask(SequenceTaskArgs args)
    {
        listSeqTask.Add(args);
        //排序 顺序为 Feature > PlotSequence > Plot > Popup
        listSeqTask.Sort((a, b) =>
        {
            int GetPriority(SequenceTaskType t) => t switch
            {
                SequenceTaskType.Feature => 0,
                SequenceTaskType.PlotSequence => 1,
                SequenceTaskType.Plot => 2,
                SequenceTaskType.Popup => 3,
                _ => 4
            };
            return GetPriority(a.seqTaskType) - GetPriority(b.seqTaskType);
        });
    }
}

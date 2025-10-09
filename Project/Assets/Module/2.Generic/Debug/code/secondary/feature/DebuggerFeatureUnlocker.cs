using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebuggerFeatureUnlocker : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject prefabFeatureBtn;

    List<DebuggerFeatureUnlockerSlot> listFeatureSlot;

    public void Open()
    {
        gameObject.SetActive(true);
        
        // 初始化slot列表
        listFeatureSlot = new List<DebuggerFeatureUnlockerSlot>();

        //清空容器
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    
        List<FeatureData> listFeatureData = AllFeature.dictData.Values.
            Where(x => x.unlockConditionType != FeatureUnlockConditionType.None && x.unlockConditionType != FeatureUnlockConditionType.Coming).ToList();

        foreach (FeatureData featureData in listFeatureData)
        {
            GameObject obj = Instantiate(prefabFeatureBtn, container);
            obj.GetComponent<DebuggerFeatureUnlockerSlot>().Init(featureData);
            listFeatureSlot.Add(obj.GetComponent<DebuggerFeatureUnlockerSlot>());
        }
    }

    public void OnConfirm()
    {
        foreach (DebuggerFeatureUnlockerSlot slot in listFeatureSlot)
        {
            if(slot.isSelected)
            {
                 SequenceTaskSystem.Instance.AddFeatureSeq(slot.featureData.featureType);
            }
        }

        gameObject.SetActive(false);
        Debugger.Instance.OnCloseDebug(); //关闭时候会让Game进入Home的状态机，从而触发SequenceTaskSystem的解锁功能
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}

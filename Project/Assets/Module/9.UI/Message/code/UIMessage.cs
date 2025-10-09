using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class UIMessage : MonoBehaviour
{
    public GameObject panel;
    private void OnDestroy()
    {
        EventManager.StopListening<MsgArgs>(EventNameMsg.EVENT_MESSAGE_UI, OnMessageUI);
    }

    public void Start()
    {
        EventManager.StartListening<MsgArgs>(EventNameMsg.EVENT_MESSAGE_UI, OnMessageUI);
    }

    public async void OnMessageUI(MsgArgs args)
    {
        await OnAsyncCreateMessageUI(args);
    }

    async UniTask OnAsyncCreateMessageUI(MsgArgs args)
    {
        try
        {
            // 异步加载预制体
            var panelPrefab = await GameAsset.GetPrefabAsync(args.target);

            // 实例化预制体并设置父对象
            var panel = Instantiate(panelPrefab, this.transform);

            // 调用预制体上的 Init 方法，传递参数
            panel.GetComponent<MsgBase>().Init(args);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error creating message UI: {ex.Message}");
        }
    }
}

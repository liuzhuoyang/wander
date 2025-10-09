using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnvVfxControl : Singleton<EnvVfxControl>
{
    public EnvVfxView envVfxView;

    public void Init()
    {
        envVfxView = gameObject.AddComponent<EnvVfxView>();
        envVfxView.Init();
        envVfxView.transform.SetParent(this.transform);
    }

    public async UniTask CreateViewObject(Vector2 pos, string targetName)
    {
        await envVfxView.CreateView(pos, targetName);
    }
}

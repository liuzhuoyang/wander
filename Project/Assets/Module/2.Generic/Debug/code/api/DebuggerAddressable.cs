using UnityEngine;
using Cysharp.Threading.Tasks;

public class DebuggerAddressable : MonoBehaviour
{
    public async void OnLoadBattleAsset()
    {
        await GameAssetBattleManager.Instance.OnLoadBattleAsset();
        //Debug.Log("=== DebuggerAddressable: 加载战斗资源 ===");
    }

    public void OnLogBattleAsset()
    {
        //Debug.Log("=== DebuggerAddressable: 战斗资源 ===");
        foreach (var item in GameAssetBattleManager.Instance.battlePrefabDict)
        {
            Debug.Log($"=== DebuggerAddressable: 战斗资源 {item.Key}");
        }

        foreach (var item in GameAssetBattleManager.Instance.battlePrefabHandles)
        {
            Debug.Log($"=== DebuggerAddressable: 资源Handle Addressable Handle {item.Key} ===");
        }
    }

    public void OnReleaseBattleAsset()
    {
        GameAssetBattleManager.Instance.OnReleaseBattlePrefabAsset();
        //Debug.Log("=== DebuggerAddressable: 释放战斗资源 ===");
    }
}

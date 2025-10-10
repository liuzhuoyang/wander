#if UNITY_EDITOR
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EditorGizmosView : MonoBehaviour
{
    public async UniTask CreateView<T>(Vector2 pos, string targetName, string spriteName, Type handleType = null, T args = null) where T : class
    {
        GameObject viewObj;
        viewObj = Instantiate(await GameAsset.GetPrefabAsync("editor_gizmos"), transform);
        viewObj.name = targetName;
        viewObj.transform.position = pos;
        viewObj.GetComponent<SpriteRenderer>().sprite = await GameAsset.GetSpriteAsync(spriteName);

        // // 根据传入类型添加编辑器控制和碰撞体
        // var handle = handleType != null && handleType.IsSubclassOf(typeof(EditHandle))
        //     ? viewObj.AddComponent(handleType) as EditHandle
        //     : viewObj.AddComponent<EditHandle>();
        // handle.AddPoligonCollider2D();
        // handle.Init(spriteName);

        // // 根据类型添加到数据
        // if (handle is EditHandleSpawnPoint)
        // {
        //     EditHandleSpawnPoint handleSpawnPoint = handle as EditHandleSpawnPoint;
        //     // SpawnPointArgs spawnPointArgs = args as SpawnPointArgs;
        //     //handleSpawnPoint.Init(spawnPointArgs);
        //     //MapControl.Instance.LevelData.AddSpawnPointHandle(handle as EditHandleSpawnPoint);
        // }
    }
}
#endif
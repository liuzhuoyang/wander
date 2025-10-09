using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class MapColliderControl : Singleton<MapColliderControl>
{
    public void Init()
    {
        
    }

    public async UniTask CreateViewObject(Vector2 pos)
    {
        GameObject viewObj;
        viewObj = Instantiate(await GameAsset.GetPrefabAsync("map_collider"), transform);
        viewObj.transform.position = pos;

        #if UNITY_EDITOR
        //编辑器场景创建碰撞体
        if (SceneManager.GetActiveScene().name == "map_editor")
        {
             //添加EditHandleTerrain类型编辑器控制组件
            viewObj.AddComponent<EditHandleCollider>().AddPoligonCollider2D();
            viewObj.GetComponent<EditHandleCollider>().Init("collider");
            //控制期加入到编辑列表，后续数据用于保存地图
            MapControl.Instance.LevelData.AddColliderHandle(viewObj.GetComponent<EditHandleCollider>());
        }
        #endif
    }
}

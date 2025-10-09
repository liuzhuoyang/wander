using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class EnvVfxView : MonoBehaviour
{
    public void Init()
    {

    }

    public async UniTask CreateView(Vector2 pos, string targetName)
    {
        GameObject viewObj;
        viewObj = Instantiate(await GameAsset.GetPrefabAsync(targetName), transform);

        viewObj.transform.position = pos;
        
        Debug.Log("=== VfxView: create vfx actor: posX " + pos.x + " PosY" + pos.y);

#if UNITY_EDITOR
        
        //非编辑场景直接跳出
        if (SceneManager.GetActiveScene().name != "map_editor")
            return;

        viewObj.name =  targetName + "|" + pos.x.ToString("F3") + "," + pos.y.ToString("F3");

        GameObject vfxSymbol;
        vfxSymbol = Instantiate(await GameAsset.GetPrefabAsync("edit_symbol_vfx"), viewObj.transform);
        vfxSymbol.GetComponent<SpriteRenderer>().sprite = await GameAsset.GetSpriteAsync("edit_vfx");
        vfxSymbol.AddComponent<EditHandle>().AddPoligonCollider2D();
        vfxSymbol.GetComponent<EditHandle>().Init(targetName);

        SpriteRenderer render = vfxSymbol.GetComponent<SpriteRenderer>();

        //添加碰撞体
        PolygonCollider2D collider = vfxSymbol.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;

        //添加刚体
        Rigidbody2D rigidbody = vfxSymbol.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Static;
#endif
    }
}

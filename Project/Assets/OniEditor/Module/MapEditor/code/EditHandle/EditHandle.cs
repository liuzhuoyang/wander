#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using onicore.editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

//编辑器场景创建的编辑器控制组件基类
//使用场景：如一个出生点被创建时，就会挂上EditHandleSpawnPoint组件,继承于EditHandle。
//执行选择擦除等操作，编辑操作会改变里面的数据，最后数据会在点保存时候写入到LevelRawData中成为地图数据
[ExecuteInEditMode]
public class EditHandle : MonoBehaviour
{   
    protected OdinEditorWindow window;  // 添加protected修饰符，使子类可以访问
    
    bool isEdit = false;
    [ReadOnly]
    public string targetName;

    public virtual void Init(string targetName)
    {
        this.targetName = targetName;
    }

    public void AddPoligonCollider2D()
    {
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void OnHoveOver()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.8f, 0.8f);
    }

    public void OnDeselect()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void OnEdit()
    {
        isEdit = true;
        window = OdinEditorWindow.InspectObject(this);
        window.titleContent = new GUIContent("EditHandle 场景物件窗口");
        window.position = new Rect(100, 100, 400, 500);
        window.Show();
        Debug.Log("=== EditHandle: 物件编辑 ===");
    }

    [BoxGroup("BoxAction", GroupName = "确认数据, 不要用左上的红色关闭窗口")]
    [Button("保存并关闭", ButtonHeight = 80)]
    public virtual void OnSaveAndClose()
    {
        isEdit = false;
        window.Close();
    }

    public virtual void OnErase()
    {
        isEdit = false;
        DestroyImmediate(gameObject);
    }
}

#endif
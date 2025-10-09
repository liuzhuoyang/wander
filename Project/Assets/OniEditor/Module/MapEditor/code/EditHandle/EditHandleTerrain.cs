#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;


    public class EditHandleTerrain : EditHandle
    {   
        [ReadOnly]
        public TerrainLayer terrainLayer;

    /*
        //旋转角度
        [Range(0, 360)]
        public int rotateZ;
    */
        public void Init(string targetName, TerrainLayer terrainLayer)
        {
            this.terrainLayer = terrainLayer;
            //this.rotateZ = rotateZ;
            base.Init(targetName);
        }

        public override void OnErase()
        {
            base.OnErase();
            MapControl.Instance.LevelData.RemoveTerrainHandle(this);
        }

        public override void OnEdit()
        {
            base.OnEdit();
            //rotateZ = 0;
            //isValidating = true;
        }

    /*
        // 在编辑器中，当属性值发生变化时，会触发OnValidate方法
        // 只有在第一次点开编辑才会开启这个标签，避免在初始化时候，触发OnValidate
        // OnSaveAndClose里会关闭，如果用红色按钮关闭就不会重置
        bool isValidating = false;
        void OnValidate()
        {
            if(isValidating)
                OnValueChanged();
        }

        void OnValueChanged()
        {
            OnUpdateRotateZ();
        }

        public void OnUpdateRotateZ()
        {
            transform.rotation = Quaternion.Euler(0, 0, rotateZ);
        }*/
    }

#endif
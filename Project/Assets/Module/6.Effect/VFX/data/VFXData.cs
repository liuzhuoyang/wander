#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.AddressableAssets;
using System.IO;
using System.Linq;
#endif

using Sirenix.OdinInspector;
using UnityEngine;

public enum VFXLifeCycleMode
{
    Manual = 0,
    Pooled = 1,
    ManagedParticle = 2,
}
[CreateAssetMenu(fileName = "VfxData", menuName = "OniData/FX/VFX/VFXData")]
public class VFXData : ScriptableObject
{
    [ReadOnly]
    public string vfxName;
    [PropertyTooltip("-1表示无限时长，需要手动清理"), HideIf("@this.lifeMode == VFXLifeCycleMode.ManagedParticle")]
    public float life;
    [LabelText("VFX的生命周期方式")]
    public VFXLifeCycleMode lifeMode = VFXLifeCycleMode.Pooled;
    [ReadOnly]
    public VfxType type;

    public bool IsPooled => lifeMode == VFXLifeCycleMode.Pooled;

#if UNITY_EDITOR
    [Button("检查并确认命名", ButtonSizes.Large)]
    public void Init()
    {
        vfxName = GetVFXPrefabName();

        EditorUtility.SetDirty(this);
    }

    string GetVFXPrefabName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this);
        string assetDirectory = Path.GetDirectoryName(assetPath);
        string parentDirectory = Path.GetDirectoryName(assetDirectory); // 获取父节点文件夹
        string prefabDirectory = Path.Combine(parentDirectory, "prefab");
        
        if (!Directory.Exists(prefabDirectory))
        {
            EditorUtility.DisplayDialog("提示", 
                $"在父文件夹 {parentDirectory} 下没有找到prefab文件夹", "OK");
            return "";
        }

        // 获取当前AudioAsset的名称（不包含扩展名）
        string currentName = Path.GetFileNameWithoutExtension(assetPath);
        
        // 在clip文件夹中查找同名的AudioClip文件
        string[] prefabFile = Directory.GetFiles(prefabDirectory, "*.prefab", SearchOption.TopDirectoryOnly);

        
        string[] allAudioFiles = prefabFile.ToArray();
        
        var matchingFiles = allAudioFiles.Where(file => 
            Path.GetFileNameWithoutExtension(file).Equals(currentName, System.StringComparison.OrdinalIgnoreCase)).ToArray();
        
        if (matchingFiles.Length > 0)
        {
            // 返回找到的第一个文件的名称（不包含扩展名）
            string foundFileName = Path.GetFileNameWithoutExtension(matchingFiles[0]);
            
            if (matchingFiles.Length > 1)
            {
                string fileList = string.Join("\n", matchingFiles.Select(f => Path.GetFileName(f)));
                EditorUtility.DisplayDialog("找到多个同名Prefab", 
                    $"在父文件夹的prefab文件夹中找到 {matchingFiles.Length} 个同名的prefab文件，将使用第一个：\n\n{fileList}", "OK");
            }
            
            return foundFileName;
        }
        else
        {
            EditorUtility.DisplayDialog("未找到同名prefab", 
                $"在父文件夹的prefab文件夹中没有找到与 '{currentName}' 同名的Prefab文件", "OK");
            return "";
        }
    }
#endif
}
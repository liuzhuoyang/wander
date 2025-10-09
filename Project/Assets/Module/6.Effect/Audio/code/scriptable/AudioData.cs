#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.AddressableAssets;
using System.IO;
using System.Linq;
#endif

using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "audio_data", menuName = "OniData/FX/Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [ReadOnly] public string clipName;

#if UNITY_EDITOR
    [Button("Init Data")]
    public void InitData()
    {
        string foundClipName = GetAudioClipName();
        
        if (string.IsNullOrEmpty(foundClipName))
        {
            EditorUtility.DisplayDialog("错误",
                "未找到同名的AudioClip文件 \n 请确保在clip文件夹中有对应的音频文件", "OK");
            return;
        }
        
        // 检查找到的文件名是否与当前Asset名称一致
        if (foundClipName != this.name)
        {
            EditorUtility.DisplayDialog("警告",
                $"找到的AudioClip文件名 '{foundClipName}' 与Asset名称 '{this.name}' 不一致 \n 是否继续使用找到的文件名？", "是", "否");
            // 这里可以根据需要决定是否继续
        }
        
        clipName = foundClipName;
        EditorUtility.SetDirty(this);
    }

    string GetAudioClipName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this);
        string assetDirectory = Path.GetDirectoryName(assetPath);
        string parentDirectory = Path.GetDirectoryName(assetDirectory); // 获取父节点文件夹
        string clipDirectory = Path.Combine(parentDirectory, "clip");
        
        if (!Directory.Exists(clipDirectory))
        {
            EditorUtility.DisplayDialog("提示", 
                $"在父文件夹 {parentDirectory} 下没有找到clip文件夹", "OK");
            return "";
        }

        // 获取当前AudioAsset的名称（不包含扩展名）
        string currentName = Path.GetFileNameWithoutExtension(assetPath);
        
        // 在clip文件夹中查找同名的AudioClip文件
        string[] audioFiles = Directory.GetFiles(clipDirectory, "*.mp3", SearchOption.TopDirectoryOnly);
        string[] wavFiles = Directory.GetFiles(clipDirectory, "*.wav", SearchOption.TopDirectoryOnly);
        string[] oggFiles = Directory.GetFiles(clipDirectory, "*.ogg", SearchOption.TopDirectoryOnly);
        
        string[] allAudioFiles = audioFiles.Concat(wavFiles).Concat(oggFiles).ToArray();
        
        var matchingFiles = allAudioFiles.Where(file => 
            Path.GetFileNameWithoutExtension(file).Equals(currentName, System.StringComparison.OrdinalIgnoreCase)).ToArray();
        
        if (matchingFiles.Length > 0)
        {
            // 返回找到的第一个文件的名称（不包含扩展名）
            string foundFileName = Path.GetFileNameWithoutExtension(matchingFiles[0]);
            
            if (matchingFiles.Length > 1)
            {
                string fileList = string.Join("\n", matchingFiles.Select(f => Path.GetFileName(f)));
                EditorUtility.DisplayDialog("找到多个同名AudioClip", 
                    $"在父文件夹的clip文件夹中找到 {matchingFiles.Length} 个同名的AudioClip文件，将使用第一个：\n\n{fileList}", "OK");
            }
            
            return foundFileName;
        }
        else
        {
            EditorUtility.DisplayDialog("未找到同名AudioClip", 
                $"在父文件夹的clip文件夹中没有找到与 '{currentName}' 同名的AudioClip文件", "OK");
            return "";
        }
    }
#endif
}
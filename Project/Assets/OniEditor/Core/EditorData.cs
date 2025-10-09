#if UNITY_EDITOR

using System.Collections.Generic;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using onicore.editor;
/// <summary>
/// 编辑器用的数据类，用于编辑时的数据传输
/// </summary>
public static class EditorData
{
    //public static string mapName;
    public static LevelData currentLevelAsset;

    public static void Reset()
    {
        currentLevelAsset = null;
    }
}
#endif
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class AddressableUtility
{
    /// <summary>
    /// 把路径以及路径下所有文件放到addressable里
    /// </summary>
    /// <param name="path"></param>
    /// <param name="groupName"></param>
    public static void AddAssetsToAddressables(string path, string groupName)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup group = settings.FindGroup(groupName);

        if (group == null)
        {
            Debug.LogError("Addressable group not found: " + groupName);
            return;
        }

        var assetPaths = AssetDatabase.FindAssets("", new[] { path })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => !p.EndsWith(".meta") && System.IO.File.Exists(p));

        foreach (var assetPath in assetPaths)
        {
            var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(assetPath), group);
            entry.address = SimplifyAddressablePath(assetPath);
        }

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, group, true, true);
    }

    //简化名字
    public static string SimplifyAddressablePath(string fullPath)
    {
        // Extracts the file name without extension
        string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fullPath);

        // Optionally, you can add more logic here to further simplify or modify the name

        return fileNameWithoutExtension;
    }
}
#endif
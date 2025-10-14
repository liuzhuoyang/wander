using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

using Sirenix.OdinInspector;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public enum FontType
{
    Title,
    Content,
}

public class UILocalizationHandler : MonoBehaviour
{
    [Tooltip("设置为动态更新内容，会在代码中更新而不是创建显示是现实文本，常用语{0}，{1}，{2}等需要动态更换内容的形式")]
    public bool isDynamic;
    public string localizationKey;
    TextMeshProUGUI content;

    [OnValueChanged("RefreshFontStyle")]
    public FontType fontType;

    private string lastLocalizationKey = null;

    private void Awake()
    {
        content = GetComponent<TextMeshProUGUI>();
        if (content == null)
        {
            Debug.LogWarning("=== UI Localization: TextMeshProUGUI 组件未找到，请检查UI预制体 ===");
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        if (content == null)
        {
            Debug.LogWarning($"=== UI Localization: {localizationKey} 错误配置的组件（找不到TextMeshProUGUI） 请检查UI预制体 {gameObject.name} ===");
            return;
        }

        if (GameData.allLocalization == null || AllLocalization.dictLocalizationData.Count <= 0)
        {
            return;
        }

        // 优化字体更新逻辑，只有在字体发生变化时才更新
        if (GameAssetManagerGeneric.Instance != null && content.font != GameAssetManagerGeneric.Instance.font)
        {
            content.font = GameAssetManagerGeneric.Instance.font;
        }

        RefreshFontStyle();

        // 如果跳过 key 更新，则直接返回
        if (isDynamic)
        {
            return;
        }

        // 如果 localizationKey 为空，直接返回
        if (string.IsNullOrEmpty(localizationKey))
        {
            Debug.LogWarning($"=== UI Localization: 未配置 localizationKey，根据下一行打印结构，查出问题的prefab ===");
            Transform current = gameObject.transform;
            string path = current.name;
            while (current.parent != null)
            {
                current = current.parent;
                path = current.name + "/" + path;
            }
            Debug.LogWarning($"对象层级结构: {path}");
            return;
        }

        // 检查是否与上次的 key 相同，避免重复查找
        if (localizationKey == lastLocalizationKey)
        {
            return; // 如果 key 没有变化，跳过更新
        }

        content.text = localizationKey.StartsWith("mapping/") ? UtilityLocalization.GetLocalization(localizationKey) : UtilityLocalization.GetLocalization(localizationKey);
        // 更新缓存的 key, 避免后续重复设置
        lastLocalizationKey = localizationKey;
    }

    //更新字体样式，是Title样式还是Content样式
    void RefreshFontStyle()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            string fontName = UtilityLocalization.GetFontName();
            //非运行模式的更新
            content = GetComponent<TextMeshProUGUI>();
            if (fontType == FontType.Title)
            {
                content.fontMaterial = GameAsset.GetAssetEditor<TMP_FontAsset>($"{fontName}").material;
            }
            else if (fontType == FontType.Content)
            {
                content.fontMaterial = GameAsset.GetAssetEditor<Material>($"{fontName}_content");
            }
            return;
        }

#endif

        if (GameAssetManagerGeneric.Instance == null || GameAssetManagerGeneric.Instance.fontMaterialTitle == null)
        {
            return;
        }

        if (fontType == FontType.Title)
        {
            content.fontMaterial = GameAssetManagerGeneric.Instance.fontMaterialTitle;
        }
        else if (fontType == FontType.Content)
        {
            content.fontMaterial = GameAssetManagerGeneric.Instance.fontMaterialContent;
        }
    }

#if UNITY_EDITOR
    [HideLabel]
    [BoxGroup("Loc Edit")]
    [DisableIf("isDynamic")]
    [TableColumnWidth(160, Resizable = false)]
    //[VerticalGroup("Key")]
    [ValueDropdown("LocalizationKeyList")]
    public string localizationKeyEditor;

    [BoxGroup("Loc Edit")]
    //[VerticalGroup("Key")]
    [ReadOnly]
    public string localizationContent;

    [BoxGroup("Loc Edit")]
    [DisableIf("isDynamic")]
    //[VerticalGroup("Key")]
    [Button("更新选择的Key和对应英文预览", ButtonHeight = 30), GUIColor(0.5f, 0.9f, 0.5f)]
    public void Fetch()
    {
        localizationKey = localizationKeyEditor;
        string value = LocalizationDataCollection.GetValue(localizationKey);
        localizationContent = value;
        gameObject.GetComponent<TextMeshProUGUI>().text = value;
        RefreshFontStyle();
    }

    //下面部分是获取当前模块和公用模块的本地化key的帮助方法
    [BoxGroup("Loc Edit")]
    List<string> LocalizationKeyList()
    {
        
        List<string> keyList = new List<string>();
        keyList.Add("");
        List<LocalizationData> dataList = GetCurrentModuleLocalizationData();

        foreach (var item in dataList)
        {
            foreach (var key in item.list)
            {
                keyList.Add(key.key);
            }
        }
        return keyList;
    }

    //获取当前模块里的本地化资源
    List<LocalizationData> GetCurrentModuleLocalizationData()
    {
        const string ASSETS_PATH = "Assets/Module/8.Localization/asset/";
        List<LocalizationData> dataList = new List<LocalizationData>();

        //获取通用资源
        dataList.AddRange(FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(ASSETS_PATH));

        // 从当前 GameObject 开始往上查找 prefab 文件夹

        string rootPrefabPath = GetPrefabRootGameObjectPath();
        Debug.Log($"=== UI Localization: prefabRoot, 当前本地化文件的根Prefab Path: {rootPrefabPath} ===");
        string moduleLocPath = GetModuleLocPath(rootPrefabPath);
        Debug.Log($"=== UI Localization: moduleLocPath, 当前本地化文件的模块Loc Path: {moduleLocPath} ===");

        if (!string.IsNullOrEmpty(moduleLocPath))
        {
            dataList.AddRange(FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(moduleLocPath));
        }

        return dataList;
    }

    //获取当前本地化文件的根Prefab Path
    string GetPrefabRootGameObjectPath()
    {
         string path = "";

        // 1. 先尝试获取预制体实例根
        var root = PrefabUtility.GetNearestPrefabInstanceRoot(gameObject);
        if (root != null)
        {
            // 获取预制体资源路径
            var source = PrefabUtility.GetCorrespondingObjectFromSource(root);
            if (source != null)
            {
                path = AssetDatabase.GetAssetPath(source);
            }
            return path;
        }

        // 2. 如果在预制体编辑模式
        var stage = PrefabStageUtility.GetCurrentPrefabStage();
        if (stage != null)
        {
            path = stage.assetPath;
            return path;
        }

        // 3. 如果都不是，返回空字符串
        return "";

    }
    
    //获取当前本地化文件的模块Loc Path
    //路径里把prefab后面的都删除，然后替换成loc
    string GetModuleLocPath(string rootPrefabPath)
    {
        string p = rootPrefabPath.Replace('\\', '/');
        int i = p.IndexOf("/prefab", StringComparison.Ordinal);
        if (i < 0)
        {
            Debug.LogError($"ToLocFolder: 找不到 'prefab' 关键词，路径: {p}");
            return "";
        }
        return p.Substring(0, i) + "/loc";
    }
#endif
}

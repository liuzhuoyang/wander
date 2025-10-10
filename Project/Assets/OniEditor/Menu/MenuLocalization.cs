#if UNITY_EDITOR
using Sirenix.OdinInspector;
using OpenAI_API;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuLocalization
{
    /*
    [BoxGroup("创建本地化资源")]
    [Button("创建本地化资源", ButtonSizes.Gigantic)]
    void CreateLocalizationUIGroup()
    {
        ScriptableObjectCreator.ShowDialog<LocalizationData>(EditorPathUtility.localizationPath, obj => { });
    }


    [BoxGroup("输出CSV")]
    [Button("输出UI本地化CSV", ButtonSizes.Gigantic)]
    public void ExportCSVUI()
    {
        ExportLocalizationCSVForLanguages("ui");
    }

    [BoxGroup("输出CSV")]
    [Button("输出剧本本地化CSV", ButtonSizes.Gigantic)]
    public void ExportCSVDialogUI()
    {
        ExportLocalizationCSVForLanguages("plot");
    }*/

    private void ExportLocalizationCSVForLanguages(string folderName)
    {
        LanguageType[] languages =
        {
        LanguageType.en, LanguageType.ja, LanguageType.ko, LanguageType.zhs,
        LanguageType.zht, LanguageType.fr, LanguageType.de, LanguageType.es,
        LanguageType.pt, LanguageType.it, LanguageType.nl, LanguageType.ru,
        LanguageType.th
        };

        Dictionary<LanguageType, string> languageMessages = new Dictionary<LanguageType, string>
        {
            { LanguageType.en, "=== 导出英文本地化CSV ===" },
            { LanguageType.ja, "=== 导出日语本地化CSV ===" },
            { LanguageType.ko, "=== 导出韩语本地化CSV ===" },
            { LanguageType.zhs, "=== 导出简体中文地化CSV ===" },
            { LanguageType.zht, "=== 导出繁体中文地化CSV ===" },
            { LanguageType.fr, "=== 导出法语本地化CSV ===" },
            { LanguageType.de, "=== 导出德语本地化CSV ===" },
            { LanguageType.es, "=== 导出西班牙语本地化CSV ===" },
            { LanguageType.pt, "=== 导出葡萄牙语本地化CSV ===" },
            { LanguageType.it, "=== 导出意大利语本地化CSV ===" },
            { LanguageType.nl, "=== 导出荷兰语本地化CSV ===" },
            { LanguageType.ru, "=== 导出俄语本地化CSV ===" },
            { LanguageType.th, "=== 导出泰语本地化CSV ===" },
        };

        foreach (var lang in languages)
        {
            List<LocalizationSerializedItem> list = new List<LocalizationSerializedItem>();
            List<LocalizationData> listGroup = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>("Assets/Moduel/8.Localization/asset/" + folderName);
            foreach (LocalizationData group in listGroup)
            {
                foreach (LocalizationSerializedItem item in group.list)
                {
                    //item.UpdateContent(lang);
                    list.Add(item);
                }
            }
            UpdateAndWriteCSV(list, $"localization_{lang.ToString().ToLower()}_{folderName}.csv", lang);

            if (languageMessages.ContainsKey(lang))
                Debug.Log(languageMessages[lang]);
        }
    }

    private void UpdateAndWriteCSV(List<LocalizationSerializedItem> list, string fileName, LanguageType language)
    {
        string path = Application.dataPath + "/AddressableRemote/localization/" + language.ToString().ToLower() + "/" + fileName;
        CSVWriter.WriteLocalization(path, list);
    }

    [BoxGroup("本地化文件")]
    [Button("查找本地化管理文件", ButtonSizes.Gigantic)]
    public void GetAllLocalizationAssetFile()
    {
        string assetPath = "Assets/Module/8.Localization/asset/all_localization.asset";
        var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

        if (asset != null)
        {
            Debug.Log("Asset found: " + assetPath);
            Selection.activeObject = asset; // 在 Unity 编辑器中选中该文件
        }
        else
        {
            Debug.LogError("Asset not found: " + assetPath);
        }
    }

    [BoxGroup("ChatGPT")]
    [InfoBox("点击后等待console输出初始化完毕提示")]
    [Button("Init Chat GPT", ButtonSizes.Gigantic)]
    public async void InitGPT()
    {
        // This line gets your API key (and could be slightly different on Mac/Linux)
        //api = new OpenAIAPI(Environment.GetEnvironmentVariable("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", EnvironmentVariableTarget.User));
        Debug.Log("=== 开始连接 Chat GPT ===");
        ChatGPT.api = new OpenAI_API.OpenAIAPI("sk-0lZmHEhB0zRNRqGL313b103a707f46B69fFb6546D978242c");
        var result = await ChatGPT.api.Completions.GetCompletion("One Two Three One Two");
        Debug.Log("=== 成功连接 Chat GPT ===");
        //StartConversation();
        //GetResponse();
        //okButton.onClick.AddListener(() => GetResponse());
    }
}
#endif
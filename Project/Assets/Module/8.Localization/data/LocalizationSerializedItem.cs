using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class LocalizationSerializedItem
{
    const string PATH = "Assets/Module/8.Localization/asset/";

    bool isTranslatingAll; //是否正在翻译所有语言，阻挡多次操作用
    [HideInInspector]
    //public string content;
    public string GetContent(LanguageType language)
    {
        
        switch (language)
        {
            case LanguageType.en:
                return textEnglish;
            case LanguageType.zhs:
                return textSimpleChinese;
            case LanguageType.zht:
                return textTraditionChinese;
            case LanguageType.ja:
                return textJapanese;
            case LanguageType.ko:
                return textKorean;
            case LanguageType.fr:
                return textFrench;
            case LanguageType.de:
                return textGerman;
            case LanguageType.pt:
                return textPortuguese;
            case LanguageType.es:
                return textSpanish;
            case LanguageType.ru:
                return textRussian;
            case LanguageType.it:
                return textItalian;
            case LanguageType.nl:
                return textDutch;
            case LanguageType.th:
                return textThai;
            default:
                return textEnglish;
        }
    }

    [TextArea(0, 1)]
    [VerticalGroup("Top")]
    [HorizontalGroup("Top/Box")]
    [HideLabel]
    [SuffixLabel("Key", Overlay = true)]
    [GUIColor(1f, 1f, 1f, 0.6f)]
    public string key;

    [VerticalGroup("Bot")]
    [HorizontalGroup("Bot/Box")]
    [HideLabel, TextArea(0, 1)]
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public string textEnglish;

    [HorizontalGroup("Bot/Box", Width = 150)]
    [HideLabel, TextArea(0, 1)]
    [SuffixLabel("AI提示词", Overlay = true)]
    public string textHint;

    [VerticalGroup("Mapping")]
    [ShowIf("isMapping")]
    [ValueDropdown("GetMappingList")]
    public List<string> mappingList;

    [HorizontalGroup("Top/Box", Width = 120)]
    [VerticalGroup("Top/Box/Toggle")]
    [LabelWidth(80)]
    public bool allLanguage;

    [VerticalGroup("Top/Box/Toggle")]
    [LabelWidth(80)]
    public bool isMapping;
    
    [HorizontalGroup("Bot/Box", Width = 120)]
    [LabelWidth(80)]
    [Button("全翻译", ButtonHeight = 34)]
    [DisableIf("isTranslatingAll")]
    public async void OnTranslateAll()
    {
        isTranslatingAll = true;
        ToSimpleseChinese();
        await Task.Delay(100); // 等待0.1秒避免太快的请求
        ToTraditionalChinese();
        await Task.Delay(100); 
        ToJapanese();
        await Task.Delay(100);
        ToKorean();
        await Task.Delay(100);  
        ToFrench();
        await Task.Delay(100);
        ToGerman();
        await Task.Delay(100);
        ToSpanish();
        await Task.Delay(100);
        ToPortuguese();
        await Task.Delay(100);
        ToItalian();
        await Task.Delay(100);
        ToDutch();
        await Task.Delay(100);
        ToRussian();
        await Task.Delay(100);
        ToThai();
        isTranslatingAll = false;

        Debug.Log($"=== LocalizationSerializedItem: OnTranslateAll Finished {isTranslatingAll}===");
    }

#region 翻译

    [VerticalGroup("Language")]
    [HorizontalGroup("Language/ChineseHans")]
    [HideLabel, SuffixLabel("简体中文", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textSimpleChinese;
    bool isTranslatingSimpleChinese;

    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/ChineseHans", Width = 120)]
    [Button("zhs")]
    [DisableIf("isTranslatingSimpleChinese")]
    public async void ToSimpleseChinese()
    {
        isTranslatingSimpleChinese = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.zhs, (response) => { 
            isTranslatingSimpleChinese = false;
            textSimpleChinese = response;
        });
        Debug.Log($"Simple Chinese Finished {isTranslatingSimpleChinese}");
    }
    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/ChineseHant")]
    [HideLabel, SuffixLabel("繁体中文", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textTraditionChinese;

    bool isTranslatingTraditionalChinese;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/ChineseHant", Width = 120)]
    [Button("zht")]
    [DisableIf("isTranslatingTraditionalChinese")]
    public async void ToTraditionalChinese()
    {
        isTranslatingTraditionalChinese = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.zht, (response) => { 
            isTranslatingTraditionalChinese = false;
            textTraditionChinese = response;
        });
        Debug.Log($"Traditional Chinese Finished {isTranslatingTraditionalChinese}");
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Japanese")]
    [HideLabel, SuffixLabel("日语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textJapanese;

    bool isTranslatingJapanese;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Japanese", Width = 120)]
    [Button("ja")]
    [DisableIf("isTranslatingJapanese")]
    public async void ToJapanese()
    {
        isTranslatingJapanese = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.ja, (response) => { 
            isTranslatingJapanese = false;
            textJapanese = response;
        });
        Debug.Log($"Japanese Finished {isTranslatingJapanese}");
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Korean")]
    [HideLabel, SuffixLabel("韩文", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textKorean;

    bool isTranslatingKorean;

    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Korean", Width = 120)]
    [Button("ko")]
    [DisableIf("isTranslatingKorean")]
    public async void ToKorean()
    {
        isTranslatingKorean = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.ko, (response) => { 
            isTranslatingKorean = false;
            textKorean = response;
        });
        Debug.Log($"Korean Finished {isTranslatingKorean}");
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/French")]
    [HideLabel, SuffixLabel("法语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textFrench;

    bool isTranslatingFrench;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/French", Width = 120)]
    [Button("fr")]
    [DisableIf("isTranslatingFrench")]
    public async void ToFrench()
    {   
        isTranslatingFrench = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.fr, (response) => { 
            isTranslatingFrench = false;
            textFrench = response;
        });
        Debug.Log($"French Finished {isTranslatingFrench}");
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/German")]
    [HideLabel, SuffixLabel("德语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textGerman;

    bool isTranslatingGerman;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/German", Width = 120)]
    [Button("de")]
    [DisableIf("isTranslatingGerman")]
    public async void ToGerman()
    {
        isTranslatingGerman = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.de, (response) => { 
            isTranslatingGerman = false;
            textGerman = response;
        });
        Debug.Log($"German Finished {isTranslatingGerman}");
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Spanish")]
    [HideLabel, SuffixLabel("西班牙语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textSpanish;

    bool isTranslatingSpanish;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Spanish", Width = 120)]
    [Button("es")]
    [DisableIf("isTranslatingSpanish")]
    public async void ToSpanish()
    {
        isTranslatingSpanish = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.es, (response) => { 
            isTranslatingSpanish = false;
            textSpanish = response;
        });
        Debug.Log($"Spanish Finished {isTranslatingSpanish}");
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Portuguese")]
    [HideLabel, SuffixLabel("葡萄牙语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textPortuguese;

    bool isTranslatingPortuguese;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Portuguese", Width = 120)]
    [Button("pt")]
    [DisableIf("isTranslatingPortuguese")]
    public async void ToPortuguese()
    {
        isTranslatingPortuguese = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.pt, (response) => { 
            isTranslatingPortuguese = false;
            textPortuguese = response;
        });
        Debug.Log($"Portuguese Finished {isTranslatingPortuguese}");
        //ChatGPT.StartConversation("Portuguese");
        //ChatGPT.GetResponse(textEnglish, LanguageType.pt, (response) => { textPortuguese = response; });
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Italian")]
    [HideLabel, SuffixLabel("意大利语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textItalian;

    bool isTranslatingItalian;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Italian", Width = 120)]
    [Button("it")]
    [DisableIf("isTranslatingItalian")]
    public async void ToItalian()
    {
        isTranslatingItalian = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.it, (response) => { 
            isTranslatingItalian = false;
            textItalian = response;
        });
        Debug.Log($"Italian Finished {isTranslatingItalian}");
        //ChatGPT.StartConversation("Italian");
        //ChatGPT.GetResponse(textEnglish, LanguageType.it, (response) => { textItalian = response; });
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Dutch")]
    [HideLabel, SuffixLabel("荷兰语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textDutch;

    bool isTranslatingDutch;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Dutch", Width = 120)]
    [Button("nl")]
    [DisableIf("isTranslatingDutch")]
    public async void ToDutch()
    {
        isTranslatingDutch = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.nl, (response) => { 
            isTranslatingDutch = false;
            textDutch = response;
        });
        Debug.Log($"Dutch Finished {isTranslatingDutch}");
        //ChatGPT.StartConversation("Dutch");
        //ChatGPT.GetResponse(textEnglish, LanguageType.nl, (response) => { textDutch = response; });
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Russian")]
    [HideLabel, SuffixLabel("俄语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textRussian;

    bool isTranslatingRussian;
    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Russian", Width = 120)]
    [Button("ru")]
    [DisableIf("isTranslatingRussian")]
    public async void ToRussian()
    {
        isTranslatingRussian = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.ru, (response) => { 
            isTranslatingRussian = false;
            textRussian = response;
        });
        Debug.Log($"Russian Finished {isTranslatingRussian}");
        //ChatGPT.StartConversation("Russian");
        //ChatGPT.GetResponse(textEnglish, LanguageType.ru, (response) => { textRussian = response; });
    }

    [TableColumnWidth(30)]
    [VerticalGroup("Language")]
    [HorizontalGroup("Language/Thai")]
    [HideLabel, SuffixLabel("泰语", Overlay = true)]
    [TextArea(0, 1)]
    [ShowIf("allLanguage")]
    public string textThai; 

    bool isTranslatingThai;

    [ShowIf("allLanguage")]
    [HorizontalGroup("Language/Thai", Width = 120)]
    [Button("th")]
    [DisableIf("isTranslatingThai")]
    public async void ToThai()
    {
        isTranslatingThai = true;
        await DeepSeekAPI.StartTranslationTask(textEnglish, textHint, LanguageType.th, (response) => { 
            isTranslatingThai = false;
            textThai = response;
        });
        Debug.Log($"Thai Finished {isTranslatingThai}");
        //ChatGPT.StartConversation("Thai");
        //ChatGPT.GetResponse(textEnglish, LanguageType.th, (response) => { textThai = response; });
    }
#endregion

#if UNITY_EDITOR
    public List<string> GetMappingList()
    {

        if (!isMapping) return null;
        List<string> listKey = new List<string>();
        LocalizationDataCollection listDataCollection = LocalizationDataCollection.GetData();
        foreach (LocalizationData asset in listDataCollection.listLocalizationDataMapping)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
 #endif
}

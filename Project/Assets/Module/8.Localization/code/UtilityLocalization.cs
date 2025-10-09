
using System;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityLocalization
{
    // 核心方法：处理映射和格式化
    static string ProcessLocalizationArgs(LocalizationArgs args, string key, params string[] dynamicValues)
    {
        if (string.IsNullOrEmpty(args.content))
        {
            Debug.LogWarning($"=== UI Localization: 空内容，key: {key} ===");
            return args.content ?? "";
        }

        if (args.mappingList != null && args.mappingList.Count > 0)
        {
            try
            {
                // 获取映射内容
                string[] mappingContent = GetMappingContent(args.mappingList);
                
                // 合并动态值
                string[] allValues = CombineValues(mappingContent, dynamicValues);
                
                return string.Format(args.content, allValues);
            }
            catch (Exception e)
            {
                Debug.LogError($"=== UI Localization: 处理映射失败，key: {key}, 错误: {e.Message} ===");
                return args.content;
            }
        }
        
        // 没有映射，直接格式化动态值
        return dynamicValues != null && dynamicValues.Length > 0 ? 
            string.Format(args.content, dynamicValues) : 
            args.content;
    }

    // 获取映射内容
    static string[] GetMappingContent(List<string> mappingList)
    {
        string[] mappingContent = new string[mappingList.Count];
        for (int i = 0; i < mappingList.Count; i++)
        {
            mappingContent[i] = GetLocalization(mappingList[i]);
        }
        return mappingContent;
    }

    // 合并值
    static string[] CombineValues(string[] mappingContent, string[] dynamicValues)
    {
        if (dynamicValues == null || dynamicValues.Length == 0)
            return mappingContent;
            
        string[] combined = new string[mappingContent.Length + dynamicValues.Length];
        mappingContent.CopyTo(combined, 0);
        dynamicValues.CopyTo(combined, mappingContent.Length);
        return combined;
    }

    // 通用获取方法
    static string GetLocalizationFromDict(Dictionary<string, LocalizationArgs> dict, string key, params string[] dynamicValues)
    {
        if (dict.TryGetValue(key, out LocalizationArgs args))
        {
            return ProcessLocalizationArgs(args, key, dynamicValues);
        }

        Debug.LogError($"=== LocalizationUtility: 找不到 key : {key} 请检查本地化资源 ===");
        return "";
    }

    #region 公开接口
    // 获取本地化值
    public static string GetLocalization(string key)
    {
        // 先检查常规本地化数据
        if (AllLocalization.dictLocalizationData.TryGetValue(key, out LocalizationArgs args))
        {
            return ProcessLocalizationArgs(args, key);
        }
        // 再检查Mapping的本地化数据
        else if (AllLocalization.dictLocalizationDataMapping.TryGetValue(key, out LocalizationArgs argsMapping))
        {
            return ProcessLocalizationArgs(argsMapping, key);
        }
        // 如果都没有，说明不存在这个key
        else
        {
            Debug.LogError($"=== LocalizationUtility: 找不到 key : {key} ===");
            return "";
        }
    }

    // 获取动态本地化值
    public static string GetLocalization(string key, params string[] dynamicParams)
    {
        return GetLocalizationFromDict(AllLocalization.dictLocalizationData, key, dynamicParams);
    }

    // 获取剧情本地化值
    public static string GetPlotLocalization(string key, params string[] dynamicParams)
    {
        return GetLocalizationFromDict(AllLocalization.dictLocalizationDataPlot, key, dynamicParams);
    }

    // 获取系统语言代码
    public static string GetSystemLanguageCode()
    {
        string language = PlayerPrefs.GetString("loc");
        Debug.Log("=== Utility : GetUserLanguageSetting:" + language + " ===");
        if (string.IsNullOrEmpty(language))
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            Debug.Log("=== Utility : SystemLanguage:" + systemLanguage.ToString() + " ===");
            switch (systemLanguage)
            {
                case SystemLanguage.Chinese:
                    language = ConstantLocKey.LANGUAGE_ZHT;
                    break;
                case SystemLanguage.ChineseSimplified:
                    language = ConstantLocKey.LANGUAGE_ZHS;
                    break;
                case SystemLanguage.ChineseTraditional:
                    language = ConstantLocKey.LANGUAGE_ZHT;
                    break;
                case SystemLanguage.Japanese:
                    language = ConstantLocKey.LANGUAGE_JA;
                    break;
                case SystemLanguage.Korean:
                    language = ConstantLocKey.LANGUAGE_KO;
                    break;
                case SystemLanguage.French:
                    language = ConstantLocKey.LANGUAGE_FR;
                    break;
                case SystemLanguage.German:
                    language = ConstantLocKey.LANGUAGE_DE;
                    break;
                case SystemLanguage.Spanish:
                    language = ConstantLocKey.LANGUAGE_ES;
                    break;
                case SystemLanguage.Italian:
                    language = ConstantLocKey.LANGUAGE_IT;
                    break;
                case SystemLanguage.Portuguese:
                    language = ConstantLocKey.LANGUAGE_PT;
                    break;
                case SystemLanguage.Russian:
                    language = ConstantLocKey.LANGUAGE_RU;
                    break;
                case SystemLanguage.Dutch:
                    language = ConstantLocKey.LANGUAGE_NL;
                    break; ;
                case SystemLanguage.Thai:
                    language = ConstantLocKey.LANGUAGE_TH;
                    break;
                default:
                    language = ConstantLocKey.LANGUAGE_EN;
                    break;
            }
            PlayerPrefs.SetString("loc", language);
            Debug.Log("=== Utility : SetUserLanguage:" + language + " ===");
        }
        return language;
    }

    // 检查语言代码是否支持
    public static string CheckContainLanguage(string languageCode)
    {
        List<string> list = new List<string> { "zh", "de", "es", "en", "fr", "it", "ja", "ko", "nl", "pt", "ru", "th", };
        if (list.Contains(languageCode))
        {
            Debug.Log("===== set language: " + languageCode + " =====");
            if (languageCode == "zh")
            {
                SystemLanguage systemLanguage = Application.systemLanguage;
                if (systemLanguage == SystemLanguage.ChineseTraditional)
                {
                    return "zht";
                }
                else
                {
                    return "zhs";
                }
            }
            else
            {
                Debug.Log($"User's system language is not Chinese. Language code: {languageCode}");
                return languageCode;
            }
        }
        Debug.Log("===== user language: " + languageCode + " not support, set to English =====");
        return "en";
    }

    // 获取字体名称
    public static string GetFontName()
    {
        string fontName = "font_" + GetFont(PlayerPrefs.GetString("loc"));
        return fontName;
    }

    // 获取字体
    public static string GetFont(string languageCode)
    {
        List<string> listCustom = new List<string> { "zhs", "zht", "ja", "ko", "th", };
        List<string> listLatin = new List<string> { "de", "es", "fr", "it", "nl", "pt", "ru" };
        if (listLatin.Contains(languageCode))
            return "latin";
        else if (listCustom.Contains(languageCode))
            return languageCode;
        else
            return "en";
    }
    #endregion

    #region 常用动态本地化，比如等级，稀有度等
    //获取等级
    public static string GetDynamicLevelText(int level)
    {
        return string.Format(GetLocalization(ConstantLocKey.DYNAMIC_LEVEL_X), level);
    }
    #endregion
}

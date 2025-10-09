using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;

public static class DeepSeekAPI
{
    private static readonly string AIP_URL = "https://tbnx.plus7.plus/v1/chat/completions";
    //private static readonly string apiUrl = "https://tbnx.plus7.plus/v1";
    const string DEEPSEEK_MODEL = "deepseek-chat";
    const string API_KEY= "sk-p6bXFSj55j8h9ufHFGqm5mYs52YKD6uXE6UdvZX5Ds6je5Ct";
    private static HttpClient httpClient;

    static DeepSeekAPI()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
        httpClient.Timeout = TimeSpan.FromSeconds(30); // 设置30秒超时
    }

    // 开始翻译任务
    public static async Task StartTranslationTask(string textEnglish, string textHint, LanguageType language, Action<string> onSucceed)
    {
        Debug.Log("=== DeepseekAPI: StartTranslationTask ===");
        string hintString = textHint == null ? "" : $"（{textHint}，括号内是给你的提示，当英语有多种翻译时候用哪个意思，不需要翻译）";
        string prompt = $"把英文翻译成 {GetLanguageName(language)}，只要返回给简短的答案，不需要任何额外解释: {textEnglish} {hintString}";
        
        try
        {
            string response = await SendRequest(prompt);
            Debug.Log("=== DeepseekAPI: response ===" + response);
            onSucceed?.Invoke(response);
        }
        catch (TaskCanceledException)
        {
            Debug.LogError("请求超时");
            onSucceed?.Invoke("");//请求超时，请重试
        }
        catch (Exception ex)
        {
            Debug.LogError($"请求失败: {ex.Message}");
            onSucceed?.Invoke("");//请求失败，检查网络连接
        }
    }

    // 给到DeepSeek的详细翻译语言
    static string GetLanguageName(LanguageType language)
    {
        switch (language)
        {
            case LanguageType.zhs: return "简体中文";
            case LanguageType.zht: return "繁体中文";
            case LanguageType.ja: return "日文";
            case LanguageType.ko: return "韩文";
            case LanguageType.fr: return "法文";
            case LanguageType.de: return "德文";
            case LanguageType.es: return "西班牙文";
            case LanguageType.pt: return "葡萄牙文";
            case LanguageType.it: return "意大利文";
            case LanguageType.nl: return "荷兰文";
            case LanguageType.ru: return "俄文";
            case LanguageType.th: return "泰文";
            default: return "英语";
        }
    }

    // 发送请求
    public static async Task<string> SendRequest(string prompt)
    {

        var requestBody = $"{{\"model\": \"{DEEPSEEK_MODEL}\", \"messages\": [{{\"role\": \"user\", \"content\": \"{prompt}\"}}]}}";
        var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(AIP_URL, content);


         // 解析JSON并提取content字段
        var responseJson = await response.Content.ReadAsStringAsync();
        var responseData = JsonUtility.FromJson<DeepseekResponse>(responseJson);
        return responseData.choices[0].message.content;
    }

    // 定义JSON解析的类结构
    [System.Serializable]
    private class DeepseekResponse
    {
        public string id;
        public string @object;
        public int created;
        public string model;
        public Choice[] choices;
        public Usage usage;
        public string system_fingerprint;
    }

    [System.Serializable]
    private class Choice
    {
        public int index;
        public Message message;
        public string finish_reason;
        public ContentFilterResults content_filter_results;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class ContentFilterResults
    {
        public FilterResult hate;
        public FilterResult self_harm;
        public FilterResult sexual;
        public FilterResult violence;
        public FilterResult jailbreak;
        public FilterResult profanity;
    }

    [System.Serializable]
    private class FilterResult
    {
        public bool filtered;
        public bool detected;
    }

    [System.Serializable]
    private class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
        public object prompt_tokens_details;
        public object completion_tokens_details;
    }
}


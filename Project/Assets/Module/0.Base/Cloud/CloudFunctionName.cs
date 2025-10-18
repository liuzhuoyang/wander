using System;
using System.Collections.Generic;

public class CloudFunctionNames
{
    // 定义所有函数名称为常量
    //基础
    public const string F_TOKEN = "F_TOKEN";
    //public const string F_GET_TIME = "F_GET_TIME";
    public const string F_VERSION = "F_VERSION";
    public const string F_ACCOUNT = "F_ACCOUNT";
    public const string F_SERVER = "F_SERVER";
    public const string F_PROGRESS_SYNC = "F_PROGRESS_SYNC";
    public const string F_ACCESS = "F_ACCESS";
    //public const string F_TEST = "F_TEST";

    //业务
    public const string F_RANK = "F_RANK";
    public const string F_MAIL = "F_MAIL";
    public const string F_GROUP_AB = "F_GROUP_AB";
    public const string F_LEVEL = "F_LEVEL";

    // 其他函数名可以继续定义...
}

public class CloudFunctionAPI
{
    private static readonly Dictionary<string, string> functionUrls = new Dictionary<string, string>()
    {
        {CloudFunctionNames.F_ACCESS, "https://wander-dcdn.onicore.host/access"},
        { CloudFunctionNames.F_TOKEN, "https://token-dcdn.onicore.host/wander_token"},
        //{CloudFunctionNames.F_GET_TIME, "https://space-dcdn.onicore.host/time"},
        {CloudFunctionNames.F_VERSION, "https://space-dcdn.onicore.host/version"},
        {CloudFunctionNames.F_ACCOUNT, "https://wander-dcdn.onicore.host/account"},
        {CloudFunctionNames.F_PROGRESS_SYNC, "https://wander-dcdn.onicore.host/progress"},
        {CloudFunctionNames.F_MAIL, "https://wander-dcdn.onicore.host/mail"},
        {CloudFunctionNames.F_GROUP_AB, "https://wander-dcdn.onicore.host/config_trigger"},
        {CloudFunctionNames.F_LEVEL, "https://wander-dcdn.onicore.host/config_level"},
        //{CloudFunctionNames.F_TEST, "https://wander-dcdn.onicore.host/test"},
    };


    // 配置一个开关，决定是否使用 CDN
    private static bool useCDN = false;  // 默认不使用 CDN

    // 提供外部设置是否使用 CDN 的方法
    public static void SetCDNUsage(bool enableCDN)
    {
        useCDN = enableCDN;
    }

    // 通过枚举获取接口 URL
    public static string GetFunctionUrl(string functionName)
    {
        if (functionUrls.ContainsKey(functionName))
        {
            //var urls = functionUrls[functionName];
            //return useCDN ? urls.cdnUrl : urls.normalUrl;
            return functionUrls[functionName];
        }
        else
        {
            throw new ArgumentException($"Function {functionName} not found.");
        }
    }
}

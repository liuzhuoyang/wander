using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;

public static class ListAssetsParser
{
    const char SPLIT_CHAR = '&';
    const char PARAM_CHAR = '^';
    public delegate string Encoder<T>(T element);
    public delegate T Decoder<T>(string str);
//将枚举类型按指定规则合并为字符列表
    public static string IEnumerableToListedString<T>(IEnumerable<T> elements, Encoder<T> processor)
    {
        string result = string.Empty;
        foreach(var item in elements){
            result += processor(item) + SPLIT_CHAR;
        }

    //字符不为空
        if(!string.IsNullOrEmpty(result))
            result = result.Remove(result.Length - 1); //移除最后一个分隔符
        return result;
    }
//将整个字符串分割为字符列表
    public static List<string> ParseListedString(string str){
        List<string> list = new List<string>();
        if(!string.IsNullOrEmpty(str)){
            string[] array = str.Split(SPLIT_CHAR);
            foreach(var item in array){
                list.Add(item);
            }
        }
        return list;
    }
    public static List<T> ParseListedString<T>(string str, Decoder<T> decoder)
    {
        List<T> list = new List<T>();
        if(!string.IsNullOrEmpty(str))
        {
            string[] array = str.Split(SPLIT_CHAR);
            foreach(var item in array)
            {
                list.Add(decoder(item));
            }
        }
        return list;
    }
//Json 转换
    public static string SerializeObject<T>(T go){
        string str = JsonConvert.SerializeObject(go).ToString();
        str = str.Replace(',', PARAM_CHAR);
        return str;
    }
    public static T DeserializeObject<T>(string jsonStr)
    {
        string str = jsonStr;
        // //匹配首尾是双引号内的内容
        // Match m = Regex.Match(jsonStr, "^\"(.*?)\"$");
        // //替换两个双引号为一个双引号
        // if(m.Success)
        //     str = m.Result("${1}").Replace("\"\"", "\"");
        // else
        //     str = str.Replace("\"\"", "\"");

        //删除首尾的双引号，若存在的话，但双引号是CSVWriter添加，
        //@Todo更合理的方式是由CSVLoader去剔除，谁添加谁减少的原则
        str = str.Trim('\"');
        str = str.Replace("\"\"", "\"");
        str = str.Replace(PARAM_CHAR, ',');

        return JsonConvert.DeserializeObject<T>(str);
    }
}
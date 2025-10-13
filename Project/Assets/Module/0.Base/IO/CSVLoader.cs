using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

public class CSVLoader : Singleton<CSVLoader>
{
    #region 读取 CSV
    // 方案：使用字典进行类型转换
    private static readonly Dictionary<Type, Func<string, object>> TypeConversionMap = new Dictionary<Type, Func<string, object>>
    {
        { typeof(bool), value => bool.Parse(value) },
        { typeof(int), value => int.Parse(value) },
        { typeof(float), value => float.Parse(value) },
        { typeof(double), value => double.Parse(value) },
        { typeof(string), value => value }
    };

    public async UniTask<Dictionary<TKey, T>> LoadCSV<TKey, T>(string csvFileName, Func<T, TKey> keySelector) where T : new()
    {
        Dictionary<TKey, T> result = new Dictionary<TKey, T>();

        // 动态加载 CSV 文件内容
        string[] csvData = await LoadAddressableAssetsAsync(csvFileName);

        // 确保 CSV 数据已经加载
        if (csvData == null || csvData.Length == 0)
        {
            Debug.LogError($"=== CSVLoader: Failed to load CSV data from {csvFileName} ===");
            return result;
        }

        // 获取表头并缓存字段映射
        string[] headers = csvData[0].Split(new char[] { ',' });
        var fieldMap = new Dictionary<string, FieldInfo>();

        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var header in headers)
        {
            var field = fields.FirstOrDefault(f => f.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
            if (field != null)
            {
                fieldMap.Add(header, field);
            }
        }

        // 遍历每一行 CSV
        for (int i = 1; i < csvData.Length; i++)
        {
            string[] row = csvData[i].Split(new char[] { ',' });

            // 创建泛型对象 T
            T obj = new T();

            // 根据缓存的字段映射进行赋值
            for (int j = 0; j < headers.Length && j < row.Length; j++)
            {
                if (fieldMap.TryGetValue(headers[j], out var field))
                {
                    var fieldType = field.FieldType;
                    try
                    {
                        if (fieldType.IsEnum && Enum.IsDefined(fieldType, row[j]))
                        {
                            field.SetValue(obj, Enum.Parse(fieldType, row[j]));
                        }
                        else if (TypeConversionMap.TryGetValue(fieldType, out var convertFunc))
                        {
                            field.SetValue(obj, convertFunc(row[j]));
                        }
                        else
                        {
                            field.SetValue(obj, Convert.ChangeType(row[j], fieldType));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"=== CSVLoader: Error parsing field '{field.Name}' in row {i + 1}: {ex.Message} ===");
                    }
                }
            }

            // 使用 keySelector 通过 obj 的某个字段动态获取键
            TKey key = keySelector(obj);

            // 添加到字典
            if (!result.ContainsKey(key))
            {
                result.Add(key, obj);
            }
            else
            {
                Debug.LogError($"=== CSVLoader: csv文件有错误，有重复的Key，FileName: {csvFileName}. Duplicate key found: {key} at row {i + 1}.  ===");
            }
        }

        Debug.Log($"=== CSVLoader: {result.Count} csv item loaded from {csvFileName} ===");

        return result;
    }
    #endregion

    #region 本地化数据
    public async UniTask<Dictionary<string, LocalizationArgs>> LoadLocalization(string targetCSV)
    {
        string language = UtilityLocalization.GetSystemLanguageCode();

        string[] stream = await LoadAddressableAssetsAsync(string.Format("localization_{0}_{1}", language, targetCSV));

        // 创建一个字典用于存储键值对，键是字符串，值也是字符串
        Dictionary<string, LocalizationArgs> dict = new Dictionary<string, LocalizationArgs>();

        int count = stream.Length;
        // 从第二行开始遍历（跳过第一行表头），处理每一行数据
        for (int i = 1; i < count; i++) // 从 1 开始，跳过第一行
        {
            string line = stream[i];

            // 检查当前行是否为空行或只有空白字符，如果是则跳过
            if (string.IsNullOrWhiteSpace(line))
            {
                continue; // 跳过该行，继续下一行
            }

            // 使用 ParseCSVLine 方法解析当前行，得到一个字段数组
            string[] row = ParseCSVLine(line);

            // 确保该行至少有两个字段（Key 和 Content）
            if (row.Length >= 2)
            {
                // 获取第一个字段作为键（Key）
                string key = row[0].Trim(); // 去掉前后空白字符

                // 获取第二个字段作为值（Content）
                string content = row[1].Trim(); // 去掉前后空白字符

                string joinedMappingKey = "";
                if(row.Length > 2)
                {
                    joinedMappingKey = row[2].Trim();
                }

                // 检查字典中是否已经存在该键，如果不存在则添加
                if (!dict.ContainsKey(key))
                {
                    LocalizationArgs args = new LocalizationArgs();
                    args.key = key;
                    args.content = content;
                    //args.joinedMapingKey = joinedMappingKey;
                    dict.Add(key, args); // 将键值对加入字典
                }
                else
                {
                    // 如果发现重复的键，输出警告信息
                    Debug.LogError($"Duplicate key found: {key}");
                }
            }
            else
            {
                // 如果当前行没有足够的字段，输出错误信息
                Debug.LogError($"Invalid CSV format at line {i}: {line}");
            }
        }

        Debug.Log($"=== CSVLoader: ui localization {dict.Count} items loaded ===");

        // 返回包含所有解析后键值对的字典
        return dict;
    }

    // 使用正则表达式来处理 CSV 行，支持双引号内的逗号
    public string[] ParseCSVLine(string line)
    {
        // 正则表达式匹配逗号，忽略在双引号内的逗号
        var regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        // 使用正则表达式进行拆分，返回字段数组
        var fields = regex.Split(line);

        // 遍历每个字段，去掉前后可能存在的双引号
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = fields[i].Trim('\"'); // 去掉字段前后的双引号
        }

        // 返回处理完毕的字段数组
        return fields;
    }
    #endregion

    #region 辅助方法
    async UniTask<string[]> LoadAddressableAssetsAsync(string csvFileName)
    {
        Debug.Log("Start loading CSV: " + csvFileName);
        AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(csvFileName);
        TextAsset csvAsset = await handle.Task;

        if (csvAsset == null)
        {
            Debug.LogError($"Failed to load CSV file: {csvFileName}");
            return null;
        }

        // 将 CSV 内容拆分成行
        string[] stream = csvAsset.text.Replace("\r", "").Split(new char[] { '\n' });
        Addressables.Release(handle);
        Debug.Log("Finished loading CSV: " + csvFileName);
        return stream;
    }
    #endregion
}

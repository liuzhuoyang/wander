#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using onicore.editor;
using System.Reflection;
using System.Text;

public static class CSVWriter
{
    /// <summary>
    /// 更新除了Localization 之外所有CSV
    /// </summary>
    public static void OnUpdateCSV()
    {
        WriteCSV(LoadAllAssets<PlotData>(EditorPathUtility.plotAssetPath), "plot_data.csv");
        EditorUtility.DisplayDialog("CSV更新结果", "完成", "ok");
        AssetDatabase.Refresh();
    }

    // 通用资源读取方法，适用于所有类型的资源
    // 传入的excludeFolderList 为要排除的文件夹列表
    public static List<T> LoadAllAssets<T>(string path, List<string> excludeFolderList = null) where T : ScriptableObject
    {
        // 获取所有资源
        var allAssets = new List<T>(FileFinder.FindAllAssetsOfAllSubFolders<T>(path, excludeFolderList));

        return allAssets;
    }

    static void WriteCSV<T>(List<T> dataList, string filePath)
    {
        // 获取类的所有公共字段，包括继承的字段
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        using (TextWriter tw = new StreamWriter(EditorPathUtility.csvPath + filePath, false))
        {
            // 写入表头 (第一个字段是ID，其余是类的字段名称)
            var header = new StringBuilder();
            header.Append("ID,");
            foreach (var field in fields)
            {
                if (IsSupportedType(field.FieldType))
                {
                    // 如果是枚举类型，将字段名的首字母改为大写
                    string fieldName = field.FieldType.IsEnum
                        ? char.ToUpper(field.Name[0]) + field.Name.Substring(1)
                        : field.Name;

                    header.Append(fieldName).Append(",");
                }
            }
            tw.WriteLine(header.ToString().TrimEnd(',')); // 去掉最后一个多余的逗号

            // 写入每行数据，并为每行添加一个ID
            int id = 1; // ID从1开始
            for (int i = 0; i < dataList.Count; i++)
            {
                var item = dataList[i];
                var line = new StringBuilder();
                line.Append(id++).Append(","); // 添加ID列

                foreach (var field in fields)
                {
                    if (IsSupportedType(field.FieldType))
                    {
                        var value = field.GetValue(item) ?? ""; // 如果字段值为 null，则写入空字符串
                        line.Append(EscapeCSV(value.ToString())).Append(",");
                    }
                }

                // 去掉最后一个多余的逗号
                string lineContent = line.ToString().TrimEnd(',');

                // 如果是最后一行，写入时不加换行符
                if (i == dataList.Count - 1)
                {
                    tw.Write(lineContent); // 最后一行不加换行符
                }
                else
                {
                    tw.WriteLine(lineContent); // 非最后一行加换行符
                }
            }
        }

        Debug.Log($"写入 {typeof(T).Name} 数据共 {dataList.Count} 个条目");
    }

    static bool IsSupportedType(Type fieldType)
    {
        // 获取类型代码，根据类型代码判断是否是基础类型
        var typeCode = Type.GetTypeCode(fieldType);

        return typeCode == TypeCode.String ||
               typeCode == TypeCode.Single ||  // float
               typeCode == TypeCode.Int32 ||   // int
               typeCode == TypeCode.Boolean || // bool
               fieldType.IsEnum;               // 支持枚举类型
    }

    #region 写出本地化csv
    public static void WriteLocalization(string filePath, List<LocalizationSerializedItem> dataList)
    {
        if (dataList.Count > 0)
        {
            /*
            using (TextWriter tw = new StreamWriter(filePath, false)) // 直接一次性打开文件
            {
                // 写入表头
                tw.WriteLine("Key, Content, MapingKey");

                for (int i = 0; i < dataList.Count; i++)
                {
                    LocalizationSerializedItem item = dataList[i];


                    // 处理错误情况：内容为空或有前后空格
                    if (!ValidateContent(item, i)) return;

                    // 使用双引号包裹有特殊字符的内容
                    string textContent = EscapeCSV(item.content);

                    string mapingKey = "";
                    if (item.mappingList != null)
                    {
                        mapingKey = string.Join("|", item.mappingList);
                    }
                    // 同样处理 item.key 以防止特殊字符
                    tw.WriteLine($"{EscapeCSV(item.key)},{textContent},{mapingKey}");
                }
            }*/
        }
    }

    // 处理本地化文本内容的验证，包括检查是否为空和前后空格
    private static bool ValidateContent(LocalizationSerializedItem item, int index)
    {
        /*
        if (string.IsNullOrEmpty(item.content))
        {
            Debug.LogError($"没配置本地化文本, 保存失败: {item.key} {index}");
            //DisplayError(item.type, item.key, index, "没配置本地化文本, 保存失败");
            return false;
        }

        if (item.content.Trim().Length != item.content.Length)
        {
            Debug.LogError($"包含前后空格, 保存失败: {item.key} {index}");
            //DisplayError(item.type, item.key, index, "包含前后空格, 保存失败");
            return false;
        }*/

        return true;
    }

    // 显示错误对话框和日志信息
    private static void DisplayError(object type, string key, int index, string message)
    {
        string errorMessage = $"{type} Key: {key} ID: {index} {message}";
        Debug.LogError(errorMessage);
        EditorUtility.DisplayDialog("错误", errorMessage, "ok");
    }

    #endregion

    // 对CSV字段进行转义，处理可能包含逗号、双引号等特殊字符
    static string EscapeCSV(string field)
    {
        if (string.IsNullOrEmpty(field))
            return "";

        // 如果字段包含逗号、双引号或换行符，则使用双引号包裹
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            // 将内部的双引号替换为两个双引号
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }

        return field;
    }
}
#endif
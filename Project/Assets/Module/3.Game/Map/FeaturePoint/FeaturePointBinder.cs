using UnityEngine;
using System;
using Newtonsoft.Json.Serialization;


// 添加自定义的Binder类
public class FeaturePointBinder : ISerializationBinder
{
    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        if (typeof(FeaturePointArgs).IsAssignableFrom(serializedType))
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
        else
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.FullName;
        }
    }

    public Type BindToType(string assemblyName, string typeName)
    {
        if (typeName == "SpawnPointArgs")
            return typeof(SpawnPointArgs);
        if (typeName == "SupplyPointArgs")
            return typeof(SupplyPointArgs);
        if(typeName == "DefenseTowerPointArgs")
            return typeof(DefenseTowerPointArgs);
        return Type.GetType($"{typeName}, {assemblyName}");
    }
}


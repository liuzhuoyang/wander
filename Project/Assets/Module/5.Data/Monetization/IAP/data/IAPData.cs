using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "IapData", menuName = "OniData/Monetization/IAP/IapData", order = 1)]
public class IAPData : ScriptableObject
{
    [ReadOnly]
    public string productID;
    [ValueDropdown("GetSKUList")]
    public SKUData sku;

#if UNITY_EDITOR
    public List<SKUData> GetSKUList()
    {
        string path = GameDataControl.GetAssetPath("all_sku");
        List<SKUData> list = new List<SKUData>();
        List<SKUData> listSKUAsset = AssetsFinder.FindAllAssets<SKUData>(path);
        foreach (SKUData asset in listSKUAsset)
        {
            list.Add(asset);
        }
        return list;
    }

    [ReadOnly]
    public float priceUSD;

    [Button("Init Data", ButtonSizes.Large)]
    public void InitData()
    {
        productID = this.name;
        priceUSD = sku.priceUSD;
    }
#endif
}
using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SKUData", menuName = "OniData/Monetization/SKU/SKUData", order = 1)]
public class SKUData : ScriptableObject
{
    [ReadOnly]
    public string skuID;
    [ReadOnly]
    public float priceUSD;
#if UNITY_EDITOR
    public PriceTag priceTag;

    [Button("Init Data")]
    public void InitData()
    {
        skuID = this.name;
        priceUSD = UtilityMonetization.GetPriceFromTag(priceTag);
    }
#endif
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遗物项
/// </summary>
public class RelicRarityCom : MonoBehaviour
{
    [SerializeField] GameObject objPrefab;
    [SerializeField] Image imgBg;
    [SerializeField] TextMeshProUGUI textRarity, textCount;
    [SerializeField] RectTransform rectTransform;
    public void Init(Rarity rarity, List<RelicSlotViewArgs> data)
    {
        //清空
        foreach (Transform child in rectTransform)
        {
            Destroy(child.gameObject);
        }
        //设置稀有度
        textRarity.text = UtilityLocalization.GetLocalization("generic/rarity/generic_rarity_" + rarity.ToString().ToLower());
        //string bannerName = Utility.GetRarityBannerName(rarity);
        //GameAssetsManager.Instance.AssignSpriteUI(bannerName, imgBg);

        //设置数量
        int allCount = data.Count;
        int unlockCount = data.FindAll(x => x.star != -1).Count;
        textCount.text = $"{unlockCount}/{allCount}";
        foreach (var item in data)
        {
            GameObject obj = Instantiate(objPrefab, rectTransform);
            obj.GetComponent<RelicSlot>().Init(item);
        }
    }
}
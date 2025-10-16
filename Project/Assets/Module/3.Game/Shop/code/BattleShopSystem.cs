using System.Collections.Generic;
using UnityEngine;

public class BattleShopSystem : Singleton<BattleShopSystem>
{

    public List<FormationItemConfig> listFormationItemData;

    public void Init()
    {

    }

    public void StartBattle()
    {
        //生成商店物品的模版
        listFormationItemData = new List<FormationItemConfig>();
        foreach (var item in BattleFormationMangaer.Instance.formationItemDataCollection.GetDataCollection())
        {
            if (item is FormationSkillData)
            {
                listFormationItemData.Add(GetSkillFormationItemConfig(item as FormationSkillData, 1));
                if ((item as FormationSkillData).canUpgrade)
                {
                    listFormationItemData.Add(GetSkillFormationItemConfig(item as FormationSkillData, 2));
                }
            }
            else if (item is FormationGearData)
            {
                listFormationItemData.Add(GetGearFormationItemConfig(item as FormationGearData, 1));
                listFormationItemData.Add(GetGearFormationItemConfig(item as FormationGearData, 2));
            }
        }
    }



    public void RefreshShopItem()
    {
        //通过商店物品的模版，再根据公式计算出刷新的物件是什么,目前随机三个
        List<FormationItemConfig> newListFormationItemData = new List<FormationItemConfig>();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, listFormationItemData.Count);
            // 深拷贝 FormationItemConfig 对象，确保每个商店物品都是独立的数据实例
            newListFormationItemData.Add(DeepCopyFormationItemConfig(listFormationItemData[randomIndex]));
        }

        EventManager.TriggerEvent(EventNameBattleShop.EVENT_REFRESH_BATTLE_SHOP_UI, new UIBattleShopArgs() { listFormationItemData = newListFormationItemData });
    }

    public FormationItemConfig GetSkillFormationItemConfig(FormationSkillData data, int level)
    {
        FormationItemConfig newItemConfig = new FormationItemConfig()
        {
            itemName = data.itemName,
            displayName = data.displayName,
            info = data.info,
            itemType = FormationItemType.Skill,
            coinCost = BattleShopFormula.GetGearCoin(data.coinCost, level),
            AdGet = data.AdGet,
            requiredChargeCount = data.requiredChargeCount,
            hasCooldown = data.hasCooldown,
            cooldownTime = data.cooldownTime,
            itemIcon = data.itemIcon,
            level = level,
        };
        return newItemConfig;
    }

    public FormationItemConfig GetGearFormationItemConfig(FormationGearData data, int level)
    {
        FormationItemConfig newItemConfig = new FormationItemConfig()
        {
            itemName = data.gearData.gearName,
            displayName = data.gearData.displayName,
            info = data.gearData.info,
            itemType = FormationItemType.Gear,
            coinCost = BattleShopFormula.GetGearCoin(data.coinCost, level),
            rarity = data.gearData.rarity,
            itemIcon = data.itemIcon,
            level = level,
        };
        return newItemConfig;
    }

    /// <summary>
    /// 深拷贝 FormationItemConfig 对象，确保每个商店物品都是独立的数据实例
    /// </summary>
    /// <param name="original">原始配置对象</param>
    /// <returns>深拷贝的配置对象</returns>
    private FormationItemConfig DeepCopyFormationItemConfig(FormationItemConfig original)
    {
        FormationItemConfig copy = new FormationItemConfig()
        {
            itemName = original.itemName,
            displayName = original.displayName,
            info = original.info,
            itemType = original.itemType,
            level = original.level,
            coinCost = original.coinCost,
            AdGet = original.AdGet,
            requiredChargeCount = original.requiredChargeCount,
            hasCooldown = original.hasCooldown,
            cooldownTime = original.cooldownTime,
            energyConsumption = original.energyConsumption,
            visualEffectPrefab = original.visualEffectPrefab,
            itemIcon = original.itemIcon
        };

        // 深拷贝效果列表
        copy.effects = new List<FormationEffectData>();
        foreach (var effect in original.effects)
        {
            copy.effects.Add(new FormationEffectData()
            {
                effectType = effect.effectType,
                value = effect.value
            });
        }

        return copy;
    }

}

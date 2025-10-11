
public static class GearUtility
{
    
    public static string GetGearRarityFrameName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "ui_shared_dynamic_slot_gear_common";
            case Rarity.Rare:
                return "ui_shared_dynamic_slot_gear_rare";
            case Rarity.Epic:
                return "ui_shared_dynamic_slot_gear_epic";
            case Rarity.Legendary:
                return "ui_shared_dynamic_slot_gear_legendary";
            case Rarity.Mythic:
                return "ui_shared_dynamic_slot_gear_mythic";
            case Rarity.Arcane:
                return "ui_shared_dynamic_slot_gear_arcane";
            default:
                return "ui_shared_dynamic_slot_gear_common";
        }
    }

    public static string GetGearRarityLightName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "pic_gear_light_common";
            case Rarity.Rare:
                return "pic_gear_light_rare";
            case Rarity.Epic:
                return "pic_gear_light_epic";
            case Rarity.Legendary:
                return "pic_gear_light_legendary";
            case Rarity.Mythic:
                return "pic_gear_light_mythic";
            case Rarity.Arcane:
                return "pic_gear_light_arcane";
            default:
                return "pic_gear_light_common";
        }
    }

    public static string GetGearSkillRarityFrameName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "ui_shared_dynamic_node_gear_skill_common";
            case Rarity.Rare:
                return "ui_shared_dynamic_node_gear_skill_rare";
            case Rarity.Epic:
                return "ui_shared_dynamic_node_gear_skill_epic";
            case Rarity.Legendary:
                return "ui_shared_dynamic_node_gear_skill_legendary";
            case Rarity.Mythic:
                return "ui_shared_dynamic_node_gear_skill_mythic";
            case Rarity.Arcane:
                return "ui_shared_dynamic_node_gear_skill_arcane";
            default:
                return "ui_shared_dynamic_node_gear_skill_common";
        }
    }

    public static string GetGearRarityBannerName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "ui_gear_slot_banner_common";
            case Rarity.Rare:
                return "ui_gear_slot_banner_rare";
            case Rarity.Epic:
                return "ui_gear_slot_banner_epic";
            case Rarity.Legendary:
                return "ui_gear_slot_banner_legendary";
            case Rarity.Mythic:
                return "ui_gear_slot_banner_mythic";
            case Rarity.Arcane:
                return "ui_gear_slot_banner_arcane";
            default:
                return "ui_gear_slot_banner_common";
        }
    }

}

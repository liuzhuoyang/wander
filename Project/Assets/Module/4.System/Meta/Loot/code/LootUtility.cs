using System.Collections.Generic;

public static class LootUtility
{
    private static List<int> listBoxPoint;//宝箱点数
    private static List<UIChestLockArgs> listChestLockArgs;//宝箱锁
    // private static List<UIChestProbabilityArgs> listChestProbabilityArgs;//宝箱概率
    public static void InitData()
    {
        listBoxPoint = new List<int>();
        listBoxPoint.Add(1);
        listBoxPoint.Add(10);
        listBoxPoint.Add(20);
        listBoxPoint.Add(50);
        listBoxPoint.Add(0);

        listChestLockArgs = new List<UIChestLockArgs>();
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 20, chestIndex = 1 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 30, chestIndex = 1 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 40, chestIndex = 2 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 80, chestIndex = 3 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 60, chestIndex = 2 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 100, chestIndex = 4 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 80, chestIndex = 2 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 120, chestIndex = 3 });
        listChestLockArgs.Add(new UIChestLockArgs { needPoint = 180, chestIndex = 4 });

        // listChestProbabilityArgs = new List<UIChestProbabilityArgs>();
        // listChestProbabilityArgs.Add(new UIChestProbabilityArgs
        // {
        //     listChestProArgs = new List<ChestProArgs>()
        //     {
        //         new ChestProArgs { rarity = Rarity.Rare, isShard = false, probability = 0.07f },
        //         new ChestProArgs { rarity = Rarity.Rare, isShard = true, probability = 0.63f },
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = false, probability = 0.03f },
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = true, probability = 0.27f },
        //     }
        // });
        // listChestProbabilityArgs.Add(new UIChestProbabilityArgs
        // {
        //     listChestProArgs = new List<ChestProArgs>()
        //     {
        //         new ChestProArgs { rarity = Rarity.Rare, isShard = false, probability = 0.1f },
        //         new ChestProArgs { rarity = Rarity.Rare, isShard = true, probability = 0.5f },
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = false, probability = 0.045f },
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = true, probability = 0.21f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = false, probability = 0.015f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = true, probability = 0.13f },
        //     }
        // });
        // listChestProbabilityArgs.Add(new UIChestProbabilityArgs
        // {
        //     listChestProArgs = new List<ChestProArgs>()
        //     {
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = false, probability = 0.1f },
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = true, probability = 0.65f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = false, probability = 0.03f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = true, probability = 0.22f },
        //     }
        // });
        // listChestProbabilityArgs.Add(new UIChestProbabilityArgs
        // {
        //     listChestProArgs = new List<ChestProArgs>()
        //     {
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = false, probability = 0.05f },
        //         new ChestProArgs { rarity = Rarity.Epic, isShard = true, probability = 0.55f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = false, probability = 0.04f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = true, probability = 0.33f },
        //         new ChestProArgs { rarity = Rarity.Mythic, isShard = false, probability = 0.003f },
        //         new ChestProArgs { rarity = Rarity.Mythic, isShard = true, probability = 0.027f },
        //     }
        // });
        // listChestProbabilityArgs.Add(new UIChestProbabilityArgs
        // {
        //     listChestProArgs = new List<ChestProArgs>()
        //     {
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = false, probability = 0.075f },
        //         new ChestProArgs { rarity = Rarity.Legendary, isShard = true, probability = 0.7f },
        //         new ChestProArgs { rarity = Rarity.Mythic, isShard = false, probability = 0.025f },
        //         new ChestProArgs { rarity = Rarity.Mythic, isShard = true, probability = 0.15f },
        //         new ChestProArgs { rarity = Rarity.Arcane, isShard = false, probability = 0.005f },
        //         new ChestProArgs { rarity = Rarity.Arcane, isShard = true, probability = 0.045f },
        //     }
        // });
    }

    public static UIChestLockArgs GetChestLockArgs(int index)
    {
        if (index < 0 || index >= listChestLockArgs.Count) return null;
        return listChestLockArgs[index];
    }

    public static List<LootDataEditor> GetChestProbabilityArgs(int index)
    {
        if (index < 0 || index > AllLoot.dictData.Count) return null;
        return AllLoot.dictData[index].listData;
    }

    public static int GetChestPoint(int index)
    {
        if (index < 0 || index >= listBoxPoint.Count) return 0;
        return listBoxPoint[index];
    }

    public static (UIChestLockArgs, int) GetNextChestLock(int currentLockIndex)
    {
        if (currentLockIndex < 0 || currentLockIndex >= listChestLockArgs.Count - 1)
        {
            return (GetChestLockArgs(0), 0);
        }
        return (GetChestLockArgs(currentLockIndex + 1), currentLockIndex + 1);
    }
}

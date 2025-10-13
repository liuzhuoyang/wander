using System.Collections.Generic;
using UnityEngine;

public class UIGear : UIBase
{
    [SerializeField] Transform listTransform;



    [SerializeField] Transform unlockTransform;

    [SerializeField] Transform lockTransform;



    [SerializeField] GameObject gearSlotPrefab;
    [SerializeField] GameObject gearEquipSlotPrefab;



    void Awake()
    {
        EventManager.StartListening<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, OnRefresh);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, OnRefresh);
    }

    void OnRefresh(UIGearArgs args)
    {
        //关闭所有
        DestroyUnitContainer();

        // 处理装备槽位
        RefreshEquipSlots(args.unlockSlot, args.dictEquipGear, args.listGearSlotViewArgs);

        List<GearSlotViewArgs> listUnlockGearSlot;
        List<GearSlotViewArgs> listLockGearSlot;
        SplitAndSortGearList(args.listGearSlotViewArgs, out listUnlockGearSlot, out listLockGearSlot);

        // 对已解锁列表处理
        int unlockChildCount = unlockTransform.childCount;
        for (int i = 0; i < listUnlockGearSlot.Count; i++)
        {
            GameObject gearSlotObj;
            if (i < unlockChildCount)
            {
                gearSlotObj = unlockTransform.GetChild(i).gameObject;
                gearSlotObj.SetActive(true);
            }
            else
            {
                gearSlotObj = Instantiate(gearSlotPrefab, unlockTransform);
            }
            gearSlotObj.GetComponent<UIGearSlot>().Init(listUnlockGearSlot[i]);
        }
        // 多余的置为不可见
        for (int i = listUnlockGearSlot.Count; i < unlockChildCount; i++)
        {
            unlockTransform.GetChild(i).gameObject.SetActive(false);
        }

        // 对未解锁列表处理
        int lockChildCount = lockTransform.childCount;
        for (int i = 0; i < listLockGearSlot.Count; i++)
        {
            GameObject gearSlotObj;
            if (i < lockChildCount)
            {
                gearSlotObj = lockTransform.GetChild(i).gameObject;
                gearSlotObj.SetActive(true);
            }
            else
            {
                gearSlotObj = Instantiate(gearSlotPrefab, lockTransform);
            }
            gearSlotObj.GetComponent<UIGearSlot>().Init(listLockGearSlot[i]);
        }

    }


    /// <summary>
    /// 将GearSlotViewArgs列表根据unlock状态分成已解锁和未解锁两个列表，并根据Rarity从低到高排序
    /// </summary>
    /// <param name="sourceList">原始列表</param>
    /// <param name="unlockList">输出的已解锁列表</param>
    /// <param name="lockList">输出的未解锁列表</param>
    private void SplitAndSortGearList(List<GearSlotViewArgs> sourceList, out List<GearSlotViewArgs> unlockList, out List<GearSlotViewArgs> lockList)
    {
        unlockList = new List<GearSlotViewArgs>();
        lockList = new List<GearSlotViewArgs>();

        foreach (var item in sourceList)
        {
            if (item.isLocked)
            {
                unlockList.Add(item);
            }
            else
            {
                lockList.Add(item);
            }
        }

        unlockList.Sort((a, b) => a.rarity.CompareTo(b.rarity));
        lockList.Sort((a, b) => a.rarity.CompareTo(b.rarity));
    }




    /// <summary>
    /// 刷新装备槽位
    /// </summary>
    /// <param name="unlockSlotCount">解锁的槽位数量</param>
    /// <param name="equipGearList">已装备的装备列表</param>
    /// <param name="allGearSlotArgs">所有装备的显示参数</param>
    private void RefreshEquipSlots(int unlockSlotCount, Dictionary<int, string> dictEquipGear, List<GearSlotViewArgs> allGearSlotArgs)
    {
        const int MAX_SLOT_COUNT = 8; // 总共8个槽位
        /* 
               // 确保有8个槽位
               int currentSlotCount = listTransform.childCount;
              for (int i = currentSlotCount; i < MAX_SLOT_COUNT; i++)
               {
                   GameObject equipSlotObj = Instantiate(gearEquipSlotPrefab, listTransform);
               }
        */
        // 更新每个槽位的状态，8个槽位都永远可见
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject equipSlotObj = listTransform.GetChild(i).gameObject;

            // 获取GearSlotHelper组件并更新状态
            GearSlotHelper slotHelper = equipSlotObj.GetComponent<GearSlotHelper>();
            if (slotHelper != null)
            {
                // 槽位是否解锁：基于槽位索引和unlockSlotCount
                bool slotIsLocked = i >= unlockSlotCount;

                // 槽位是否有装备：基于equipGearList中对应位置
                bool slotHasGear = dictEquipGear.ContainsKey(i);
                string gearName = slotHasGear ? dictEquipGear[i] : "";

                // 创建装备槽位的显示参数
                GearSlotViewArgs slotArgs = allGearSlotArgs.Find(args => args.gearName == gearName);
                slotHelper.Init(i, slotIsLocked, slotHasGear, slotArgs);
            }
        }
    }

    private void DestroyUnitContainer()
    {
        foreach (Transform child in unlockTransform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in lockTransform)
        {
            child.gameObject.SetActive(false);
        }

    }


}

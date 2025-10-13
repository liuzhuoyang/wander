using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GearSlotHelper : MonoBehaviour, IPointerClickHandler
{
    int slotIndex;

    public Transform lockPage;
    public Transform nullPage;
    public Transform equipPage;

    [SerializeField] GameObject gearSlotPrefab; // 装备卡片prefab

    bool isActive = false;
    bool isEquip = false;
    bool isLocked = false;

    private UIGearSlot currentGearSlot; // 当前装备卡片组件
    private GearSlotViewArgs gearArgs;

    public void Init(int slotIndex, bool slotIsLocked, bool slotIsEquip, GearSlotViewArgs gearArgs)
    {
        this.slotIndex = slotIndex;
        this.isEquip = slotIsEquip;
        this.isLocked = slotIsLocked;
        this.gearArgs = gearArgs;
        // 清理旧的装备卡片
        ClearGearSlot();

        // 如果有装备，创建装备卡片
        if (isEquip && gearArgs != null)
        {
            CreateGearSlot(gearArgs);
        }

        // 控制页面显示
        UpdatePageVisibility();
    }

    /// <summary>
    /// 更新页面显示状态
    /// </summary>
    private void UpdatePageVisibility()
    {
        if (lockPage != null)
        {
            lockPage.gameObject.SetActive(isLocked);
        }

        if (nullPage != null)
        {
            // 解锁但未装备时显示空界面
            nullPage.gameObject.SetActive(!isLocked && !isEquip);
        }

        if (equipPage != null)
        {
            // 解锁且已装备时显示装备页面
            equipPage.gameObject.SetActive(!isLocked && isEquip);
        }
    }


    public void StartEquiping()
    {
        isActive = true;

        if (isLocked)
        {
            //晃动
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(new Vector3(1.08f, 1.08f, 1f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    public void StopEquiping()
    {
        isActive = false;
        transform.DOKill();
        transform.localScale = Vector3.one;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked)
        {
            //未解锁
            Debug.Log("未解锁");
            return;
        }

        if (isActive)
        {
            GearSystem.Instance.OnEquipGear(slotIndex);
            return;
        }
        else
        {
            // 检查槽位是否有装备，如果有装备则屏蔽槽位点击
            if (isEquip)
            {
                // 槽位有装备时，不处理槽位点击，让卡片的点击事件处理
                return;
            }

        }
    }

    /// <summary>
    /// 创建装备卡片
    /// </summary>
    /// <param name="gearArgs">装备显示参数</param>
    private void CreateGearSlot(GearSlotViewArgs gearArgs)
    {
        if (equipPage == null || gearSlotPrefab == null)
        {
            Debug.LogWarning("GearSlotHelper: equipPage 或 gearSlotPrefab 未设置");
            return;
        }

        // 创建装备卡片
        GameObject gearSlotObj = Instantiate(gearSlotPrefab, equipPage);
        currentGearSlot = gearSlotObj.GetComponent<UIGearSlot>();

        if (currentGearSlot != null)
        {
            // 直接使用传入的装备数据初始化卡片
            currentGearSlot.Init(gearArgs);
        }
    }

    /// <summary>
    /// 清理装备卡片
    /// </summary>
    private void ClearGearSlot()
    {
        if (currentGearSlot != null)
        {
            Destroy(currentGearSlot.gameObject);
            currentGearSlot = null;
        }

        // 清理equipPage下的所有子对象
        if (equipPage != null)
        {
            foreach (Transform child in equipPage)
            {
                Destroy(child.gameObject);
            }
        }
    }


}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILoot : UIBase
{
    [SerializeField] SlicedFilledImage fill;
    [SerializeField] TextMeshProUGUI textPoint, textBoxName, textOpenNum;
    [SerializeField] Image imgChestLock, imgChest, imgCenter;
    [SerializeField] GameObject objBtnLeft, objBtnRight, objChestLock, objChest, objPrefab, objBtnOpen, objBtnOpenNo;
    [SerializeField] List<LootSlot> listSlot;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Animator animator;

    bool needAnimation;
    UILootArgs args;
    Coroutine shakeCoroutine;
    const float SHAKE_ANGLE = 5f; // 抖动角度
    const float SHAKE_DURATION = 0.1f; // 每次抖动持续时间

    void Awake()
    {
        EventManager.StartListening<UILootArgs>(EventNameLoot.EVENT_LOOT_INIT, OnInit);
        EventManager.StartListening<UILootArgs>(EventNameLoot.EVENT_LOOT_REFRESH_UI, OnRefreshUI);
        EventManager.StartListening<UILootArgs>(EventNameLoot.EVENT_LOOT_CLAIM_REFRESH, OnClaimRefresh);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UILootArgs>(EventNameLoot.EVENT_LOOT_INIT, OnInit);
        EventManager.StopListening<UILootArgs>(EventNameLoot.EVENT_LOOT_REFRESH_UI, OnRefreshUI);
        EventManager.StopListening<UILootArgs>(EventNameLoot.EVENT_LOOT_CLAIM_REFRESH, OnClaimRefresh);
    }

    void OnInit(UILootArgs args)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            imgChest.transform.rotation = Quaternion.identity; // 重置旋转
        }

        //宝箱锁
        for (int i = 0; i < listSlot.Count; i++)
        {
            listSlot[i].Init(i);
        }
        RefreshChestLock(GameData.userData.userLoot.point, args.chestLockArgs.needPoint, args.chestLockArgs.chestIndex);
        needAnimation = false;
        OnRefreshUI(args);
        needAnimation = true;
    }

    void OnClaimRefresh(UILootArgs args)
    {
        RefreshChestLock(GameData.userData.userLoot.point, args.chestLockArgs.needPoint, args.chestLockArgs.chestIndex);
        RefreshListChest(args.chooseIndex, args.listCount);
        RefreshBtnCount(args.canOpenCount);
    }

    public void OnAnimationEnd()
    {
        needAnimation = false;
        OnRefreshUI(args);
        needAnimation = true;
    }

    void OnRefreshUI(UILootArgs args)
    {
        //刷新宝箱
        RefreshListChest(args.chooseIndex, args.listCount);

        if (needAnimation)
        {
            this.args = args;
            animator.SetTrigger("Change");
            return;
        }
        //宝箱名称
        textBoxName.text = UtilityLocalization.GetLocalization("page/loot/page_loot_box_" + (args.chooseIndex + 1));
        //宝箱icon
        GameAssetControl.AssignIcon("item_loot_box_" + (args.chooseIndex + 1), imgCenter);
        //刷新宝箱概率
        foreach (Transform child in rectTransform)
        {
            Destroy(child.gameObject);
        }
        Dictionary<Rarity, float> rarityPro = new Dictionary<Rarity, float>();
        foreach (var item in LootUtility.GetChestProbabilityArgs(args.chooseIndex + 1))
        {
            if (rarityPro.ContainsKey(item.rarity))
            {
                rarityPro[item.rarity] += item.probability;
            }
            else
            {
                rarityPro.Add(item.rarity, item.probability);
            }
        }
        foreach (var item in rarityPro)
        {
            GameObject obj = Instantiate(objPrefab, rectTransform);
        }
        //刷新按钮
        if (args.chooseIndex <= 0)
        {
            objBtnLeft.SetActive(false);
        }
        else
        {
            objBtnLeft.SetActive(true);
        }
        if (args.chooseIndex >= listSlot.Count - 1)
        {
            objBtnRight.SetActive(false);
        }
        else
        {
            objBtnRight.SetActive(true);
        }
        //刷新可开宝箱数量
        RefreshBtnCount(args.canOpenCount);
    }

    void RefreshListChest(int chooseIndex, List<int> listCount)
    {
        for (int i = 0; i < listSlot.Count; i++)
        {
            listSlot[i].Refresh(chooseIndex, listCount[i]);
        }
    }

    void RefreshBtnCount(int canOpenCount)
    {
        if (canOpenCount > 0)
        {
            objBtnOpen.SetActive(true);
            objBtnOpenNo.SetActive(false);
            textOpenNum.text = UtilityLocalization.GetLocalization("page/loot/page_loot_open", canOpenCount.ToString());
            return;
        }
        objBtnOpen.SetActive(false);
        objBtnOpenNo.SetActive(true);
    }

    IEnumerator ShakeChest()
    {
        while (true)
        {
            // 向右摆动
            float elapsed = 0f;
            while (elapsed < SHAKE_DURATION)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / SHAKE_DURATION;
                imgChest.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, SHAKE_ANGLE, progress));
                yield return null;
            }

            // 向左摆动
            elapsed = 0f;
            while (elapsed < SHAKE_DURATION)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / SHAKE_DURATION;
                imgChest.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(SHAKE_ANGLE, -SHAKE_ANGLE, progress));
                yield return null;
            }

            // 回到中间
            elapsed = 0f;
            while (elapsed < SHAKE_DURATION)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / SHAKE_DURATION;
                imgChest.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-SHAKE_ANGLE, 0, progress));
                yield return null;
            }

            yield return new WaitForSeconds(1f); // 等待1秒后再次开始抖动
        }
    }

    void RefreshChestLock(float point, float needPoint, int chestIndex)
    {
        fill.fillAmount = point / needPoint;
        textPoint.text = point + "/" + needPoint;
        if (point >= needPoint)
        {
            objChestLock.SetActive(false);
            objChest.SetActive(true);
            GameAssetControl.AssignIcon("item_loot_box_" + (chestIndex + 1), imgChest);
            // 开始抖动动画
            if (shakeCoroutine == null)
            {
                shakeCoroutine = StartCoroutine(ShakeChest());
            }
        }
        else
        {
            objChestLock.SetActive(true);
            GameAssetControl.AssignIcon("item_loot_box_" + (chestIndex + 1), imgChestLock);
            objChest.SetActive(false);
            // 停止抖动动画
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                shakeCoroutine = null;
                imgChest.transform.rotation = Quaternion.identity; // 重置旋转
            }
        }
    }

    public void OnClose()
    {
        base.CloseUI();
        LootSystem.Instance.OnClose();
    }

    public void OnClickBtnLeft()
    {
        LootSystem.Instance.OnClickBtnLeft();
    }

    public void OnClickBtnRight()
    {
        LootSystem.Instance.OnClickBtnRight();
    }

    public void OnClickBtnClaim()
    {
        LootSystem.Instance.OnClickBtnClaim();
    }

    public void OnClickChestLock()
    {
        LootSystem.Instance.OnClickChestLock();
    }

    public void OnOpenRelic()
    {
        //RelicSystem.Instance.Open();
    }
}

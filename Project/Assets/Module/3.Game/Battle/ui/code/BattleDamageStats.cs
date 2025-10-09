using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//伤害统计UI
public class BattleDamageStats : MonoBehaviour
{
    [SerializeField] GameObject objMain, objPrefab;
    [SerializeField] RectTransform rectTransform;

    private List<BattleDamageStatsSlot> listDamageStatsSlot;
    private bool isShow;
    void Awake()
    {
        EventManager.StartListening<UIDamageStatsArgs>(EventNameModeBattle.EVENT_BATTLE_DAMAGE_STATS_INIT, OnInitUI);
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_REGISTER_EVENT_UI, OnRegisterEvent);
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_UNREGISTER_EVENT_UI, OnUnRegisterEvent);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIDamageStatsArgs>(EventNameModeBattle.EVENT_BATTLE_DAMAGE_STATS_INIT, OnInitUI);
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_REGISTER_EVENT_UI, OnRegisterEvent);
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_UNREGISTER_EVENT_UI, OnUnRegisterEvent);
    }

    //波次开始隐藏main
    void OnInitUI(UIDamageStatsArgs args)
    {
        isShow = false;
        rectTransform.gameObject.SetActive(false);
        // objMain.SetActive(false);
        // objShow.SetActive(true);
        listDamageStatsSlot = new List<BattleDamageStatsSlot>();

        foreach (Transform child in rectTransform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnRegisterEvent(UIBattleFightArgs args)
    {
        //BattleData.damageStatsArgs.Subscribe(OnShowDamage);
    }

    void OnUnRegisterEvent(UIBattleFightArgs args)
    {
        //BattleData.damageStatsArgs.Unsubscribe(OnShowDamage);
    }

    //显示
    void OnShow()
    {
        float targetAlpha = 1f;
        float alphaStep = isShow ? 0f : 0.4f;

        foreach (var item in listDamageStatsSlot)
        {
            item.SetAlpha(targetAlpha);
            targetAlpha = Mathf.Max(0f, targetAlpha - alphaStep);
        }
    }

    //刷新伤害
    void OnShowDamage(UIDamageStatsArgs args)
    {
        //判断顺序是否变化
        for (int i = 0; i < listDamageStatsSlot.Count; i++)
        {
            if (listDamageStatsSlot[i].GetDamageStatsName() != args.listDamageStats[i].damageStatsName)
            {
                listDamageStatsSlot[i].Init(args.listDamageStats[i]);
            }
            else
            {
                listDamageStatsSlot[i].SetDamage(Mathf.CeilToInt(args.listDamageStats[i].damage), args.listDamageStats[i].damagePercent);
            }
        }
        //是否有新增伤害
        for (int i = listDamageStatsSlot.Count; i < args.listDamageStats.Count; i++)
        {
            GameObject obj = Instantiate(objPrefab, rectTransform);
            obj.GetComponent<BattleDamageStatsSlot>().Init(args.listDamageStats[i]);
            listDamageStatsSlot.Add(obj.GetComponent<BattleDamageStatsSlot>());
            // OnShow();
            // scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    // public void OnClickShow()
    // {
    //     // BattleData.isShow = true;
    //     // OnShow(BattleData.damageStatsArgs);
    // }

    // public void OnClickHide()
    // {
    //     // BattleData.isShow = false;
    //     // objMain.SetActive(false);
    //     // objShow.SetActive(true);
    // }

    //渐隐
    public void OnClick()
    {
        isShow = !isShow;
        //刷新间隔
        // rectTransform.GetComponent<VerticalLayoutGroup>().spacing = isShow ? 0f : 10f;
        rectTransform.gameObject.SetActive(isShow);
        // scrollRect.verticalNormalizedPosition = 1f;
        // OnShow();
    }

}
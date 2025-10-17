using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIModeBattlePrepare : UIBase
{
    public Transform transBottom;

    [SerializeField] GameObject objBoss, objHubBoss, objHubElite, objExpandAd, objRefreshAd;
    [SerializeField] GameObject objPreview;

    [Header("基地血条")]
    [SerializeField] BattleHealthBase battleBaseHealth;
    [SerializeField] UIModeBattlePrepareDebug uiBattleMergeDebug;
    [SerializeField] TextMeshProUGUI textExpandCost, textRefreshCost, textExpandCount;
    [SerializeField] TextMeshProUGUI textToken; //洛能矿数量

    [Header("经验")]
    [SerializeField] TextMeshProUGUI textLevel;
    [SerializeField] TextMeshProUGUI textExp;
    [SerializeField] SlicedFilledImage barExp;
    int expNext;

    [Header("按钮")]
    [SerializeField] GameObject objBtnFight, objBtnRefresh, objBtnExpand;
    [SerializeField] GameObject objGroupRefreshAD, objGroupRefreshTokenCost;
    [SerializeField] GameObject objGroupExpandAD, objGroupExpandTokenCost;

    [Header("三级武器广告")]
    [SerializeField] GameObject objADGear;
    [SerializeField] Image imgADGear;
    [SerializeField] SlicedFilledImage fillADGear;
    Vector2 rewardStartPosition;
    string adGearName;
    Coroutine coroutineHideADGear;
    Sequence animationSequence;
    Vector2 hubBossElitePos;

    public void Awake()
    {
        rewardStartPosition = objADGear.GetComponent<RectTransform>().anchoredPosition;
        hubBossElitePos = objHubBoss.transform.position;
        EventManager.StartListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_INIT_UI, OnInitUI);
        EventManager.StartListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_REFRESH_UI, OnRefreshUI);
        EventManager.StartListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_ON_REGISTER_EVENT_UI, OnRegisterEvent);
        EventManager.StartListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_ON_UNREGISTER_EVENT_UI, OnUnRegisterEvent);
        EventManager.StartListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_SHOW_HEALTH_UI, OnShowHealthUI);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_INIT_UI, OnInitUI);
        EventManager.StopListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_REFRESH_UI, OnRefreshUI);
        EventManager.StopListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_ON_REGISTER_EVENT_UI, OnRegisterEvent);
        EventManager.StopListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_PREPARE_ON_UNREGISTER_EVENT_UI, OnUnRegisterEvent);
        EventManager.StopListening<UIBattlePrepareArgs>(EventNameModeBattle.EVENT_BATTLE_SHOW_HEALTH_UI, OnShowHealthUI);
    }

    void OnInitUI(UIBattlePrepareArgs args)
    {
        objADGear.GetComponent<RectTransform>().anchoredPosition = rewardStartPosition;
        uiBattleMergeDebug.Init();
        //波次结束护盾重置 合成界面不显示进度条
        battleBaseHealth.Init();

        float targetY = transBottom.localPosition.y - 800;
        transBottom.DOLocalMoveY(targetY, 0.5f).SetEase(Ease.OutSine).SetDelay(0.5f).From();
    }

    void OnRegisterEvent(UIBattlePrepareArgs args)
    {
       
    }

    void OnUnRegisterEvent(UIBattlePrepareArgs args)
    {
    }

    void OnRefreshUI(UIBattlePrepareArgs args)
    {
        
    }

    void OnUpdateRefreshCostPerWave(int chipPerRefresh)
    {
        if (textRefreshCost == null)
        {
            return;
        }
        textRefreshCost.text = chipPerRefresh.ToString();
    }

    private void Reset()
    {

    }

    public void OnShowHealthUI(UIBattlePrepareArgs args)
    {
        battleBaseHealth.gameObject.SetActive(args.isShowHealthUI);
        objPreview.SetActive(false);
    }

    public void OnFight()
    {
        BattleSystem.Instance.OnChangeBattleState(BattleStates.PrepareEnd);
    }

    public void OnRefreshItem()
    {
        //BattleSystem.Instance.OnRefreshBrick();
    }

    public void OnPause()
    {

        BattleSystem.Instance.OnPause();
    }
}

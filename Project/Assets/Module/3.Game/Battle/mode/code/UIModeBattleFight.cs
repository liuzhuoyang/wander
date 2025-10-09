using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.Mathematics;

public class UIModeBattleFight : UIBase
{
    // public GameObject objMana;
    public TextMeshProUGUI textChip;

    public TextMeshProUGUI textLevel;
    public SlicedFilledImage barExp;

    public TextMeshProUGUI textWave;

    [Header("敌人数量")]
    [SerializeField] private TextMeshProUGUI textEnemyCount;
    [SerializeField] private TextMeshProUGUI textExp;

    [Header("敌人数量动画")]
    [SerializeField] private float sizePunch = 1.05f;
    [SerializeField] private float anglePunch = 10;
    [SerializeField] private float punchDuration = 0.15f;
    [SerializeField] private int lowerEnemyCount = 30;
    [SerializeField] private int lowEnemyCount = 20;
    [SerializeField] private float lowEnemyBaseSize = 1.25f;
    [SerializeField] private Color lowerEnemyColor = Color.red;
    [SerializeField] private Color lowEnemyColor = Color.red;
    [Header("Zero动画")]
    [SerializeField] private float zeroSize = 1.5f;
    [SerializeField] private RectTransform enemyCountRect;

    [Header("基地血条")]
    //基地血条
    [SerializeField] private BattleHealthBase battleBaseHealth;

    [Header("Boss 血条")]
    public BattleHealthBoss bossHealth;

    [Header("破损预警")]
    //破损红色预警
    [SerializeField] GameObject objRedAlert;

    [Header("Debug")]
    public UIBattleDebug uiDebug;
    [Header("设置按钮")]
    [SerializeField] GameObject btnSetting;
    
    public void Awake()
    {
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_INIT_UI, OnInitUI);
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_REGISTER_EVENT_UI, OnRegisterEvent);
        EventManager.StartListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_UNREGISTER_EVENT_UI, OnUnRegisterEvent);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_INIT_UI, OnInitUI);
        EventManager.StopListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_REGISTER_EVENT_UI, OnRegisterEvent);
        EventManager.StopListening<UIBattleFightArgs>(EventNameModeBattle.EVENT_BATTLE_FIGHT_ON_UNREGISTER_EVENT_UI, OnUnRegisterEvent);
    }

    void OnInitUI(UIBattleFightArgs args)
    {
        
    }

    void OnRegisterEvent(UIBattleFightArgs args)
    {
        //红色预警
        objRedAlert.SetActive(false);

        BattleData.battleToken.Subscribe(OnUpdateExp);
    }

    void OnUnRegisterEvent(UIBattleFightArgs args)
    {
        BattleData.battleToken.Unsubscribe(OnUpdateExp);
    }

    void OnUpdateChip(int chip)
    {
        textChip.text = chip.ToString();
    }

    void OnUpdateWave(int currentWave)
    {
        //textWave.text = string.Format(LocalizationUtility.GetLocalization("dynamic/wave_x_y"), currentWave, BattleData.totalWave);
    }

    void OnUpdateEnemyCount(int enemyCount)
    {
        textEnemyCount.text = enemyCount.ToString();

        float baseSize = 1;
        if (enemyCount <= lowerEnemyCount)
        {
            if (enemyCount > lowEnemyCount)
            {
                textEnemyCount.color = Color.Lerp(Color.white, lowerEnemyColor,
                                                 1 - (0f + enemyCount - lowEnemyCount) / (lowerEnemyCount - lowEnemyCount + 0f));
            }
            else if (enemyCount > 0)
            {
                textEnemyCount.color = lowEnemyColor;
                baseSize = lowEnemyBaseSize;
            }
            else
            {
                baseSize = zeroSize;
            }
        }
        else
        {
            textEnemyCount.color = Color.white;
        }

        enemyCountRect.DOKill();
        //字数改变动画，为0时播放特别动画
        if (enemyCount == 0)
        {
            enemyCountRect.localScale = Vector3.one * lowEnemyBaseSize;
            enemyCountRect.localRotation = Quaternion.identity;
            enemyCountRect.DOScale(Vector3.one * zeroSize, .5f).SetEase(Ease.OutBack, 5);
        }
        else
        {
            enemyCountRect.localScale = Vector3.one * baseSize;
            enemyCountRect.localRotation = Quaternion.identity;
            enemyCountRect.DOPunchScale(Vector3.one * sizePunch, punchDuration, 1);
            enemyCountRect.DOPunchRotation(Vector3.forward * UnityEngine.Random.Range(-anglePunch, anglePunch), punchDuration, UnityEngine.Random.Range(1, 4));
        }
    }

    void OnUpdateExp(int exp)
    {
        /*
        textLevel.text = BattleData.inGameLevel.Value.ToString();
        expNext = BattleFormula.GetInGameLevelExpNeeded(BattleData.inGameLevel);
        // textExp.text = BattleData.inGameExp.Value + "/" + expNext;
        float expValue = math.min((float)BattleData.inGameExp.Value / (float)expNext, 1);
        barExp.fillAmount = expValue;
        textExp.text = (expValue * 100).ToString("F0") + "%";*/
    }

    void OnUpdateHealth(float currentHealth, float maxhealth, bool isChangeHealth)
    {
        battleBaseHealth.UpdateHealth(currentHealth, maxhealth, isChangeHealth);
    }

    void OnUpdateBossHealth(int currentHealth)
    {
        bossHealth.OnUpdateBossHealth(currentHealth);
        if (currentHealth <= 0)
        {
            bossHealth.gameObject.SetActive(false);
        }
    }

    public void OnPause()
    {
        BattleSystem.Instance.OnPause();
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHealthBoss : MonoBehaviour
{
    [Header("Boss 血条")]
    //boss血条
    [SerializeField] private TextMeshProUGUI textBossHealth;
    [SerializeField] private TextMeshProUGUI textBossHealthBarCount;
    [SerializeField] private SlicedFilledImage barBossHealth;
    [SerializeField] private Image healthBackground; //生命值的背景

    [Header("Delta")]
    [SerializeField] private float deltaLerpSpeed = 5; 
    [SerializeField] private SlicedFilledImage healthDelta; //显示生命值下降情况
    [SerializeField] private SlicedFilledImage shieldDelta; //显示护盾值变化情况

    private int maxBossHealth;
    private int healthPerline = 1;//每条血量
    private int firstHealthPerline = 1;//第一条血量
    private int targetHealth;
    private int shownHealth; //显示血量，有一定的延迟
    private int deltaHealth; //显示血量，延迟更高
    private const int HEALTH_LINE_COUNT = 100;

    public void OnInit()
    {
        maxBossHealth = 0;
        shownHealth = 0;
    }
    void Update()
    {
        // Health Update
        if (shownHealth != targetHealth)
        {
            shownHealth = (int)Mathf.Lerp(shownHealth, targetHealth, Time.deltaTime * deltaLerpSpeed);
            if (Mathf.Abs(shownHealth - targetHealth) < 2)
            {
                shownHealth = targetHealth;
            }
        }
        int barLeft = GetHealthBarLeft(shownHealth);

        //会报错
        //barBossHealth.fillAmount = GetHealthRatio(shownHealth);

        //barBossHealth.color = UtilityColor.Instance.GetHealthBarColor(UtilityColor.Instance.LoopHealthBarIndex(barLeft));
        if (barLeft <= 1)
        {
            healthBackground.color = Color.clear;
        }
        else
        {
            healthBackground.color = UtilityColor.Instance.GetHealthBarColor(UtilityColor.Instance.LoopHealthBarIndex(barLeft-1));
        }

        deltaHealth = (int)Mathf.Lerp(deltaHealth, shownHealth, Time.deltaTime * deltaLerpSpeed);
        healthDelta.fillAmount = GetHealthRatio(deltaHealth);
    }
    float GetHealthRatio(int health)
    {
        int healthBarIndex = GetHealthBarLeft(health);
        if (healthBarIndex == HEALTH_LINE_COUNT)
        {
            //第一管血，血量采用调整值
            return (health - (healthBarIndex - 1) * healthPerline) / (float)firstHealthPerline;
        }
        else
        {
            return (health - (healthBarIndex - 1) * healthPerline) / (float)healthPerline;
        }
    }
    int GetHealthBarLeft(float health)
    {
        return Mathf.CeilToInt((health-firstHealthPerline+0f)/(0f+healthPerline)) + 1;
    }
    public void OnUpdateMaxHealth(int maxHealth)
    {
        deltaHealth = shownHealth = maxBossHealth = maxHealth;
        healthPerline = Mathf.RoundToInt((maxBossHealth + 0f) / (0f + HEALTH_LINE_COUNT));
        firstHealthPerline = maxBossHealth - healthPerline * (HEALTH_LINE_COUNT - 1);
        OnUpdateBossHealth(maxHealth);
    }
    public void OnUpdateBossHealth(int currentHealth)
    {
        if (maxBossHealth != 0)
        {
            targetHealth = currentHealth;

            textBossHealth.text = $"{Mathf.Max(currentHealth, 0)}";

            int barLeft = GetHealthBarLeft(currentHealth);
            textBossHealthBarCount.text = "x" + barLeft.ToString();
        }
        if (currentHealth <= 0)
        {
            maxBossHealth = 0;
            textBossHealthBarCount.text = "x0";
        }
    }
}
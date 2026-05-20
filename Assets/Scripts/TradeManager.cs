using TMPro;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance;

    public GameObject tradePanel;

    [Header("Trade LVL texts")]
    public TextMeshProUGUI healthLvlText;
    public TextMeshProUGUI damageLvlText;
    public TextMeshProUGUI rateLvlText;
    public TextMeshProUGUI distLvlText;

    private int healthLvl = 0;
    private int damageLvl = 0;
    private int rateLvl = 0;
    private int distLvl = 0;

    private PlayerHealth playerHealth;
    private PlayerShooting playerShooting;

    public int GetHealthLvl() => healthLvl;
    public int GetDamageLvl() => damageLvl;
    public int GetRateLvl() => rateLvl;
    public int GetDistLvl() => distLvl;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerShooting = player.GetComponent<PlayerShooting>();
        }
        UpdateTradeUI();
        UIManager.Instance.UpdateUI();
    }

    public void OpenTradeMenu()
    {
        tradePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseTradeMenu()
    {
        tradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BuyHealth()
    {
        if (UIManager.Instance.currentKills >= 10 && healthLvl < 5)
        {
            UIManager.Instance.currentKills -= 10;
            healthLvl++;
            playerHealth.IncreaseMaxHealth(1);
            UpdateTradeUI();
            UIManager.Instance.UpdateUI();
        }
    }

    public void BuyDamage()
    {
        if (UIManager.Instance.currentKills >= 20 && damageLvl < 1)
        {
            UIManager.Instance.currentKills -= 20;
            damageLvl++;
            playerShooting.arrowDamage += 1;
            UpdateTradeUI();
            UIManager.Instance.UpdateUI();
        }
    }

    public void BuyFireRate()
    {
        if (UIManager.Instance.currentKills >= 5 && rateLvl < 4)
        {
            UIManager.Instance.currentKills -= 5;
            rateLvl++;
            playerShooting.fireRate -= 0.15f;
            UpdateTradeUI();
            UIManager.Instance.UpdateUI();
        }
    }

    public void BuyFireDistance()
    {
        if (UIManager.Instance.currentKills >= 5 && distLvl < 5)
        {
            UIManager.Instance.currentKills -= 5;
            distLvl++;
            playerShooting.arrowLifetime += 0.2f;
            UpdateTradeUI();
            UIManager.Instance.UpdateUI();
        }
    }

    public void UpdateTradeUI()
    {
        healthLvlText.text = $"LVL {healthLvl}/5";
        damageLvlText.text = damageLvl >= 1 ? "MAX" : $"LVL {damageLvl}/1";
        rateLvlText.text = rateLvl >= 4 ? "MAX" : $"LVL {rateLvl}/4";
        distLvlText.text = distLvl >= 5 ? "MAX" : $"LVL {distLvl}/5";
    }

    public void LoadData(int hLvl, int dLvl, int rLvl, int diLvl, int kills)
    {
        healthLvl = hLvl;
        damageLvl = dLvl;
        rateLvl = rLvl;
        distLvl = diLvl;
        UIManager.Instance.currentKills = kills;

        playerShooting.arrowDamage = 1 + damageLvl;
        playerShooting.fireRate = 0.75f - (rateLvl * 0.15f);
        playerShooting.arrowLifetime = 0.5f + (distLvl * 0.2f);

        UpdateTradeUI();
        UIManager.Instance.UpdateUI();
    }
}

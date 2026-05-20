using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject statusPanel;

    [Header("UI texts")]
    public TextMeshProUGUI healthAmountText;
    public TextMeshProUGUI killsText;

    public int currentKills = 0;

    private PlayerHealth playerHealth;

    public int GetCurrentKills() => currentKills;

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
        }
        UpdateUI();
    }
    
    public void AddKill()
    {
        currentKills++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerHealth != null)
        {
            int health = playerHealth.GetHealth();
            healthAmountText.text = health > 0 ? new string('*', health) : "";
        }
        killsText.text = $"Kills: {currentKills}";
    }
}
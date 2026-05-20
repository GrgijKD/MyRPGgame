using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("Panels")]
    public GameObject savePanel;
    public TextMeshProUGUI lastSaveText;
    public GameObject gameOverPanel;

    private Transform playerTransform;
    private Vector3 playerStartPosition;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerStartPosition = playerTransform.position;
        }
        UpdateSavePanelText();

        if (PlayerPrefs.GetInt("LoadFromSave", 0) == 1)
        {
            Invoke("LoadProgress", 0.1f);
            PlayerPrefs.SetInt("LoadFromSave", 0);
        }
    }

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
        UpdateSavePanelText();
        Time.timeScale = 0f;
    }

    public void CloseSavePanel()
    {
        savePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateSavePanelText()
    {
        if (PlayerPrefs.HasKey("SaveDateTime"))
        {
            lastSaveText.text = "Last Save:\n" + PlayerPrefs.GetString("SaveDateTime");
        }
        else
        {
            lastSaveText.text = "There is no last save";
        }
    }

    public void SaveProgress()
    {
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        TradeManager tm = TradeManager.Instance;

        PlayerPrefs.SetInt("HealthLvl", tm.GetHealthLvl());
        PlayerPrefs.SetInt("DamageLvl", tm.GetDamageLvl());
        PlayerPrefs.SetInt("RateLvl", tm.GetRateLvl());
        PlayerPrefs.SetInt("DistLvl", tm.GetDistLvl());
        PlayerPrefs.SetInt("CurrentKills", UIManager.Instance.GetCurrentKills());
        PlayerPrefs.SetInt("CurrentHealth", playerHealth.GetHealth());

        string dateTimeStr = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        PlayerPrefs.SetString("SaveDateTime", dateTimeStr);

        PlayerPrefs.Save();
        UpdateSavePanelText();
    }

    public void LoadProgress()
    {
        if (!PlayerPrefs.HasKey("SaveDateTime"))
        {
            return;
        }

        TradeManager tm = TradeManager.Instance;
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();

        // Destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Player to start position
        playerTransform.position = playerStartPosition;
        playerTransform.gameObject.SetActive(true);

        tm.LoadData(
            PlayerPrefs.GetInt("HealthLvl"),
            PlayerPrefs.GetInt("DamageLvl"),
            PlayerPrefs.GetInt("RateLvl"),
            PlayerPrefs.GetInt("DistLvl"),
            PlayerPrefs.GetInt("CurrentKills")
        );

        int savedHealth = PlayerPrefs.GetInt("CurrentHealth");
        playerHealth.LoadHealth(savedHealth, PlayerPrefs.GetInt("HealthLvl"));

        UIManager.Instance.UpdateUI();
        tm.UpdateTradeUI();

        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
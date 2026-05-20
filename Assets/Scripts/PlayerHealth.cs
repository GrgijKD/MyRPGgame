using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int health;

    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    private Coroutine blinkCoroutine;

    void Awake()
    {
        health = maxHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health = maxHealth;
        UIManager.Instance.UpdateUI();
    }
    public int GetHealth() => health;

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        health -= damage;
        UIManager.Instance.UpdateUI();

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
    }

    void StartInvincibility()
    {
        isInvincible = true;
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(InvincibilityBlinkRoutine());
    }

    IEnumerator InvincibilityBlinkRoutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float timer = 0;

        while (timer < invincibilityDuration)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);

            sr.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        StopInvincibility();
    }

    void StopInvincibility()
    {
        isInvincible = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        blinkCoroutine = null;
    }

    void Die()
    {
        SaveManager.Instance.gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }

    public void LoadHealth(int savedHealth, int healthLvl)
    {
        maxHealth = 5 + healthLvl;
        health = savedHealth;
        StopInvincibility();
    }
}
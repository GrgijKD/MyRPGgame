using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Vector2 damageSourcePosition)
    {
        currentHealth -= amount;

        if (TryGetComponent(out DamageEffect damageEffect)) damageEffect.FlashRed();

        // Knockback from damage
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 pushDirection = ((Vector2)transform.position - damageSourcePosition).normalized;
            rb.AddForce(pushDirection * 7f, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        UIManager.Instance.AddKill();
        Destroy(gameObject);
    }
}
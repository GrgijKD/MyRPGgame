using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float lifetime = 0.5f;
    public int damage = 1;
    private Vector2 moveDirection;
    private float moveSpeed;

    public void Setup(Vector2 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Enemy"))
        {
            if (hitInfo.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
                enemy?.TakeDamage(damage, transform.position);
                Destroy(gameObject);
            }
        }
        else if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
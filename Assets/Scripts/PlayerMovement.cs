using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    public Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // Right-left movement flip
        if (movement.x > 0.1f) // Right
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x < -0.1f) // Left
        {
            spriteRenderer.flipX = false;
        }

        if (movement.sqrMagnitude < 0.01f)
        {
            spriteRenderer.flipX = false;
        }

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * speed;
    }
}
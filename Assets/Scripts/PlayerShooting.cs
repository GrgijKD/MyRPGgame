using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting settings")]
    public GameObject arrowPrefab;
    public float fireRate = 0.75f;
    public float arrowLifetime = 0.5f;
    public int arrowDamage = 1;
    public float arrowSpeed = 10f;
    public float fireOffset = 0.7f; // For not shooting arros from the center of character

    [Header("Input System")]
    public InputActionReference fireAction;

    private float nextFireTime = 0f;

    private void OnEnable() => fireAction.action.Enable();
    private void OnDisable() => fireAction.action.Disable();

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Vector2 inputDir = fireAction.action.ReadValue<Vector2>();

            if (inputDir.sqrMagnitude > 0)
            {
                Vector2 shootDir = GetMajorDirection(inputDir);
                Shoot(shootDir);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    // Four-direction shooting
    Vector2 GetMajorDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }

    void Shoot(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Vector3 spawnPosition = transform.position + (Vector3)dir * fireOffset;

        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, rotation);
        ArrowScript arrowScript = arrow.GetComponent<ArrowScript>();

        arrowScript.damage = arrowDamage;
        arrowScript.lifetime = arrowLifetime;
        arrowScript.Setup(dir, arrowSpeed);
    }
}
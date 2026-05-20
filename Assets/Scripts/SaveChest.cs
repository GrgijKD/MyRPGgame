using UnityEngine;
using UnityEngine.InputSystem;

public class SaveChest : MonoBehaviour
{
    public InputActionReference interactAction;
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && interactAction.action.WasPressedThisFrame())
        {
            SaveManager.Instance.OpenSavePanel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
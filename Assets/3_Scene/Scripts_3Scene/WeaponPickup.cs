using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponType weaponType;          // select in inspector (Gun / Knife)
    public GameObject interactPrompt;      // optional UI

    private WeaponSystem playerSystem;

    private void Awake()
    {
        playerSystem = FindAnyObjectByType<WeaponSystem>();

        if (playerSystem == null)
            Debug.LogError("WeaponPickup: No WeaponSystem found in scene!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerSystem == null)
            {
                Debug.LogError("WeaponPickup: playerSystem is null!");
                return;
            }

            playerSystem.EquipWeapon(weaponType);

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            Destroy(gameObject); // pickup disappears (correct)
        }
    }
}

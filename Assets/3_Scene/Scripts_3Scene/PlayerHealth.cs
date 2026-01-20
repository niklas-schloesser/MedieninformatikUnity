using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Lives")]
    public int lives = 3;
    public Image[] heartImages; // Size = 3
    public Transform respawnPoint;

    [Header("UI")]
    public Slider healthSlider;
    public Image damageFlash;
    public TextMeshProUGUI healthText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;

    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }

        if (damageFlash != null)
        {
            damageFlash.color = new Color(1, 0, 0, 0);
        }

        if (respawnPoint == null)
        {
            Debug.LogError("Respawn Point NOT assigned in PlayerHealth!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        isInvincible = true;
        Invoke(nameof(ResetInvincibility), 0.3f);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = currentHealth.ToString();

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        StopAllCoroutines();
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            LoseLife();
        }
    }

    void ResetInvincibility()
    {
        isInvincible = false;
    }

    void LoseLife()
    {
        lives--;

        if (lives >= 0 && lives < heartImages.Length)
        {
            heartImages[lives].enabled = false;
        }

        if (lives > 0)
        {
            Invoke(nameof(Respawn), 0.1f);
        }
        else
        {
            GameOver();
        }
    }

   void Respawn()
{
    // Reset health
    currentHealth = maxHealth;
    healthSlider.value = currentHealth;
    healthText.text = currentHealth.ToString();

    // Reset position & rotation
    transform.position = respawnPoint.position;
    transform.rotation = respawnPoint.rotation;

    // Reset physics
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
}

    void GameOver()
    {
        Debug.Log("GAME OVER");

        enabled = false; // Stop health logic
        Time.timeScale = 0f; // Freeze game
    }

    System.Collections.IEnumerator DamageFlash()
    {
        if (damageFlash == null) yield break;

        damageFlash.color = new Color(1, 0, 0, 0.4f);
        yield return new WaitForSeconds(0.2f);
        damageFlash.color = new Color(1, 0, 0, 0);
    }
}

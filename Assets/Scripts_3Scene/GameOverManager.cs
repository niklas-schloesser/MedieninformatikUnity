using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;      // Panel
    public TMP_Text gameOverText;         // "GAME OVER"
    public TMP_Text timeFinishedText;     // "Time Finished"
    public Button restartButton;          // Button component
    public TMP_Text restartButtonText;    // Text inside button
    public GameObject player;

    private void Start()
    {
        // Hide panel at start
        gameOverPanel.SetActive(false);

        // Button click
        restartButton.onClick.AddListener(RestartLevel);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        gameOverText.text = "GAME OVER";
        timeFinishedText.text = "Time Finished";
        restartButtonText.text = "Restart";
        Time.timeScale = 0f; // FREEZE GAME

    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; // UNFREEZE

    }
}

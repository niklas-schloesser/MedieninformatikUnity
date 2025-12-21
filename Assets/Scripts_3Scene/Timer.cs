using UnityEngine;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timerDuration = 300f;
    public TMP_Text timerText;
    public GameOverManager gameOverManager;

    private Coroutine timerCoroutine;

    private void Start()
    {
        // Do NOT auto start
        if (timerText != null)
            timerText.text = "05:00";
    }

    public void StartTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(StartTimerCoroutine());
        }
    }

    private IEnumerator StartTimerCoroutine()
    {
        float timeLeft = timerDuration;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeLeft / 60f);
                int seconds = Mathf.FloorToInt(timeLeft % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }

            yield return null;
        }

        gameOverManager.ShowGameOver();
    }
}

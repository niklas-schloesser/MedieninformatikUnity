using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool running;

    public System.Action OnTimeUp;

  public void StartTimer()
{
    timeRemaining = timeLimit;
    running = true;

    if (timerText != null)
        timerText.gameObject.SetActive(true);
}


    void Update()
    {
        if (!running) return;

        timeRemaining -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


        if (timeRemaining <= 0)
        {
            running = false;
            OnTimeUp?.Invoke();
        }
    }
}

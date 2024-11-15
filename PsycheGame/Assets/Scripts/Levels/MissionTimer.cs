using UnityEngine;
using UnityEngine.UI;

public class MissionTimer : MonoBehaviour
{
    [SerializeField] private float missionDuration = 120f;
    [SerializeField] private GameObject modalPanel; 
    [SerializeField] private Text timerText; 

    private float timeRemaining;
    private bool isTimerRunning = false;

    void Start()
    {
        timeRemaining = missionDuration;
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                TimerExpired();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void TimerExpired()
    {
        Debug.Log("Mission timer expired!");
        modalPanel.SetActive(true);
    }
}

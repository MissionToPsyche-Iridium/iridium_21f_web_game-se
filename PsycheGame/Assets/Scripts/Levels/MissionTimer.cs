using TMPro;
using UnityEngine;

public class MissionTimer : MonoBehaviour
{
    [SerializeField] private float missionDuration = 120f;
    [SerializeField] private GameObject modalPanel; 
    [SerializeField] private GameObject timerText; 

    private float timeRemaining;
    private bool isTimerRunning = false;

    void Start()
    {
        timeRemaining = missionDuration;
        isTimerRunning = false; 
    }

    public void StartMissionTimer()
    {
        timeRemaining = missionDuration;
        isTimerRunning = true;
        Debug.Log("Mission Timer Started!");
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
        
        TextMeshProUGUI textMeshPro = timerText.GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            textMeshPro.text = $"{minutes:00}:{seconds:00}";
            //Debug.Log($"{minutes:00}:{seconds:00}");
        }
    }

    private void TimerExpired()
    {
        Debug.Log("Mission timer expired!");
        modalPanel.SetActive(true);
    }
}

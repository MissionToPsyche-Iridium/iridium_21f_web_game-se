using TMPro;
using UnityEngine;

public class MissionTimer : MonoBehaviour
{
    [SerializeField] private GameObject modalPanel; 
    [SerializeField] private TextMeshProUGUI timerText; 


    public void UpdateTimerUI(float timeRemaining)
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

    public void ShowTimer()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
    }

    public void HideTimer()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }

    public void ShowModalPanel()
    {
        if (modalPanel != null)
        {
            modalPanel.SetActive(true);
        }
    }
}

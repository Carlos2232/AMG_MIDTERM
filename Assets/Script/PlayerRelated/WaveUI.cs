using UnityEngine;
using TMPro;

public class SurvivalTimerUI : MonoBehaviour
{
    public static SurvivalTimerUI Instance;

    [Header("UI References")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float totalGameTime = 480f; 
    private float currentTime;
    private bool isRunning = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentTime = totalGameTime;
    }

    void Update()
    {
        if (!isRunning) return;

        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                TriggerWinCondition();
            }
        }

        UpdateTimerDisplay(currentTime);
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
            int seconds = Mathf.FloorToInt(timeToDisplay % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void TriggerWinCondition()
    {
        Debug.Log("Victory! Survived the full 8 minutes.");
    }
}
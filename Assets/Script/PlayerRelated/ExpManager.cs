using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    [Header("UI References")]
    public Slider expSlider;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText; 

    [Header("Level Stats")]
    public int currentExp = 0;
    public int maxExp = 50;
    public int currentLevel = 1;

    [Header("Animation Settings")]
    public float lerpSpeed = 10f;
    private float displayedExp = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        displayedExp = currentExp;
        if (expSlider != null)
        {
            expSlider.maxValue = maxExp;
            expSlider.value = currentExp;
        }
        UpdateExpUI();
    }

    void Update()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = maxExp;
            displayedExp = Mathf.Lerp(displayedExp, currentExp, lerpSpeed * Time.deltaTime);
            expSlider.value = displayedExp;
        }
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            displayedExp = 0f;
            currentLevel++;
            maxExp = Mathf.RoundToInt(maxExp * 1.5f);
            Debug.Log("Level Up! Reached Level: " + currentLevel);

            if (LevelManager.Instance != null && LevelManager.Instance.levelUpUIPanel != null)
            {
                Time.timeScale = 0f;
                LevelManager.Instance.levelUpUIPanel.SetActive(true);
            }
        }

        UpdateExpUI();
    }

    void UpdateExpUI()
    {
        if (expText != null)
        {
            expText.text = currentExp + " / " + maxExp;
        }

        if (levelText != null)
        {
            levelText.text = "Lv. " + currentLevel;
        }
    }
}
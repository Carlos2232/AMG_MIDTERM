using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro; 

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Audio Mixer Reference")]
    public AudioMixer audioMixer; 

    [Header("EXP & Level Progress")]
    public int currentLevel = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 10;

    [Header("Power Levels (Max 10)")]
    public int laserLevel = 1;
    public int orbitLevel = 1;
    public int pulseLevel = 1;

    [Header("UI References")]
    public GameObject levelUpUIPanel; 
    public Button[] upgradeButtons = new Button[3];

    [Header("Weapon Level Text UIs")]
    public TextMeshProUGUI laserLevelText; 
    public TextMeshProUGUI orbitLevelText; 
    public TextMeshProUGUI pulseLevelText; 


    private float savedMusicVolumeBeforeLevelUp = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        UpdateWeaponLevelUI();
    }

    void UpdateWeaponLevelUI()
    {
        if (laserLevelText != null)
        {
            if (laserLevel >= 10) laserLevelText.text = "Beam Lv. 10 (MAX)";
            else laserLevelText.text = "Beam Lv. " + laserLevel;
        }

        if (orbitLevelText != null)
        {
            if (orbitLevel >= 10) orbitLevelText.text = "Orbit Lv. 10 (MAX)";
            else orbitLevelText.text = "Orbit Lv. " + orbitLevel;
        }

        if (pulseLevelText != null)
        {
            if (pulseLevel >= 10) pulseLevelText.text = "Pulse Lv. 10 (MAX)";
            else pulseLevelText.text = "Pulse Lv. " + pulseLevel;
        }
    }

    public void AddEXP(int amount)
    {
        currentEXP += amount;
        Debug.Log("EXP: " + currentEXP + "/" + expToNextLevel + " (Level " + currentLevel + ")");

        if (currentEXP >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentEXP -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);

        Time.timeScale = 0f;
        if (levelUpUIPanel != null)
        {
            levelUpUIPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelUpSound();
        }

        if (audioMixer != null)
        {
            float currentDecibels;
            if (audioMixer.GetFloat("MusicVolume", out currentDecibels))
            {
                savedMusicVolumeBeforeLevelUp = Mathf.Pow(10f, currentDecibels / 20f);
            }

            audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(savedMusicVolumeBeforeLevelUp * 0.3f, 0.0001f, 1f)) * 20f);
        }
    }

    public void SelectUpgrade(int powerIndex)
    {
        switch (powerIndex)
        {
            case 1:
                if (laserLevel < 10) laserLevel++;
                Debug.Log("Laser Beam upgraded to Level " + laserLevel + "!");
                break;
            case 2:
                if (orbitLevel < 10) orbitLevel++;
                Debug.Log("Orbiting Orb upgraded to Level " + orbitLevel + "!");
                break;
            case 3:
                if (pulseLevel < 10) pulseLevel++;
                Debug.Log("Shockwave Pulse upgraded to Level " + pulseLevel + "!");
                break;
        }

        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(savedMusicVolumeBeforeLevelUp, 0.0001f, 1f)) * 20f);
        }

        if (levelUpUIPanel != null)
        {
            levelUpUIPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Added for the New Input System

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    void Start()
    {
        if (mainMenuPanel == null && transform.childCount > 0)
            mainMenuPanel = transform.GetChild(0).gameObject;

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Using the New Input System to check for Escape key press
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
            }
        }
    }

    public void PlayGame()
    {
        PlayButtonClickSFX();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RestartMusic();
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        PlayButtonClickSFX();
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        PlayButtonClickSFX();

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        PlayButtonClickSFX();
        Debug.Log("Quit Game clicked.");
        Application.Quit();
    }

    void PlayButtonClickSFX()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
}
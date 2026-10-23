using UnityEngine;
using UnityEngine.InputSystem;

public class TogglePanel : MonoBehaviour
{
    public GameObject settingsPanel; 

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ESC key pressed!");
            Toggle();
        }
    }

    public void Toggle()
    {
        if (settingsPanel == null)
        {
            Debug.LogError("Settings Panel is NOT assigned in the Inspector on " + gameObject.name, this);
            return;
        }

        bool willBeActive = !settingsPanel.activeSelf;

        settingsPanel.SetActive(willBeActive);
        Time.timeScale = willBeActive ? 0f : 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        Debug.Log("Settings Panel toggled. Is active: " + willBeActive);
    }
}
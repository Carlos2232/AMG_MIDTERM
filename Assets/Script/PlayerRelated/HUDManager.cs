using UnityEngine;

public class GameHUDManager : MonoBehaviour
{
    [Header("HUD Elements to Hide (Drag your HUD panel or objects here)")]
    public GameObject hudContainer; 

    [Header("Optional Panel References")]
    public GameObject settingsPanel; 
    public GameObject levelUpPanel;  

    void Update()
    {
        if (hudContainer == null) return;

        bool isSettingsOpen = (settingsPanel != null && settingsPanel.activeSelf);
        bool isLevelUpOpen = (levelUpPanel != null && levelUpPanel.activeSelf);

        if (isSettingsOpen || isLevelUpOpen)
        {
            if (hudContainer.activeSelf) hudContainer.SetActive(false);
        }
        else
        {
            if (!hudContainer.activeSelf) hudContainer.SetActive(true);
        }
    }
}
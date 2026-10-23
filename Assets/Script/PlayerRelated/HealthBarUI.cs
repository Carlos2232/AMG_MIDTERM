using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider; 
    public PlayerController playerController; 
    public TextMeshProUGUI HPText;

    void Start()
    {
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();

        if (playerController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
            }
        }

        if (playerController != null && healthSlider != null)
        {
            healthSlider.maxValue = playerController.maxHP;
            healthSlider.value = playerController.currentHP;
        }
    }

    void Update()
    {
        if (playerController != null && healthSlider != null)
        {
            healthSlider.maxValue = playerController.maxHP;
            healthSlider.value = playerController.currentHP;

            if (HPText != null)
            {
                HPText.text = Mathf.RoundToInt(playerController.currentHP) + " / " + Mathf.RoundToInt(playerController.maxHP);
            }
        }
    }
}
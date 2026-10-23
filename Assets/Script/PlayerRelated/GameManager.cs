using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Timer / Survival Settings")]
    public float totalSurvivalTime = 480f; 
    private float currentTime;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentTime = totalSurvivalTime;
    }

    void Start()
    {
        Time.timeScale = 1f; 
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                TriggerWin();
            }
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (AudioManager.Instance != null)
        {
            if (AudioManager.Instance.musicSource != null)
            {
                AudioManager.Instance.musicSource.Stop();
            }
            AudioManager.Instance.PlayGameOverSound();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Debug.Log("Game Over: Player HP reached 0.");
    }

    void TriggerWin()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (AudioManager.Instance != null)
        {
            if (AudioManager.Instance.musicSource != null)
            {
                AudioManager.Instance.musicSource.Stop();
            }
            AudioManager.Instance.PlayWinSound();
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        Debug.Log("Victory: Survived the full 8 minutes.");
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RestartMusic();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RestartMusic();
        }

        SceneManager.LoadScene("MainMenu");
    }

    
}
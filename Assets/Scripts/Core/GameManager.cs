using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateState(GameState.Playing);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                UpdateState(GameState.Pause);
            }
            else if (currentState == GameState.Pause)
            {
                UpdateState(GameState.Playing);
            }
        }
    }

    public void UpdateState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Pause:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 1f; 
                Debug.Log("Game Over!");
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
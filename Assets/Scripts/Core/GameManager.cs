using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public static GameManager Instance;
    public GameState currentState;

    private InputAction pauseAction;

    // supaya input tidak kebaca 2x
    private bool canPause = true;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Input ESC khusus pause
        pauseAction = new InputAction(
            name: "Pause",
            type: InputActionType.Button,
            binding: "<Keyboard>/escape"
        );

        pauseAction.performed += OnPausePressed;
        pauseAction.Enable();
    }

    void Start()
    {
        currentState = GameState.Playing;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        // Cegah spam input
        if (!canPause)
            return;

        // Jangan pause saat game over
        if (currentState == GameState.GameOver)
            return;

        // Jangan pause kalau sedang klik UI
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        // Toggle pause
        if (currentState == GameState.Playing)
        {
            PauseGame();
        }
        else if (currentState == GameState.Pause)
        {
            ResumeGame();
        }

        StartCoroutine(PauseDelay());
    }

    IEnumerator PauseDelay()
    {
        canPause = false;

        // realtime supaya tetap jalan saat TimeScale = 0
        yield return new WaitForSecondsRealtime(0.2f);

        canPause = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        currentState = GameState.Pause;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        currentState = GameState.Playing;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);

        Debug.Log("Game Resumed");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        currentState = GameState.GameOver;

        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnDestroy()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
            pauseAction.Disable();
            pauseAction.Dispose();
        }
    }
}
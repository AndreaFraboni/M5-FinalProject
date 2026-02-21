using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    // UI referements
    public GameObject pauseMenu;
    public GameObject gameOver;
    public GameObject menuGameOver;
    public GameObject WinnerBanner;
    public GameObject menuWinner;

    public bool isPaused = false;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("ThemeGame");
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("MouseClickSound");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Restart()
    {
        AudioManager.Instance.StopAllAudioSource();
        gameOver.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        AudioManager.Instance.StopAllAudioSource();

        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        AudioManager.Instance.StopAllAudioSource();
        gameOver.SetActive(true);
        Invoke("ShowGameOverMenu", 1f);
    }

    public void ShowGameOverMenu()
    {
        AudioManager.Instance.PlayMusic("GameOverMusic");
        gameOver.SetActive(false);
        Time.timeScale = 0;
        menuGameOver.SetActive(true);
    }
    public void Winner()
    {
        AudioManager.Instance.StopAllAudioSource();
        WinnerBanner.SetActive(true);
        Invoke("ShowWinnerMenu", 1f);
    }

    public void ShowWinnerMenu()
    {
        WinnerBanner.SetActive(false);
        AudioManager.Instance.PlayMusic("WinnerMusic");
        Time.timeScale = 0;
        menuWinner.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		     Application.Quit();
#endif
        }
    }
}

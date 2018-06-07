using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public static bool messageTextState = false;
    public GameObject pauseMenuUI;
    public GameObject pauseButton;
    public GameObject messageText;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    public void PauseButton()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1F;
        GameIsPaused = false;
        pauseButton.SetActive(true);
        CardAlgorithm.playState = true;
        messageText.SetActive(messageTextState);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0F;
        GameIsPaused = true;
        pauseButton.SetActive(false);
        CardAlgorithm.playState = false;
        messageText.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");        
    }

    public void Hakkinda()
    {

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}

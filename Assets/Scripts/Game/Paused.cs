using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paused : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;

        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        isPaused = true;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Home");
    }
}

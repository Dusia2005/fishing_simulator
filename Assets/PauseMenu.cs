using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Универсальный метод для перехода на сцену по индексу
    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f; // Включаем время при переходе
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadMenu() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
    }
}

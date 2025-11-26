using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для кнопки "НАЧАТЬ"
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Метод для кнопки "ВЫХОД"
    public void ExitGame()
    {
        Debug.Log("Игра Закрылась");
        Application.Quit();
    }
}

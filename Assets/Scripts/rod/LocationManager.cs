using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LocationManager : MonoBehaviour
{
    public LocationData[] locations;
    public PauseMenu pauseMenu; // ссылка на PauseMenu

    private void Start()
    {
        UpdateLocationAccess();
    }

    // Проверяем, какие локации доступны
    public void UpdateLocationAccess()
    {
        if (PlayerProgress.Instance == null) return;

        int score = PlayerProgress.Instance.totalScore;

        foreach (var loc in locations)
        {
            if (loc.button != null)
                loc.button.interactable = (score >= loc.requiredScore);
        }
    }

    // Вызывается при нажатии кнопки
    public void OnLocationButtonClicked(int index)
    {
        if (PlayerProgress.Instance.totalScore >= locations[index].requiredScore)
        {
            if (pauseMenu != null)
            {
                pauseMenu.LoadSceneByIndex(locations[index].sceneIndex);
            }
        }
        else
        {
            Debug.Log("Недостаточно очков для открытия локации");
        }
    }
}

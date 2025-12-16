using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    public Text scoreText;
    public Image fillBar;
    public Text totalScoreText;
    public Text pointsToNextText;
    public LocationManager locationManager; // ссылка на LocationManager

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (PlayerProgress.Instance == null || locationManager == null) return;

        int totalScore = PlayerProgress.Instance.totalScore;

        // Ищем следующую локацию, которая ещё не доступна
        LocationData nextLocation = null;
        foreach (var loc in locationManager.locations)
        {
            if (totalScore < loc.requiredScore)
            {
                nextLocation = loc;
                break;
            }
        }

        if (nextLocation != null)
        {
            int pointsToNext = Mathf.Max(nextLocation.requiredScore - totalScore, 0);
            scoreText.text = $"Всего очков: {totalScore}\n " +
                $"До следующей локации: {pointsToNext}";


        }

    }
}

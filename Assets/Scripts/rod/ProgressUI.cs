using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    public Text progressText;
    public Text fishCountText;

    public LocationManager locationManager;
    public int totalFishTypes; // Сколько ВСЕГО видов рыб в игре

    void Update()
    {
        UpdateFishCount();
        UpdateLocationProgress();
    }

    void UpdateFishCount()
    {
        int caught = PlayerProgress.Instance.GetCaughtFishCount();
        fishCountText.text = $"Видов рыб поймано: {caught}/{totalFishTypes}";
    }

    void UpdateLocationProgress()
    {
        int totalScore = PlayerProgress.Instance.totalScore;

        LocationData nextLocation = GetNextLockedLocation();

        if (nextLocation == null)
        {
            //  ПОСЛЕДНЯЯ ЛОКАЦИЯ ОТКРЫТА
            progressText.text = "Все локации открыты!";
        }
        else
        {
            int pointsToNext = Mathf.Max(nextLocation.requiredScore - totalScore, 0);

            progressText.text =
                $"Очки: {totalScore}/{nextLocation.requiredScore}\n" +
                $"До следующей локации: {pointsToNext}";
        }
    }

    LocationData GetNextLockedLocation()
    {
        foreach (var loc in locationManager.locations)
        {
            if (PlayerProgress.Instance.totalScore < loc.requiredScore)
                return loc;
        }

        return null; // все открыты
    }
}

using UnityEngine;
using System.Collections.Generic;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

    public int totalScore;

    //  Уникальные пойманные рыбы
    private HashSet<string> caughtFishTypes = new HashSet<string>();

    private void Awake()
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

    public void AddScore(int amount)
    {
        totalScore += amount;
    }

    public void RegisterFish(string fishName)
    {
        caughtFishTypes.Add(fishName);
    }

    public int GetCaughtFishCount()
    {
        return caughtFishTypes.Count;
    }
}

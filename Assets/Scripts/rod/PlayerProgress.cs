using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;
    public int totalScore;

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
        Debug.Log("Очки: " + totalScore);
    }
}

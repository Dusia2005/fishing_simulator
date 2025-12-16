using UnityEngine;

[System.Serializable]
public class FishData
{
    public string fishName;
    public Sprite fishSprite;

    [Range(0f, 100f)]
    public float spawnChance;

    public int scoreValue;
}

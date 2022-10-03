using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 1)]
public class GameConfiguration : ScriptableObject
{
    public MapInstance[] Maps;
    [Header("Game")]
    public int InitialSlots = 3;
    public int InitialCoins = 3;
    /// <summary>
    /// Trial 0 is intro. Reroll cost -1 means reroll is disabled
    /// </summary>
    public int[] RerollCostPerTrialIndex = new int[20];
    public UnitSpawnConfiguration[] RecruitsConfigPerTrialIndex = new UnitSpawnConfiguration[20];

    [Header("UI")]
    [Range(1,10)]
    public float FadeSpeed = 2.0f;
    public float AnnouncementDuration = 3.0f;
    public string SpriteAtlas_Coin = "<sprite name=\"coin\">";
    [Header("Audio")]
    public AudioClip ClickSound;
    public AudioClip AnnouncementSound;
}

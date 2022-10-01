using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 1)]
public class GameConfiguration : ScriptableObject
{
    public MapInstance[] Maps;
    [Range(1,10)]
    public float FadeSpeed = 2.0f;
    public float AnnouncementDuration = 3.0f;

    public AudioClip ClickSound;
    public AudioClip AnnouncementSound;
}

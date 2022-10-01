using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration", order = 1)]
public class GameConfiguration : ScriptableObject
{
    public MapInstance[] Maps;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    [SerializeField]
    GameConfiguration configuration;

    public GameConfiguration GetConfiguration() { return configuration; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameInstance : MonoBehaviour
{
    [SerializeField]
    GameConfiguration configuration;

    public GameState GameState { get; private set; }

    public GameConfiguration Configuration { get { return configuration; } }

    private void Awake()
    {
        GameState = new GameState();        
    }
}

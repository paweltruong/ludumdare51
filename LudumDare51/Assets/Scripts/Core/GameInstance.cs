using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameInstance : MonoBehaviour
{
    [SerializeField]
    GameConfiguration configuration;

    public GameState GameState { get; private set; }

    public UnityEvent<int> OnTotalCoinsChanged;

    public GameConfiguration Configuration { get { return configuration; } }

    private void Start()
    {
        GameState = new GameState();
    }
}

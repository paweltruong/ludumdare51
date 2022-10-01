using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public SceneManager SceneManager { get; private set; }
    public GameInstance GameInstance { get; private set; }
    public AudioManager AudioManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        SceneManager = GetComponentInChildren<SceneManager>();
        GameInstance = GetComponentInChildren<GameInstance>();
        AudioManager = GetComponentInChildren<AudioManager>();
    }
}

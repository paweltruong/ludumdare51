using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

public class Singleton : MonoBehaviour
{
    [SerializeField] SceneManager _sceneManager;
    [SerializeField] GameInstance _gameInstance;
    [SerializeField] AudioManager _audioManager;

    public static Singleton Instance { get; private set; }
    public SceneManager SceneManager => _sceneManager;
    public GameInstance GameInstance => _gameInstance;
    public AudioManager AudioManager => _audioManager;

    private void Awake()
    {
        //fix this bug https://forum.unity.com/threads/errors-with-the-urp-debug-manager.987795/
        DebugManager.instance.enableRuntimeUI = false;
     
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Assert.IsNotNull(SceneManager);
        Assert.IsNotNull(GameInstance);
        Assert.IsNotNull(AudioManager);      
    }
}

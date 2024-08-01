using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject splashScreen;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private SaveSystem _saveSystem;

    private string _currentScene = "";

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;

        SceneManager.sceneLoaded += Initialize;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {

    }

    private void Initialize(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Loaded Game Manager");
        var playerController = FindObjectOfType<PlayerController>();
        if(playerController != null ) player = playerController.gameObject;

        _saveSystem = FindObjectOfType<SaveSystem>();

        if(_saveSystem == null) return;

        if(player != null && _saveSystem.LoadedData != null)
        {
            var health = player.GetComponent<PlayerController>();
            health.CurrentPlayerHealth = _saveSystem.LoadedData.playerHealth;
        }
    }

    public void LoadSave()
    {
        if (_saveSystem.LoadedData != null)
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex(_saveSystem.LoadedData.sceneIndex);
            LoadScene(sceneName);
        }
    }

    public void LoadScene(string sceneName)
    {
        splashScreen.SetActive(true);

        if (_currentScene != "")
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _currentScene = sceneName;
        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnNewSceneLoaded;
    }

    public void SaveData()
    {
        if(player != null)
        {
            _saveSystem.SaveData(SceneManager.GetActiveScene().buildIndex + 1, player.GetComponent<PlayerController>().CurrentPlayerHealth);
        }
    }

    private void OnNewSceneLoaded(Scene sceneName, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(sceneName);

        splashScreen.SetActive(false);

        SceneManager.sceneLoaded -= OnNewSceneLoaded;
    }
}

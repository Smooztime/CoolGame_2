using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    [SerializeField]
    private string playerHealthKey = "Player Health", sceneKey = "SceneIndex", savePresentKey = "Save Present";

    public LoadedData LoadedData {  get; private set; }

    public UnityEvent<bool> OnDataLoadedResult;
    public UnityEvent<bool> OnSaveDataCheck;

    public static SaveSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var result = LoadData();
        OnDataLoadedResult?.Invoke(result);

    }

    public bool LoadData()
    {
        bool dataPresent = PlayerPrefs.GetInt(savePresentKey) == 1;
        if (dataPresent)
        {
            LoadedData = new LoadedData();
            LoadedData.playerHealth = PlayerPrefs.GetInt(playerHealthKey);
            LoadedData.sceneIndex = PlayerPrefs.GetInt(sceneKey);
            return true;
        }
        OnSaveDataCheck?.Invoke(dataPresent);
        return dataPresent;
    }

    public void SaveData(int sceneIndex, int playerHealth)
    {
        if(LoadedData == null) LoadedData = new LoadedData();

        LoadedData.playerHealth = playerHealth;
        LoadedData.sceneIndex = sceneIndex;
        PlayerPrefs.SetInt(playerHealthKey, playerHealth);
        PlayerPrefs.SetInt(sceneKey, sceneIndex);
        PlayerPrefs.SetInt(savePresentKey, 1);

    }

    public void ResetData()
    {
        PlayerPrefs.DeleteKey(playerHealthKey);
        PlayerPrefs.DeleteKey(sceneKey);
        PlayerPrefs.DeleteKey(savePresentKey);
        LoadedData = null;
    }
}

public class LoadedData
{
    public int playerHealth = -1;
    public int sceneIndex = -1;
}

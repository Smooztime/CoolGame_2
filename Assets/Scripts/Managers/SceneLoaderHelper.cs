using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderHelper : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoadOnAwake;

    private string currentSceneName;

    
    public void LoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }

    public void restart()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }

    public void LoadSave()
    {
        GameManager.Instance.LoadSave();
    }

    private void Start()
    {
        if(sceneToLoadOnAwake != "") LoadScene(sceneToLoadOnAwake);
    }
}

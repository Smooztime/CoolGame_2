using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private SaveSystem _saveSystem;

    private void Awake()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ShowUI();
    }

    private void ShowUI()
    {
        if (Time.timeScale == 0f)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
    }

    public void LoadData()
    {
        _saveSystem.LoadData();
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.OpenURL("about:Blank");
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;

    private void OnEnable()
    {
        SaveSystem.instance.OnSaveDataCheck.AddListener(UpdateButtonInteractable);
    }

    private void OnDisable()
    {
        SaveSystem.instance.OnSaveDataCheck.RemoveListener(UpdateButtonInteractable);
    }

    private void Start()
    {
        UpdateButtonInteractable(SaveSystem.instance.LoadData());
    }

    private void UpdateButtonInteractable(bool hasSaveData)
    {
        continueButton.interactable = hasSaveData;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("First_Level");
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

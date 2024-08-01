using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorExit : MonoBehaviour
{
    private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            GameManager.Instance.SaveData();
            var nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            nextSceneName = SceneUtility.GetScenePathByBuildIndex(nextScene);
            GameManager.Instance.LoadScene(nextSceneName);
        }
    }
}

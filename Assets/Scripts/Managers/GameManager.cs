using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void Pause()
    {
        Time.timeScale = 0f;
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
    }

    public static void ExitGame() //To stop playing game
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

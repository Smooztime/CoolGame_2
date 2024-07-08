using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorTexture;
    [SerializeField]
    private Vector2 cursorPosition;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, cursorPosition, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

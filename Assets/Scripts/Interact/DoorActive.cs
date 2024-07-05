using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActive : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lightTriggerOn;

    private bool canOpenDoor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject on in lightTriggerOn)
        {
            if (on.gameObject.activeSelf)
            {
                canOpenDoor = true;
            }
            else
            {
                canOpenDoor = false;
            }
        }
        if (canOpenDoor == true)
        {
            gameObject.SetActive(false);
        }
    }
}

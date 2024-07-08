using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActive : MonoBehaviour
{
    [SerializeField] 
    private LightTrigger[] lightTriggerOn;
    [SerializeField]
    private GameObject door;

    private bool allLightOn;

    private void FixedUpdate()
    {
        CheckLight();
    }

    private void CheckLight()
    {
        allLightOn = false;
        foreach (var lightTrigger in lightTriggerOn)
        {
            if (!lightTrigger.IsLightOn())
            {
                allLightOn = true;
                break;
            }
            
        }

        if (!allLightOn)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        door.gameObject.SetActive(false);
    }

    private void CloseDoor()
    {
        door.gameObject.SetActive(true);
    }
}

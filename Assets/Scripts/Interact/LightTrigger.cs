using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject lightOn;
    [SerializeField] private GameObject lightOff;

    private bool _isLightOn = false;

    void Start()
    {
        
        lightOn.SetActive(_isLightOn);
        lightOff.SetActive(!_isLightOn);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            ToggleLights();
        }
    }

    // Toggle lights method
    public void ToggleLights()
    {
        _isLightOn = !_isLightOn;

        lightOn.SetActive(_isLightOn);
        lightOff.SetActive(!_isLightOn);
    }
}

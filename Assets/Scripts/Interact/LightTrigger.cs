using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject lightOn;
    [SerializeField] private GameObject lightOff;

    private bool _isLightOn; //check _isLightOn for now is false

    void Start()
    {
        lightOn.SetActive(_isLightOn); //_isLightOn is false
        lightOff.SetActive(!_isLightOn); //_isLightOn is true
    }

    private void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            ToggleLights();
        }
    }
    public void ToggleLights()
    {
        _isLightOn = !_isLightOn; //change value of _isLightOn = true because !_isLightOn = true

        lightOn.SetActive(_isLightOn); //_isLightOn is true
        lightOff.SetActive(!_isLightOn); //_isLightOn is false
    }

    public virtual bool IsLightOn()
    {
        return _isLightOn;
    }
}

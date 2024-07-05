using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;
    [SerializeField]
    private Image fill;
    [SerializeField]
    Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(float maxHP, float currentHP)
    {
        healthBarSlider.value = currentHP / maxHP; //Set up Health to player
        fill.color = gradient.Evaluate(healthBarSlider.normalizedValue);
    }
}

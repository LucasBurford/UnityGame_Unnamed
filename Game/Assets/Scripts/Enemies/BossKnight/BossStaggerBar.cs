using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStaggerBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        
    }

    public void SetSlider(float amount)
    {
        slider.value = amount;
    }
}

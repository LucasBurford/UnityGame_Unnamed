using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightMove : MonoBehaviour
{
    // Target to follow player
    public Transform target;

    public bool shouldFollowPlayer;

    // Self light
    public Light2D light2D;

    // Intensity change factor
    public float intensityChange;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise values
        intensityChange = 0.0001f;
    }

    private void Update()
    {
        if (shouldFollowPlayer)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }

    public void Increase()
    {
        light2D.intensity += intensityChange;
    }

    public void Decrease()
    {
        light2D.intensity -= intensityChange;
    }

    public void ChangeIntensity(float newIntensity)
    {
        light2D.intensity = newIntensity;
    }
}

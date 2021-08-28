using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PowerupLight : MonoBehaviour
{
    public Light2D light2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        light2D.intensity = Mathf.PingPong(Time.time, 3);
    }
}

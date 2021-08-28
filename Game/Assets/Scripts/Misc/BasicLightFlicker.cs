using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BasicLightFlicker : MonoBehaviour
{
    public Light2D mLight;

    // Update is called once per frame
    void Update()
    {
        mLight.intensity = Mathf.PingPong(Time.time, 1);
    }
}

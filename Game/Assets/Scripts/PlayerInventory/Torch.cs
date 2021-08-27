using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Torch : MonoBehaviour
{
    public Light2D light2D;

    // Start is called before the first frame update
    void Start()
    {
        light2D.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !light2D.enabled)
        {
            light2D.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && light2D.enabled)
        {
            light2D.enabled = false;
        }
    }
}

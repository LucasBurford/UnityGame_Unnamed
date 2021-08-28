using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FireFlicker : MonoBehaviour
{
    // Hold reference to parent light
    public Light2D mLight;

    public ParticleSystem ps;
    public ParticleSystem.MainModule main;

    public float energy;

    public bool isDrained;

    // Start is called before the first frame update
    void Start()
    {
        isDrained = false;
        energy = 3;
        main = ps.main;
    }

    // Update is called once per frame
    void Update()
    {
        // If energy is less than or equal to 0
        if (energy <= 0.01f)
        {
            energy = 0;

            // Set drained to true
            isDrained = true;
        }
        else
        {
            isDrained = false;
        }

        // If energy is drained...
        if (isDrained)
        {
            // Turn light brightness way down
            mLight.intensity = 0.1f;

            // Turn fire particles way down
            if (ps != null)
            {
                main.startLifetime = 0.2f;
            }

            StartCoroutine(Wait());
        }
        // Else if fire still has energy
        else
        {
            // Flicker the fire based on energy
            mLight.intensity = Mathf.PingPong(Time.time, energy);

            // Turn fire particles back up
            if (ps != null)
            {
                main.startLifetime = 5;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(30);
        energy = 3;
    }
}

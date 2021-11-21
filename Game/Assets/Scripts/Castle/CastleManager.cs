using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayRandomSound();
    }

    public void PlayRandomSound()
    {
        if (Random.Range(0, 1000) == 100)
        {
            FindObjectOfType<AudioManager>().Play("CastleMetalCreak");
        }
    }
}

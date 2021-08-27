using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z); 
    }

    public void ChangeAudioClip(AudioClip newClip)
    {
        gameObject.GetComponent<AudioSource>().clip = newClip;
    }
}

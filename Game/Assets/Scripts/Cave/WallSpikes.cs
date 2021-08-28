using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpikes : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        // Initially shoot forward
        ShootForward();
    }
    private void ShootForward()
    {
        transform.position = new Vector3(-211, -112, 0);

        // Wait 
        StartCoroutine(Wait(1.5f, 1));
    }

    private void Retract()
    {
        transform.position = new Vector3(-213, -112, 0);

        // Wait 
        StartCoroutine(Wait(1.5f, 2));
    }

    IEnumerator Wait(float seconds, int state)
    {
        // Wait for seconds
        yield return new WaitForSeconds(seconds);

        switch (state)
        {
            case 1:
                {
                    Retract();
                }
                break;

            case 2:
                {
                    ShootForward();
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            gameManager.Die("Impaled by spikes");
        }
    }
}

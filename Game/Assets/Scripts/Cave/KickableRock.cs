using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickableRock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindObjectOfType<AudioManager>().Play("RockHit1");
    }
}

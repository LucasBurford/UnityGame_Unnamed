using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCollider : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerMovement.surface = PlayerMovement.Surface.stone;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovement.surface = PlayerMovement.Surface.grass;
    }
}

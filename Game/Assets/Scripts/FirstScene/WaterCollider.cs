using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "PondFlower")
        {
            // Set walking sound to water one
            playerMovement.surface = PlayerMovement.Surface.water;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovement.surface = PlayerMovement.Surface.grass;
    }
}

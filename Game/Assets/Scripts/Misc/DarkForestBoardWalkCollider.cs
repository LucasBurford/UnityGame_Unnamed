using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkForestBoardWalkCollider : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Set walking sound to water one
            playerMovement.surface = PlayerMovement.Surface.darkForestWood;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovement.surface = PlayerMovement.Surface.grass;
    }
}

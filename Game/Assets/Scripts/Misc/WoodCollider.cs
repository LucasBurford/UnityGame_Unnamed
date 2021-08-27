using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCollider : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.surface = PlayerMovement.Surface.wood;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerMovement.surface = PlayerMovement.Surface.grass;
    }
}

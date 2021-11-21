using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void Update()
    {
        transform.Rotate(0, 3, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            playerMovement.canDodge = true;

            FindObjectOfType<ItemAcquired>().DisplayItemText("You can now dodge! Press Space while moving to dodge", 5);

            Destroy(gameObject);
        }
    }
}

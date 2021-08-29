using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMakeshiftShield : MonoBehaviour
{
    public GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            gameManager.hasShield = true;

            gameManager.SetCheckpoint(transform.position);

            FindObjectOfType<ItemAcquired>().DisplayItemText("Shield", "Use to block attacks", "Right Click", 6);

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntry : MonoBehaviour
{
    public PlayerMovement player;
    public GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Set checkpoint to entry point of cave
            gameManager.SetCheckpoint(new Vector3(-143,-107,0));

            // Set player position to entry point of cave
            player.SetPosition(new Vector3(-143, -107, 0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHouseDoor : MonoBehaviour
{
    public GameObject player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            player.transform.position = new Vector3(-506.5f, 80.5f, 0);
        }
    }
}

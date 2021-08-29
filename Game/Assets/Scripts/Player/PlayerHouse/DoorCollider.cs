using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    public GameObject player, playerHouse;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.transform.position = new Vector3(-23.5f, -1, 0);
        playerHouse.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCollider : MonoBehaviour
{
    public PlayerInventory playerInventory;

    public GameObject torchToDestroy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject torch = GameObject.Find("Torch");
        playerInventory.playerInventory.Add(torch);

        Destroy(torchToDestroy);
    }
}

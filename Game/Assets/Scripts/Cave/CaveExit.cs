using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveExit : MonoBehaviour
{
    public GameObject player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            player.transform.position = new Vector3(-15, -9.8f, 0);
        }
    }
}

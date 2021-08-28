using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveExit : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement player;

    private int x;

    private void Update()
    {
        // Generate random numbers to play random drip sounds
        x = Random.Range(0, 100);

        switch (x)
        {
            //case 1:
            //    {
            //        PlayRandomSound("")
            //    }
        }
    }

    public void PlayRandomSound(string clip)
    {
        FindObjectOfType<AudioManager>().Play(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            StartCoroutine(gameManager.AreaCrossfade(2));
            player.SetPosition(new Vector3(-15, -9.8f, 0));
        }
    }
}

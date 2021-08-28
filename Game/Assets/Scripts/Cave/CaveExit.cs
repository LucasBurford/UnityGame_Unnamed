using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveExit : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement player;

    private float x;
    public bool isInCave;

    private void Update()
    {
        if (isInCave)
        {
            // Generate random numbers to play random drip sounds
            x = Random.Range(0, 1000);

            switch (x)
            {
                case 10:
                    {
                        PlayRandomSound("WaterDrip1");
                    }
                    break;

                case 20:
                    {
                        PlayRandomSound("WaterDrip2");
                    }
                    break;

                case 30:
                    {
                        PlayRandomSound("WaterDrip3");
                    }
                    break;

                case 40:
                    {
                        PlayRandomSound("WaterDrip4");
                    }
                    break;

                case 50:
                    {
                        PlayRandomSound("WaterDrip5");
                    }
                    break;

                case 60:
                    {
                        PlayRandomSound("WaterDrip6");
                    }
                    break;

                case 70:
                    {
                        PlayRandomSound("WaterDrip7");
                    }
                    break;

                case 80:
                    {
                        PlayRandomSound("WaterDrip8");
                    }
                    break;

                case 90:
                    {
                        PlayRandomSound("WaterDrip9");
                    }
                    break;

                case 100:
                    {
                        PlayRandomSound("WaterDrip10");
                    }
                    break;

                case 0.1f:
                    {
                        PlayRandomSound("CaveMonsterGrowl");
                    }
                    break;

                case 500:
                    {
                        PlayRandomSound("CaveMonsterGrowl");
                    }
                    break;
            }
        }
        else
        {
            return;
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

            isInCave = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HealingPlantBig : MonoBehaviour
{
    public GameManager gameManager;

    public Light2D light2D;

    // Update is called once per frame
    void Update()
    {
        light2D.intensity = Mathf.PingPong(Time.time, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            gameManager.currentHealth = gameManager.maxHealth;
            FindObjectOfType<AudioManager>().Play("PlayerHealBig");
            Destroy(gameObject);
        }
    }
}

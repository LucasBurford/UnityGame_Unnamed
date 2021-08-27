using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HealingPlant : MonoBehaviour
{
    public GameManager gameManager;

    public float healingFactor;

    public Light2D light2D;

    // Start is called before the first frame update
    void Start()
    {
        healingFactor = 10;
    }

     void Update()
    {
        light2D.intensity = Mathf.PingPong(Time.time, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight" && gameManager.currentHealth < gameManager.maxHealth)
        {
            gameManager.Heal(healingFactor);

            int rand = Random.Range(1, 4);

            switch(rand)
            {
                case 1:
                    {
                        FindObjectOfType<AudioManager>().Play("PlayerHeal1");
                    }
                    break;
                case 2:
                    {
                        FindObjectOfType<AudioManager>().Play("PlayerHeal2");
                    }
                    break;
                case 3:
                    {
                        FindObjectOfType<AudioManager>().Play("PlayerHeal3");
                    }
                    break;
            }
            
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "Player_Knight" && gameManager.currentHealth >= gameManager.maxHealth && gameManager.canHeal)
        {
            gameManager.collectedHealingPlants++;
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : MonoBehaviour
{
    public PlayerAttacks playerAttacks;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Double melee damage
            playerAttacks.meleeDamage *= 2;

            FindObjectOfType<ItemAcquired>().DisplayItemText("Melee damage increaed!", 5);

            Destroy(gameObject);
        }
    }
}

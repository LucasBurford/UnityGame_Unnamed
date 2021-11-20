using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public OctopusBoss boss;
    public int timesTriggered;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timesTriggered++;

            // Spawn the boss when the player walks back over the trigger
            if (timesTriggered == 2)
            {
                boss.SpawnBoss();
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;

public class KnightStartCollider : MonoBehaviour
{
    // Reference to camera to access bg music
    public Camera cam;

    // Reference to boss fight script
    public BossKnight bossKnight;
    public GameObject bossKnightGO;

    // Reference to Knight Start text
    public TMP_Text knightStartText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Set knight to active and commence the fight
            bossKnightGO.SetActive(true);
            bossKnight.isActive = true;

            bossKnight.GetComponent<AIPath>().canMove = true;

            // Set the text
            knightStartText.text = "Boss fight! \n\n -Village Knight- \n\nUse Fire to stagger him!";
            StartCoroutine(RemoveText());

            // Play impact sound
            FindObjectOfType<AudioManager>().Play("KnightStartSound");

            // Stop OG BG music
            cam.GetComponent<AudioSource>().Stop();

            // Change music
            FindObjectOfType<AudioManager>().Play("KnightFightBGMusic");
        }
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(6);
        Destroy(knightStartText);
        Destroy(gameObject);
    }
}

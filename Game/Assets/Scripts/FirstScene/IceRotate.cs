using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class IceRotate : MonoBehaviour
{
    // Reference to Game Manager
    public GameManager gameManager;

    public GameObject enemy;

    public Light2D light2D;

    // Reference to powerup text
    public TMP_Text iceAttainedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the sprite
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Give the ice ability
            gameManager.ice = true;
            // Set ice to active attack
            gameManager.powerups = GameManager.Powerups.ice;

            // Enable the text
            iceAttainedText.enabled = true;

            // Set the text
            iceAttainedText.text = "New ability gained: \n ICE";

            // Set enemy to active
            enemy.SetActive(true);

            // Move object away
            gameObject.transform.position = new Vector3(1000, 1000);

            // Destroy light
            Destroy(light2D);

            // Remove text and object after 4 seconds
            StartCoroutine(RemoveText());
        }
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(4);

        Destroy(iceAttainedText);
        Destroy(gameObject);
    }
}

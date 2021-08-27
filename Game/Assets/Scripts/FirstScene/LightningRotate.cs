using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class LightningRotate : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject enemy;

    public Light2D light2D;

    public TMP_Text lightningAttainedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Give the powerup
            gameManager.lightning = true;
            // Automatically switch to it so player can use it immediately
            gameManager.powerups = GameManager.Powerups.lightning;

            // Show lightning gained text
            lightningAttainedText.enabled = true;
            // Set text
            lightningAttainedText.text = "New ability gained: LIGHTNING \n Use 1,2,3,4 to switch between powers";
            // Destroy light
            Destroy(light2D);
            // Set enemy to active
            enemy.SetActive(true);
            // Move object away
            gameObject.transform.position = new Vector3(1000, 1000);
            // Remove text after 7 seconds
            StartCoroutine(RemoveText());
        }
    }

    IEnumerator RemoveText()
    {
        // Wait for 6 seconds
        yield return new WaitForSeconds(6);
        // Destroy lightning game object and text
        Destroy(lightningAttainedText);
        Destroy(gameObject);
    }
}

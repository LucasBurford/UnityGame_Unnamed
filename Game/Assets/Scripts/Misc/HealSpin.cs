using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealSpin : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text text;

    private void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Give the player the healing ability
            gameManager.canHeal = true;

            // Enable text
            text.GetComponent<TextMeshProUGUI>().enabled = true;

            // Inform the player
            text.text = "You can now collect Healing Plants if your health is full. Press E to use them.";

            transform.position = new Vector3(1000, 1000, 0);

            // Wait
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);

        text.GetComponent<TextMeshProUGUI>().enabled = false;

        Destroy(gameObject);
    }
}

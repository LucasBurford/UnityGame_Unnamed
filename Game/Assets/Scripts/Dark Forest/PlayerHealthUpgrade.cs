using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUpgrade : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.maxHealth = 200;

        // Play sound
        FindObjectOfType<AudioManager>().Play("PowerupAcquire");

        // Display text
        text.gameObject.SetActive(true);
        gameManager.currentHealth = gameManager.maxHealth;

        transform.position = new Vector3(2000, 2000, 2);

        StartCoroutine(WaitToRemove());
    }

    IEnumerator WaitToRemove()
    {
        yield return new WaitForSeconds(2);
        text.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

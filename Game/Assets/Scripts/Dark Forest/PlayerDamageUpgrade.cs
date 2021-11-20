using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDamageUpgrade : MonoBehaviour
{
    public PlayerAttacks playerAttacks;
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Increase melee damage
        playerAttacks.MeleeDamage = 65;

        // Play sound
        FindObjectOfType<AudioManager>().Play("PowerupAcquire");

        // Display text
        text.gameObject.SetActive(true);

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

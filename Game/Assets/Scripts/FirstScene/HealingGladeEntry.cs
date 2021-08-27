using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealingGladeEntry : MonoBehaviour
{
    public TMP_Text text;
    public bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight" && !hasTriggered)
        {
            hasTriggered = true;

            // Display the text
            text.GetComponent<TextMeshProUGUI>().enabled = true;
            text.text = "Healing Glade";

            // Wait
            StartCoroutine(Wait());
        }
        else if (collision.gameObject.name == "Player_Knight" && hasTriggered)
        {
            hasTriggered = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);

        text.enabled = false;
    }
}

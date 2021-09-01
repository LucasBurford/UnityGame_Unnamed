using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DarkForestEntry : MonoBehaviour
{
    public GameObject darkForestArea;

    public TMP_Text darkForestEntryText;

    public bool isActive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            if (!isActive)
            {
                // Enable area
                darkForestArea.SetActive(true);

                // Set isActive to true
                isActive = true;

                // Display text
                darkForestEntryText.GetComponent<TextMeshProUGUI>().enabled = true;
                darkForestEntryText.text = "Dark Forest";

                // Wait to remove text
                StartCoroutine(Wait(2));
            }
            else
            {
                // Display text
                darkForestEntryText.GetComponent<TextMeshProUGUI>().enabled = true;
                darkForestEntryText.text = "Highpoint Village";

                // Wait to remove text
                StartCoroutine(Wait(2));

                // Wait to disable areas
                StartCoroutine(Wait2(3));
            }
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        darkForestEntryText.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    IEnumerator Wait2(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Disable area
        darkForestArea.SetActive(false);

        // Set isActive back to false
        isActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartAreaEntry : MonoBehaviour
{
    public TMP_Text text;
    public int hasTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            hasTriggered++;
        }

        if (hasTriggered == 2)
        {
            text.GetComponent<TextMeshProUGUI>().enabled = true;
            text.text = "Home";

            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);

        text.GetComponent<TextMeshProUGUI>().enabled = false;

        hasTriggered = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemAcquired : MonoBehaviour
{
    public TMP_Text itemAcquiredText;

    public float fade;
    public float y;

    public bool rise;

    private void Update()
    {
        if (rise)
        {
            itemAcquiredText.rectTransform.Translate(0, y, 0);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Displays text of the item the player has just acquired
    /// </summary>
    /// <param name="item">Item just acquired</param>
    /// <param name="seconds">Time for text to disappear</param>
    public void DisplayItemText(string item, int seconds)
    {
        // Enable the text
        EnableDisable(true);

        // Set the text
        itemAcquiredText.text = "New item acquired! \n*" + item + "*";

        // Play item acquired sound
        FindObjectOfType<AudioManager>().Play("ItemAcquire");

        // Remove text after passed in time
        StartCoroutine(Wait(seconds));
    }

    private void EnableDisable(bool state)
    {
        itemAcquiredText.GetComponent<TextMeshProUGUI>().enabled = state;
    }

    IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        rise = true;

        itemAcquiredText.CrossFadeAlpha(0.0f, fade, false);

        yield return new WaitForSeconds(seconds);

        rise = false;

        EnableDisable(false);
    }
}

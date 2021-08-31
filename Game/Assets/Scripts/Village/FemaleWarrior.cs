using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleWarrior : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            dialogueTrigger.TriggerDialogue();

            FindObjectOfType<AudioManager>().Play("WizardSpeaking");
        }
    }
}

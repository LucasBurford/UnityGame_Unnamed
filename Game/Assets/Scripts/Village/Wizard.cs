using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameManager gameManager;

    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;

    // Bool to determine if the very first dialogue has been triggered
    public bool dialogue1HasTriggered;

    public ItemAcquired itemAcquired;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.hasEnded && !gameManager.hasMap)
        {
            // Give player the map
            gameManager.hasMap = true;

            // Display to player
            itemAcquired.DisplayItemText("Map", 5);
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            if (!dialogue1HasTriggered)
            {
                dialogue1HasTriggered = true;

                dialogueTrigger.TriggerDialogue();

                FindObjectOfType<AudioManager>().Play("WizardSpeaking");
            }
            else
            {
                Debug.Log("Come back later!");
            }
        }
    }
}

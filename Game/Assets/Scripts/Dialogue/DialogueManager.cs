using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    #region Members
    // List of sentences
    private Queue<string> sentences;

    public GameManager gameManager;

    public bool hasEnded;

    // Reference to animator
    public Animator animator;

    // References to dialogue text
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    // Character Mugshot
    public Image characterMugshot;
    #endregion

    #region Dialogue Events
    // Who is in converstaion enum - add to when needed
    public enum CharacterInConversationWith
    {
        none,
        player,
        wizard,
        femaleWarrior
    }
    public static CharacterInConversationWith characterInConversationWith;

    // Conversation ended delegate/event
    public delegate void DialogueEndedPlayer();
    public static event DialogueEndedPlayer OnDialogueEndPlayer;

    public delegate void DialogueEndedWizard();
    public static event DialogueEndedWizard OnDialogueEndWizard;

    public delegate void DialogueEndedFemaleWarrior();
    public static event DialogueEndedFemaleWarrior OnDialogueEndFemaleWarrior;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        characterInConversationWith = CharacterInConversationWith.none;
    }

    private void Update()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Set animation to true
        animator.SetBool("IsOpen", true);

        // Set bool in GameManager
        gameManager.isInDialogue = true;

        // Set name text
        nameText.text = dialogue.name;

        // Clear previous sentences
        sentences.Clear();

        // Loop through sentence queue 
        foreach(string sentence in dialogue.sentences)
        {
            // Add each one to the queue
            sentences.Enqueue(sentence);
        }

        // Display next sentence
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            hasEnded = EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        // Stop animating text
        StopAllCoroutines();

        // Display dialogue sentence letter by letter
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
        }

        yield return null;
    }

    public bool EndDialogue()
    {
        // Set animation to true
        animator.SetBool("IsOpen", false);

        // Set bool in Game Manager to false
        gameManager.isInDialogue = false;

        // Handle which event to raise
        CheckCharacterInConversationWith();

        // Return true when dialogue has ended
        return true;
    }

    private void CheckCharacterInConversationWith()
    {
        /// Handle which event to rasie
        switch (characterInConversationWith)
        {
            case CharacterInConversationWith.player:
                {
                    // Raise OnDialogueEnd event for Player
                    OnDialogueEndPlayer();
                }
                break;

            case CharacterInConversationWith.wizard:
                {
                    if (characterInConversationWith == CharacterInConversationWith.wizard)
                    {
                        // Raise OnDialogueEnd event for Village Wizard
                        OnDialogueEndWizard();
                    }
                }
                break;

            case CharacterInConversationWith.femaleWarrior:
                {
                    if (characterInConversationWith == CharacterInConversationWith.femaleWarrior)
                    {
                        // Raise OnDialogueEnd event for Female Warrior
                        OnDialogueEndFemaleWarrior();
                    }
                }
                break;
        }

        // After all is done, reset it back to none for simplicitie's sake
        characterInConversationWith = CharacterInConversationWith.none;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // List of sentences
    private Queue<string> sentences;

    public GameManager gameManager;

    public bool hasEnded;

    // Reference to animator
    public Animator animator;

    // References to dialogue text
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
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
        Debug.Log("End of convorsation");

        // Set animation to true
        animator.SetBool("IsOpen", false);

        // Set bool in Game Manager to false
        gameManager.isInDialogue = false;

        return true;
    }
}

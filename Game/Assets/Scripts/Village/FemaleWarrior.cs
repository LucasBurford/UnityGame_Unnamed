using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class FemaleWarrior : MonoBehaviour
{
    #region Members
    #region References
    [Header ("References")]

    // Reference to Dialogue Trigger and Manager
    public DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;

    // Reference to Animator
    public Animator animator;

    // Reference to AI pathing
    public AIPath ai;

    // This character mugshot
    public Sprite mugshot;

    #endregion

    #region Gameplay
    [Header("Gameplay")]

    // Enum for state character is in
    [SerializeField]
    private CharacterState state;
    [SerializeField]
    private enum CharacterState
    {
        idle,
        following,
        inConversation,
    }

    // Damage character will deal to enemies
    [SerializeField]
    private float damage;

    // Character run speed
    [SerializeField]
    private float moveSpeed;

    // Attack point and range
    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRange;

    // Target layers
    [SerializeField]
    private LayerMask targetLayer;

    // Prevent attack spam
    [SerializeField]
    private bool hasAttacked;

    public bool isAttacking;
    #endregion

    #region Dialogue
    [Header("Dialogue")]

    // Dialogue triggers
    [SerializeField]
    private bool dialogue1HasTriggered;

    [SerializeField]
    // Trigger dialogue "Check out the pond"
    private DialogueTrigger darkForestWaterDialogue;
    private bool darkForestWaterDialogueDone;

    [SerializeField]
    // Trigger dialogue "This river leads to the heart of the Dark Forest"
    private DialogueTrigger pondDialogue;
    private bool pondDialogueDone;

    [SerializeField]
    // Trigger dialogue "This is the entry to the Dark Forest
    private DialogueTrigger darkForestEntryDialogue;
    private bool darkForestEntryDialogueDone;

    #endregion

    #region Misc
    [Header("Misc")]

    [SerializeField]
    private bool hasPlayed;
    #endregion
    #endregion

    private void Start()
    {
        // Set destination to follow player
        gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<PlayerMovement>().gameObject.transform;

        // Subscribe OnDialogueEnded
        DialogueManager.OnDialogueEndFemaleWarrior += OnDialogueEnd;
        DialogueManager.OnDialogueEndPlayer += OnPlayerDialogueEnd;

        // Initialise values
        damage = 50;
        moveSpeed = 5;
        state = CharacterState.idle;
        attackRange = 2;

        // Set AI move speed to moveSpeed
        ai.maxSpeed = moveSpeed;
    }

    private void Update()
    {
        // Check what state character is in and handle accordingly
        CheckState();

        // Check move speed and face correct direction
        CheckDirection();
    }

    private void CheckState()
    {
        switch (state)
        {
            case CharacterState.idle:
                {
                    // Handle idle behaviour
                    Idle();
                }
                break;

            case CharacterState.following:
                {
                    // Handle following behaviour
                    FollowPlayer();
                }
                break;

            case CharacterState.inConversation:
                {
                    // Handle conversation behaviour
                    InConversation();
                }
                break;
        }
    }

    private void Idle()
    {
        // Stop following player
        ai.canMove = false;

        // Play idle animation
        animator.SetFloat("MoveSpeed", 0);
    }

    private void FollowPlayer()
    {
        // Follow player - allow to move
        ai.canMove = true;

        // Play running animation
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    private void Attack()
    {
        // Start animation
        animator.SetBool("IsAttacking", true);

        // Set hasAttacked to true
        hasAttacked = true;

        // Wait to actually cast attack to match animation
        StartCoroutine(CastAttack(1));
    }

    private void InConversation()
    {
        // Stop character from moving
        ai.canMove = false;

        // Set animation to idle
        animator.SetFloat("MoveSpeed", 0);
    }

    private void CheckDirection()
    {

    }

    private void PlaySound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    private void OnDialogueEnd()
    {
        state = CharacterState.following;
    }

    private void OnPlayerDialogueEnd()
    {
        if (!pondDialogueDone)
        {
            // Start dialogue, then prevent it from happening again
            ChangeDialogueSettings();
            pondDialogue.TriggerDialogue();
            pondDialogueDone = true;
        }
    }

    private void ChangeDialogueSettings()
    {
        // Set mugshot to this sprite
        dialogueManager.characterMugshot.sprite = mugshot;

        // Set enum in DialogueManager
        DialogueManager.characterInConversationWith = DialogueManager.CharacterInConversationWith.femaleWarrior;
    }

    private void ResetAttack()
    {
        // Reset hasAttacked
        hasAttacked = false;

        //hasPlayed = false;
    }

    IEnumerator CastAttack(float seconds)
    {
        // Wait to match animation
        yield return new WaitForSeconds(seconds);

        // Cast circle and collect hit objects
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayer);

        // Iterate through array and cause damage
        foreach (Collider2D col in hitObjects)
        {
            print("We hit: " + col.gameObject.name);
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        // Reset animation
        animator.SetBool("IsAttacking", false);

        // Wait to reset attack
        StartCoroutine(WaitToResetAttack(2));
    }

    IEnumerator WaitToResetAttack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        ResetAttack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "DarkForestWaterDialogue" && !darkForestWaterDialogueDone)
        {
            ChangeDialogueSettings();
            darkForestWaterDialogue.TriggerDialogue();
            darkForestWaterDialogueDone = true;
        }

        if (collision.gameObject.name == "DarkForestEntry" && !darkForestEntryDialogueDone)
        {
            // Start dialogue and prevent from replaying
            ChangeDialogueSettings();
            darkForestEntryDialogue.TriggerDialogue();
            darkForestEntryDialogueDone = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight" && !dialogue1HasTriggered)
        {
            // Prevent dialogue from happening again
            dialogue1HasTriggered = true;

            ChangeDialogueSettings();

            // Trigger the dialogue
            dialogueTrigger.TriggerDialogue();

            // Play speaking audio
            PlaySound("WizardSpeaking");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If collision is with an enemy and has not yet attacked, or is between attacks
        if (collision.gameObject.tag == "Enemy")
        {
            if (!hasAttacked)
            {
                print("Attacking!");
                Attack();

                // Set hasAttacked to true to prevent attack spamming
                hasAttacked = true;
            }
            else
            {
                print("Can't attack yet!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        inCombat
    }

    // Damage character will deal to enemies
    [SerializeField]
    private float damage;

    // Character run speed
    [SerializeField]
    private float moveSpeed;

    // Dialogue triggers
    [SerializeField]
    private bool dialogue1HasTriggered;
    #endregion
    #endregion

    private void Start()
    {
        // Set destination to follow player
        gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<PlayerMovement>().gameObject.transform;

        // Initialise values
        damage = 50;
        moveSpeed = 5;
        state = CharacterState.idle;

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

            case CharacterState.inCombat:
                {
                    // Handle combat behaviour
                    InCombat();
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

    private void InCombat()
    {
        // TODO: Attack enemies, play animations, sounds etc
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight" && !dialogue1HasTriggered)
        {
            // Prevent dialogue from happening again
            dialogue1HasTriggered = true;

            // Trigger the dialogue
            dialogueTrigger.TriggerDialogue();

            // Play speaking audio
            PlaySound("WizardSpeaking");
        }
    }
}

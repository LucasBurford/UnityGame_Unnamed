using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Goblin : MonoBehaviour
{
    #region Members
    [Header("References")]
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public Animator animator;
    public AIPath ai;

    [Header("Gameplay")]
    public States state;
    public enum States
    {
        idle,
        chasingPlayer,
        attacking
    }

    public float health;
    public float damageInflict;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        state = States.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, playerMovement.transform.position) <= 5)
        {
            state = States.chasingPlayer;
            ai.canMove = true;
        }
        else
        {
            state = States.idle;
            ai.canMove = false;
        }

        CheckState();

        if (health <= 0)
        {
            Die();
        }

        if (ai.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(6, 6, 6);
        }
        else if (ai.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(-6, 6, 6);
        }
    }

    public void CheckState()
    {
        switch (state)
        {
            case States.idle:
                {
                    animator.Play("GoblinIdle");

                    if (Random.Range(0, 1000) == 100)
                    {
                        FindObjectOfType<AudioManager>().Play("GoblinIdle");
                    }
                }
                break;

            case States.chasingPlayer:
                {
                    animator.Play("GoblinRun");
                }
                break;

            case States.attacking:
                {
                    if (Random.Range(1, 2) == 1)
                    {
                        animator.Play("GoblinAttack1");
                    }
                    else if (Random.Range(1, 2) == 2)
                    {
                        animator.Play("GoblinAttack2");
                    }
                }
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        FindObjectOfType<AudioManager>().Play("GoblinHurt");
    }

    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("GoblinDeath");

        gameManager.GiveXP(50);

        Destroy(gameObject);
    }
}

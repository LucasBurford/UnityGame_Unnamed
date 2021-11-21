using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Goblin : MonoBehaviour
{
    #region Members
    [Header("References and spec stuff")]
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public Animator animator;
    public AIPath ai;
    public Transform attackPoint;
    public float attackRange;

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
    public float attackDistance;

    public bool canAttack;

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

                    if (Vector2.Distance(gameObject.transform.position, playerMovement.transform.position) <= attackDistance)
                    {
                        state = States.attacking;

                        if (canAttack)
                        {
                            Attack();
                        }
                        else
                        {
                            print("Goblin waits to attack");
                        }
                    }
                }
                break;

            case States.attacking:
                {
                    animator.Play("GoblinAttack1");
                }
                break;
        }
    }

    public void Attack()
    {
        // Play attack sound 
        FindObjectOfType<AudioManager>().Play("GoblinAttack");

        // Cast circle and gather hitboxes
        Collider2D[] hitBoxes = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange);

        // Iterate through array 
        foreach(Collider2D col in hitBoxes)
        {
            if (col.gameObject.name == "Player_Knight")
            {
                gameManager.TakeDamage(damageInflict);
            }
        }

        canAttack = false;

        StartCoroutine(WaitToResetAttack());
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

    IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(5);

        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

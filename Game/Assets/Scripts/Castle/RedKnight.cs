using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RedKnight : MonoBehaviour
{
    #region Members
    [Header("References")]
    public AIPath ai;
    public AIDestinationSetter aiSetter;
    public Animator animator;
    public PlayerMovement player;

    [Header("Gameplay and spec")]
    public float health;
    public float damageInflict;
    public float distanceToMove;
    public float distanceToPlayer;

    public Transform detectEnemies;
    public float detectEnemiesRange;

    public Transform attackPoint;
    public float attackRange;

    public Collider2D[] detectedObjects;
    public bool hasTarget;

    public bool canAttack;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        health = 300;
        damageInflict = 40;
        detectEnemiesRange = 4;
        attackRange = 2.5f;
        ai.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        CheckRotation();

        if (!hasTarget)
        {
            DetectEnemies();
        }

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        print(aiSetter.target);
    }

    public void DetectEnemies()
    {
        // Gather hitboxes
        detectedObjects = Physics2D.OverlapCircleAll(detectEnemies.position, detectEnemiesRange);

        // Loop through array and see if any enemies are detected
        foreach (Collider2D col in detectedObjects)
        {
            // If detect enemies circle gathers enemy tags
            if (col.gameObject.tag == "Enemy")
            {
                // Set ai's destination to that pos
                aiSetter.target = col.gameObject.transform;

                // When ai reaches target, cast attack
                if (ai.reachedDestination)
                {
                    if (canAttack)
                    {
                        Attack();
                    }
                }
            }
        }
    }

    public void Attack()
    {
        hasTarget = true;

        // Gather hitboxes of enemy
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        // Loop through array 
        foreach(Collider2D col in hitObjects)
        {
            if (col.gameObject.name == "Goblin")
            {
                col.gameObject.GetComponent<Goblin>().TakeDamage(damageInflict);
            }
        }

        PlayAnimation("KnightAttack");
    }

    public void CheckState()
    {
        if (ai.canMove)
        {
            PlayAnimation("KnightWalk");
        }
        else
        {
            PlayAnimation("KnightIdle");
        }
    }

    public void CheckRotation()
    {
        if (ai.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else if (ai.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
        }
    }

    public void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectEnemies == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(detectEnemies.position, detectEnemiesRange);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

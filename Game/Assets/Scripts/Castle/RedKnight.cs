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
    public LayerMask enemyLayers;

    [Header("Gameplay and spec")]
    public float health;
    public float damageInflict;
    public float distanceToMove;
    public float distanceToPlayer;

    public bool moving;
    public bool attacking;

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
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        CheckRotation();

        DetectEnemies();

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
    }

    public void DetectEnemies()
    {
        // Gather hitboxes
        detectedObjects = Physics2D.OverlapCircleAll(detectEnemies.position, detectEnemiesRange, enemyLayers);

        // Loop through array and see if any enemies are detected
        foreach (Collider2D col in detectedObjects)
        {
            // If detect enemies circle gathers enemy tags
            if (col.gameObject.tag == "Enemy")
            {
                ai.canMove = true;

                // Set ai's destination to that pos
                aiSetter.target = col.gameObject.transform;

                // When ai reaches target, cast attack
                if (ai.reachedDestination)
                {
                    if (canAttack)
                    {
                        Attack();
                    }
                    else
                    {
                        print("Red Knight waits to attack");
                    }
                }
            }
        }
    }

    public void Attack()
    {
        attacking = true;

        ai.canMove = false;

        // Gather hitboxes of enemy
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Loop through array 
        foreach(Collider2D col in hitObjects)
        {
            if (col.gameObject.name == "Goblin")
            {
                col.gameObject.GetComponent<Goblin>().TakeDamage(damageInflict);
            }
        }

        PlayAnimation("KnightAttack");
        canAttack = false;

        StartCoroutine(WaitToResetAttack());
    }

    public void CheckState()
    {
        if (ai.canMove)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            PlayAnimation("KnightWalk");
        }
        else if (!moving && !attacking)
        {
            PlayAnimation("KnightIdle");
        }
        else if (!moving && attacking)
        {
            PlayAnimation("KnightAttack");
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

    IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(2);

        canAttack = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OctopusBoss : MonoBehaviour
{
    #region Members
    [Header("References and spec stuff")]
    public AIPath ai;
    public PlayerMovement playerMovement;
    public GameManager gameManager;

    public Transform attackPoint;
    public float attackRange;

    [Header("Gameplay")]
    public bool activated;
    public bool chasing;
    public bool canAttack;

    [Header("Health and damage")]
    public float health;
    public float damageInflict;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        health = 500;
        damageInflict = 40;
        attackRange = 1.4f;

        ai.canMove = true;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If boss fight has been activated, and all dialogue etc has played, persue the player
        if (activated && chasing)
        {
            // Allow the ai to move and follow player
            ai.canMove = true;
        }

        CheckPositions();

        CheckRotation();
    }

    public void Attack()
    {
        print("Attack");

        // Set canAttack to false to prevent instant death
        canAttack = false;

        // Cast a circle to gather hitboxes
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        // Iterate through list 
        foreach (Collider2D col in hitObjects)
        {
            // If collides with player
            if (col.gameObject.tag == "Player")
            {
                // Take damage
                gameManager.TakeDamage(damageInflict);
            }
        }

        // Wait to attack again to prevent instant death
        StartCoroutine(WaitToResetAttack());
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    public void Heal()
    {
        health += 10;
    }

    public void SlowDown()
    {
        ai.maxSpeed = 1;

        StartCoroutine(WaitToResetMoveSpeed());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "IceAttack(Clone)")
        {
            SlowDown();
        }
    }

    private void CheckPositions()
    {
        // Check that boss is close enough to attack player
        if (Vector2.Distance(gameObject.transform.position, playerMovement.transform.position) <= 5)
        {
            // If enemy can attack
            if (canAttack)
            {
                Attack();
            }
            else
            {
                print("Boss is waiting");
            }
        }
    }

    private void CheckRotation()
    {
        if (ai.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-16f, 16f, 16f);
        }
        else if (ai.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(16f, 16f, 16f);
        }
    }

    public void SpawnBoss()
    {
        // Activate the actual GameObject AND the boss fight
        gameObject.SetActive(true);
        activated = true;

        // Play starting scream
        FindObjectOfType<AudioManager>().Play("OctopusBossScream");
        StartCoroutine(WaitToStartEllieLine());
    }

    IEnumerator WaitToResetMoveSpeed()
    {
        yield return new WaitForSeconds(3);

        ai.maxSpeed = 4;
    }

    IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(2);

        canAttack = true;
    }

    IEnumerator WaitToStartEllieLine()
    {
        yield return new WaitForSeconds(4);

        // Play voice line
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

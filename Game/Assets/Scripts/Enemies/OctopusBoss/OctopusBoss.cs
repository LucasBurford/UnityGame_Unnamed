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
    public Material material;
    public bool isDisolving;
    public float fade;

    public Transform attackPoint;
    public float attackRange;

    [Header("Gameplay")]
    public bool activated;
    public bool chasing;
    public bool canAttack;
    public bool canPoison;

    [Header("Health and damage")]
    public float health;
    public float damageInflict;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        health = 500;
        damageInflict = 1;
        attackRange = 1.4f;
        material.SetFloat("_Fade", 1);

        ai.canMove = true;
        canAttack = true;
        canPoison = true;
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

        if (isDisolving)
        {
            fade -= 0.01f;

            material.SetFloat("_Fade", fade);
        }
    }

    public void Attack()
    {
        print("Attack");

        // Set canAttack to false to prevent attack spam
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

                // Only poison player once every 5 seconds to prevent spam
                if (canPoison)
                {
                    gameManager.isPoisoned = true;
                    canPoison = false;

                    StartCoroutine(WaitToResetPoison());
                }
            }
        }

        // Wait to attack again to prevent attack spam
        StartCoroutine(WaitToResetAttack());
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
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

    public void Die()
    {
        //isDisolving = true;

        FindObjectOfType<AudioManager>().Play("OctopusBossScream");

        FindObjectOfType<EnemyTracker>().octopusBossIsDead = true;

        gameManager.GiveXP(200);

        Destroy(gameObject);

        // Wait to destroy object
       // StartCoroutine(WaitToDestroyObject());
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

    IEnumerator WaitToResetPoison()
    {
        yield return new WaitForSeconds(5);

        canPoison = true;
    }

    IEnumerator WaitToStartEllieLine()
    {
        yield return new WaitForSeconds(4);

        // Play voice line
    }

    IEnumerator WaitToDestroyObject()
    {
        yield return new WaitForSeconds(6);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Modified Skeleton class: adds pathfinding, higher health and damage
/// </summary>
public class DarkSkeleton : MonoBehaviour
{
    #region References
    [Header("References")]

    public GameManager gameManager;
    public AIPath ai;
    public AIDestinationSetter aiD;
    public Animator animator;
    public Transform attackPoint;
    #endregion

    #region Gameplay
    [Header("Gameplay Values")]

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float maxDamage;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private bool canAttack;

    [SerializeField]
    private bool isWalking;

    [SerializeField]
    private bool hasPlayed;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        // Get components
        GetComponents();

        // Initialise values
        InitialiseValues();
    }

    private void GetComponents()
    {
        // Get GameManager
        gameManager = FindObjectOfType<GameManager>();

        // Get AIPath & Destination Setter
        ai = gameObject.GetComponent<AIPath>();
        aiD = gameObject.GetComponent<AIDestinationSetter>();
    }

    private void InitialiseValues()
    {
        // Get pathfinding target
        aiD.target = FindObjectOfType<PlayerMovement>().transform;
        ai.maxSpeed = 2.5f;

        // Initialise values
        canAttack = true;
        maxHealth = 200;
        currentHealth = 200;
        damage = maxDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        // Check if Skeleton is walking
        CheckSpeed();
        // Change animation if necesary
        ChangeMoveAnimation();

        if (Random.Range(0, 1000) < 5)
        {
            PlaySkeletonSound("SkeletonIdle");
        }
    }

    private void AttackPlayer()
    {
        // Set AI move speed to 0 to prevent pushing
        ai.maxSpeed = 0;

        // Get collided objects by casting a circle
        Collider2D[] objects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D col in objects)
        {
            // Handle collision with player
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("Hit Player");

                // Play attack animation
                animator.SetBool("IsAttacking", true);

                //-- Take damage moved to ResetAttackAnimation to match animation timings --\\

                // Set canAttack to false
                canAttack = false;

                // Reset animation back to idle
                StartCoroutine(ResetAttackAnimation(0.7f));

                // Set canAttack back to true
                StartCoroutine(ResetAttackBool(2));
            }

            // Handle shield block audio
            if (col.gameObject.name == "PlayerShield")
            {
                StartCoroutine(WaitToPlayShieldSound(0.7f));
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // Deduct passed in damage from current health
        currentHealth -= damage;
    }

    private void CheckSpeed()
    {
        if (ai.velocity.x >= 0.01f || ai.velocity.y >= 0.01f)
        {
            isWalking = true;
        }
        else if (ai.velocity.x <= -0.01f || ai.velocity.y <= -0.01f)
        {
            isWalking = true;
        }
        else if (ai.velocity.x == 0 || ai.velocity.y == 0)
        {
            isWalking = false;
        }
    }

    private void ChangeMoveAnimation()
    {
        // If Skeleton is moving towards player
        if (isWalking)
        {
            // Set walking animation to true
            animator.SetFloat("MoveSpeed", 2.5f);
        }
        // If skeleton isn't moving at all
        else if (!isWalking)
        {
            // Set walking animation to false
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    private void Die()
    {
        Debug.Log("Skeleton died");

        // Set canAttack to false so he doesn't attack while dissolving
        canAttack = false;

        if (!hasPlayed)
        {
            PlaySkeletonSound("SkeletonDeath");
        }

        hasPlayed = true;

        // Wait for time to destroy object
        StartCoroutine(WaitToDestroy(3));
    }

    private void PlaySkeletonSound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            if (canAttack)
            {
                AttackPlayer();
            }
            else
            {
                Debug.Log("Can't attack yet!");
            }
        }
    }

    public void ChangeAttackDamage(float amount)
    {
        damage = amount;

        StartCoroutine(ResetAttackDamage(2));
    }

    IEnumerator ResetAttackDamage(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        damage = maxDamage;
    }

    IEnumerator ResetAttackAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Damge the player
        gameManager.TakeDamage(damage);

        // Reset animation back to idle
        animator.SetBool("IsAttacking", false);
    }

    IEnumerator ResetAttackBool(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        canAttack = true;

        // Reset AI move speed
        ai.maxSpeed = 2.5f;
    }

    IEnumerator WaitToDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

    IEnumerator WaitToPlayShieldSound (float seconds)
    {
        yield return new WaitForSeconds(seconds);

        FindObjectOfType<AudioManager>().Play("ShieldHit");
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

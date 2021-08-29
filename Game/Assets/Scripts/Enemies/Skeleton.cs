using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    #region References
    public GameManager gameManager;
    public Animator animator;

    public Transform attackPoint;

    public Material material;
    #endregion

    #region Gameplay
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float maxDamage;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float fade;

    [SerializeField]
    private float fadeFactor;

    [SerializeField]
    private bool canAttack;

    [SerializeField]
    private bool isDissolving;

    public bool isBeingBlocked;

    private bool hasPlayed;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        maxHealth = 200;
        currentHealth = maxHealth;

        fade = 1;

        damage = maxDamage;

        canAttack = true;

        material.SetFloat("_Fade", fade);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (isDissolving)
        {
            fade -= fadeFactor;

            if (fade <= 0)
            {
                fade = 0;
                isDissolving = false;
            }

            material.SetFloat("_Fade", fade);
        }

        if (Random.Range(0, 1000) < 10)
        {
            PlaySkeletonSound("SkeletonIdle");
        }
    }

    private void AttackPlayer()
    {
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

        // Play dissolve animation
        isDissolving = true;

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

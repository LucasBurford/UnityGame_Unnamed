using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEye : MonoBehaviour, IEnemy
{
    #region Members
    public GameManager gameManager;

    public Animator animator;

    public Material material;

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float fade;

    [SerializeField]
    private float fadeFactor;

    [SerializeField]
    private bool isDissolving;

    [SerializeField]
    private bool hasAttacked;

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private bool hasDied;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Get GameManager
        gameManager = FindObjectOfType<GameManager>();

        // Get animator
        animator = gameObject.GetComponent<Animator>();

        // Set destination
        gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<PlayerMovement>().gameObject.transform;

        maxHealth = 70;
        damage = 5;

        currentHealth = maxHealth;

        material.SetFloat("_Fade", fade);
    }

    // Update is called once per frame
    void Update()
    {
        // Stop enemies from too far away from searching
        if (Vector3.Distance(transform.position, FindObjectOfType<PlayerMovement>().transform.position) > 40)
        {
            gameObject.GetComponent<AIPath>().canSearch = false;
        }
        else
        {
            gameObject.GetComponent<AIPath>().canSearch = true;
        }

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

        if (Random.Range(0, 1000) < 5)
        {
            FindObjectOfType<AudioManager>().Play("FlyingEyeIdle");
        }
    }

    public void AttackPlayer()
    {
        // Set hasAttacked to false
        hasAttacked = true;

        // Set animation to attacking
        animator.SetBool("IsAttacking", true);

        // Damage player
        gameManager.TakeDamage(damage);

        // Wait to reset attack animation
        StartCoroutine(WaitToResetAttackAnimation(0.4f));

        // Wait to reset attack time
        StartCoroutine(WaitToResetAttack(2));
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        animator.SetFloat("DamageTaken", amount);

        StartCoroutine(WaitToResetDamageAnimation(0.3f));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the enemy hasn't attacked, or is not waiting between attacks
        if (!hasAttacked && !hasDied)
        {
            if (collision.gameObject.name == "Player_Knight")
            {
                AttackPlayer();
            }
        }
        else
        {
            Debug.Log("Flying Eye: Can't attack yet!");
        }
    }

    public void Die()
    {
        hasDied = true;

        isDissolving = true;

        FindObjectOfType<AudioManager>().Play("FlyingEyeDeath");

        StartCoroutine(WaitToDissolve(2));
    }

    IEnumerator WaitToResetAttackAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Set animation back to flying
        animator.SetBool("IsAttacking", false);
    }

    IEnumerator WaitToResetAttack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Set hasAttacked back to true
        hasAttacked = false;
    }

    IEnumerator WaitToResetDamageAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        animator.SetFloat("DamageTaken", 0);
    }

    IEnumerator WaitToDissolve(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }
}

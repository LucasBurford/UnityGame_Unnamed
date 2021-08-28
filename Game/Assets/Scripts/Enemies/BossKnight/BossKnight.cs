using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public class BossKnight : MonoBehaviour
{
    #region Members
    // Reference to GameManager
    public GameManager gameManager;

    // BossKnightDeath
    public BossKnightDeath bossKnightDeath;

    // Reference to animator
    public Animator animator;

    // Bool to determine if boss should persue player
    public bool isActive;

    // Bool to determine if boss is staggered
    public bool staggered;

    // Boss's health
    public float health;

    // Boss's stagger
    public float stagger;

    // Decrease stagger by DeltaTime * this
    public float staggerDecrease;

    // Reference to health bar
    public BossHealthBar bossHealthBar; 

    // Reference to stagger bar
    public BossStaggerBar bossStaggerBar;

    public Collider2D mCollider;
    #endregion

    #region Fight Related Members
    // Float to determine how much the knight damages the player
    public float damage;

    // Bool to determine if knight is touching player - close enough to attack
    public bool touchingPlayer;

    // Bool to determine if knight can attack
    public bool canAttack;

    // Origin of attacks
    public Transform attackPoint;

    // Attack range
    public float attackRange;

    // Layer mask
    public LayerMask layer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AIPath>().canSearch = false; ;
        canAttack = true;
        health = 100;
        damage = 15;
        stagger = 0;
        staggerDecrease = 10;
        attackRange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // Boss fight should only commence if boss fight has been activated
        if (isActive)
        {
            gameObject.GetComponent<AIPath>().canSearch = true;

            // If stagger is greater than or equal to 100...
            if (stagger >= 100)
            {
                // Set staggered to true
                staggered = true;
            }

            // If the boss is staggered
            if (staggered && stagger >= 0)
            {
                // Decrease stagger meter by small amount
                stagger -= Time.deltaTime * staggerDecrease;

                // Stop boss from moving
                ChangeMoveBehaviour(false);
            }
            // If stagger drops to 0 or below
            if (stagger <= 0)
            {
                // Set stagger to 0 and staggered to false
                stagger = 0;
                staggered = false;
            }

            // If boss is NOT staggered...
            if (!staggered)
            {
                // Allow boss to move 
                ChangeMoveBehaviour(true);

                // If knight is close to player - Aka touching
                if (touchingPlayer)
                {
                    // Attack the player
                    Attack();
                }
            }
            // If boss IS staggered...
            if (staggered)
            {
                // Freeze him in place
                ChangeMoveBehaviour(false);
            }

            // Constantly update the health and stagger bar
            UpdateMeters();

            if (health <= 0)
            {
                Die();
            }
        }
        else
        {
            return;
        }
    }

    public void ChangeMoveBehaviour(bool pBool)
    {
        gameObject.GetComponent<AIPath>().canMove = pBool;
    }

    public void Attack()
    {
        if (canAttack)
        {
            // Attack player - cast circle and store collided objects
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layer);

            // Loop through collided objects
            foreach (Collider2D hObject in hitObjects)
            {
                // Deduct health from player
                if (hObject.gameObject.tag == "Player")
                {
                    gameManager.TakeDamage(damage);
                }
            }

            // Set can attack to false to prevent infinite damage
            canAttack = false;

            // Wait for x seconds before attacking again
            StartCoroutine(Wait(2));
        }
    }

    public void TakeDamage(float damage)
    {
        if (staggered)
        {
            health -= damage;
        }
        else
        {
            Debug.Log("You must stagger the boss to damage him!");
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "FireBall(Clone)")
        {
            // Only add to stagger meter if the boss isn't already staggered
            if (!staggered)
            {
                stagger += 100;
            }
            else
            {
                Debug.Log("Boss is already staggered, move in for an attack!");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            // Set bool to true
            touchingPlayer = true;
        }
    }

    private void UpdateMeters()
    {
        bossHealthBar.SetSlider(health);
        bossStaggerBar.SetSlider(stagger);
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }

    private void Die()
    {
        bossKnightDeath.OnBossKnightDeath();
        mCollider.isTrigger = true;
        Destroy(gameObject);
    }
}

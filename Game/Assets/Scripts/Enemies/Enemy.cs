using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;

    public float moveSpeed;
    public float health;
    public float damage;

    [SerializeField]
    private bool isDead;

    // Dissolve factor
    [SerializeField]
    private float fade;
    
    [SerializeField]
    private float fadeFactor;

    // Reference to Dissolve material
    [SerializeField]
    private Material material;

    [SerializeField]
    private bool isDissolving;

    int rand;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise values

        gameManager = FindObjectOfType<GameManager>();
        gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<PlayerMovement>().gameObject.transform;

        moveSpeed = 5;
        health = 100;
        damage = 10;

        fade = 1;
        material.SetFloat("_Fade", fade);

        // Set AI move speed
        gameObject.GetComponent<AIPath>().maxSpeed = moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Die();
        }

        // Randomly play idle sound
        rand = Random.Range(0, 1000);

        if (rand == 50)
        {
            FindObjectOfType<AudioManager>().Play("TrollIdle");
        }

        if (isDissolving)
        {
            Debug.Log("Is dissolving");
            Dissolve();
        }
        else
        {
            Debug.Log("Not dissolving");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            DamagePlayer();
        }
    }

    private void DamagePlayer()
    {
        gameManager.TakeDamage(damage);
        Wait(2);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        FindObjectOfType<AudioManager>().Play("TrollHurt1");
    }

    public void SlowDown(float amount)
    {
        // Slow down enemy movement by amount
        gameObject.GetComponent<AIPath>().maxSpeed -= amount;
        // Wait for time
        StartCoroutine(SpeedBackUp());
    }

    public void KnockBack(float amount)
    {
        transform.position = new Vector3(transform.position.x - amount, transform.position.y);
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;

            isDissolving = true;

            gameManager.GiveXP(25);

            FindObjectOfType<AudioManager>().Play("TrollDeath");

            StartCoroutine(Wait(2));
        }
    }

    public void StartDissolve()
    {
        Debug.Log("Start dissolve");

        isDissolving = true;
    }

    private void Dissolve()
    {
        fade -= 0.01f;

        if (fade <= 0)
        {
            fade = 0;
            isDissolving = false;
        }

        material.SetFloat("_Fade", fade);
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

    IEnumerator SpeedBackUp()
    {
        // Wait for 3 seconds then set speed back to normal
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<AIPath>().maxSpeed = moveSpeed;
    }
}

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

        // Set AI move speed
        gameObject.GetComponent<AIPath>().maxSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
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
        Wait();
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
        gameManager.GiveXP(25);

        FindObjectOfType<AudioManager>().Play("TrollDeath");

        Destroy(gameObject);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

    IEnumerator SpeedBackUp()
    {
        // Wait for 3 seconds then set speed back to normal
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<AIPath>().maxSpeed = moveSpeed;
    }
}

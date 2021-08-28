using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttack : MonoBehaviour
{
    // Impact effect
    public GameObject hitEffect;

    // Damage caused
    public float damage;

    // Slow down the enemies movement speed
    public float slowEffect;

    private void Start()
    {
        damage = 5;
        slowEffect = 3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.SlowDown(slowEffect);
        }

        FindObjectOfType<AudioManager>().Play("IceImpact");

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.2f);
        Destroy(gameObject);
    }
}

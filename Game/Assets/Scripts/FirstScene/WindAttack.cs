using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttack : MonoBehaviour
{
    // Impact effect
    public GameObject hitEffect;

    // Damage
    public float damage;

    // How far the enemy is knocked back
    public float knockBack;

    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        knockBack = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.KnockBack(knockBack);
        }

        FindObjectOfType<AudioManager>().Play("WindImpact");

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.2f);
        Destroy(gameObject);
    }
}

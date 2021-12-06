using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RedKnight : MonoBehaviour
{
    #region Members
    [Header("References")]
    public AIPath ai;
    public AIDestinationSetter aiSetter;
    public Animator animator;
    public PlayerMovement player;

    [Header("Gameplay and spec")]
    public float health;
    public float damageInflict;
    public float distanceToMove;
    public float distanceToPlayer;

    public Transform detectEnemies;
    public float detectEnemiesRange;

    public Collider2D[] hitObjects;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        health = 300;
        damageInflict = 40;
        detectEnemiesRange = 4;
        ai.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        CheckRotation();
        DetectEnemies();

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        print(aiSetter.target);
    }

    public void DetectEnemies()
    {
        // Gather hitboxes
        hitObjects = Physics2D.OverlapCircleAll(detectEnemies.position, detectEnemiesRange);

        // Loop through array and see if any enemies are detected
        foreach (Collider2D col in hitObjects)
        {
            // If detect enemies circle gathers enemy tags
            if (col.gameObject.tag == "Enemy")
            {
                // Get position of detected enemy
                Vector2 enemyPos = col.gameObject.transform.position;

                // Set ai's destination to that pos
                aiSetter.target = col.gameObject.transform;

                // Allow ai to move
                ai.canMove = true;
            }
        }
    }

    public void CheckState()
    {
        // If player is within following distance
        if (distanceToPlayer <= 6)
        {
            ai.canMove = true;
            PlayAnimation("KnightWalk");
        }
        else if (distanceToPlayer >= 7)
        {
            ai.canMove = false;
            PlayAnimation("KnightIdle");
        }

        if (ai.reachedDestination)
        {
            PlayAnimation("KnightIdle");
        }
    }

    public void CheckRotation()
    {
        if (ai.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else if (ai.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
        }
    }

    public void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectEnemies == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(detectEnemies.position, detectEnemiesRange);
    }
}

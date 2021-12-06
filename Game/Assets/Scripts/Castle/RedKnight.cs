using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RedKnight : MonoBehaviour
{
    #region Members
    [Header("References")]
    public AIPath ai;
    public Animator animator;
    public PlayerMovement player;

    [Header("Gameplay and spec")]
    public float health;
    public float damageInflict;
    public float distanceToMove;
    public float distanceToPlayer;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        health = 300;
        damageInflict = 40;

        ai.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        CheckRotation();

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        print(ai.reachedDestination);
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
}

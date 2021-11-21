using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    #region Members
    [Header("References")]
    public Animator animator;

    [Header("Gameplay")]
    public States state;
    public enum States
    {
        idle,
        running,
        attacking
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        state = States.idle;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    public void CheckState()
    {
        switch (state)
        {
            case States.idle:
                {
                    animator.Play("GoblinIdle");
                }
                break;

            case States.running:
                {
                    animator.Play("GoblinRun");
                }
                break;

            case States.attacking:
                {
                    if (Random.Range(1, 2) == 1)
                    {
                        animator.Play("GoblinAttack1");
                    }
                    else if (Random.Range(1, 2) == 2)
                    {
                        animator.Play("GoblinAttack2");
                    }
                }
                break;
        }
    }
}

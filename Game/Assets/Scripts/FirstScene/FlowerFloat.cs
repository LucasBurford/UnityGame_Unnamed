using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerFloat : MonoBehaviour
{
    // Moving speeds
    public float moveX;
    public float moveY;

    private void Start()
    {
        moveX = Random.Range(-0.01f, 0.01f);
        moveY = Random.Range(-0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the flowers along X and Y by X and Y speeds 
        transform.Translate(moveX, moveY, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PondColliderTop" || collision.gameObject.name == "PondColliderBottom")
        {
            moveY *= -1;
        }

        if (collision.gameObject.name == "PondColliderLeft" || collision.gameObject.name == "PondColliderRight")
        {
            moveX *= -1;
        }
    }
}

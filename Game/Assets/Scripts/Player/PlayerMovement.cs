﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    #region Members
    #region References
    [Header("References")]

    private Material material;

    public GameManager gameManager;

    public FireFlicker campfire;

    public Rigidbody2D rb;

    public Camera cam;

    public AudioSource audioSource;
    public AudioClip audioGrass;
    public AudioClip audioWater;
    public AudioClip audioWood;
    public AudioClip audioStone;
    #endregion

    #region Gameplay
    [Header("Gameplay")]
    [SerializeField]

    Vector2 movement;

    public float moveSpeed;
    public bool isRunning;

    // Bool to determine if player can move
    private bool canMove;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    #endregion

    #region Dialogue
    [Header("Dialogue")]
    #endregion

    #region Misc
    [Header("Misc")]

    public bool isDissolving;
    public bool isReverseDisolving;
    public float fade;

    public enum Surface
    {
        grass,
        water,
        wood,
        stone
    }

    public Surface surface;
    #endregion
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        material = GetComponent<SpriteRenderer>().material;
        fade = 1;

        canMove = true;

        surface = Surface.grass;
        moveSpeed = 5f;

        material.SetFloat("_Fade", fade);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canMove)
        {
            Movement();
        }
        else
        {
            movement = new Vector2(0, 0);
            return;
        }

        if (isDissolving)
        {
            Dissolve();
        }
        else if (isReverseDisolving)
        {
            ReverseDissolve();
        }

        SurfaceAudio();
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Handle Movement
    private void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0 || movement.x < 0 || movement.y > 0 || movement.y < 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    // Move the player to a specified position
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void SurfaceAudio()
    {
        switch (surface)
        {
            case Surface.grass:
                {
                    audioSource.clip = audioGrass;
                }
                break;

            case Surface.water:
                {
                    audioSource.clip = audioWater;
                }
                break;

            case Surface.wood:
                {
                    audioSource.clip = audioWood;
                }
                break;

            case Surface.stone:
                {
                    audioSource.clip = audioStone;
                }
                break;
        }

        if (isRunning)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void StartDissolve()
    {
        isDissolving = true;
    }

    public void SetReverseDissolve()
    {
        isReverseDisolving = true;
    }

    private void Dissolve()
    {
        fade -= 0.01f;

        if (fade <= 0)
        {
            fade = 0;
            isDissolving = false;
        }

        material.SetColor("Color_3A09B3A0", new Color(240, 5, 5));
        material.SetFloat("_Fade", fade);
    }

    private void ReverseDissolve()
    {
        fade += 0.01f;

        if (fade >= 1)
        {
            fade = 1;
            isReverseDisolving = false;
        }

        material.SetColor("Color_3A09B3A0", new Color(0, 240, 255));
        material.SetFloat("_Fade", fade);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "CampFire" && gameManager.currentHealth < 100)
        {
            gameManager.Heal(1f);

            // Drain energy from fire - DO NOT CHANGE THIS VALUE 
            campfire.energy -= 0.030f;
        }
    }

    // Stop player moving
    public IEnumerator StopPlayerMoving(float seconds)
    {
        canMove = false;

        yield return new WaitForSeconds(seconds);

        canMove = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    #region Members
    private static GameObject instance;

    public GameManager gameManager;

    public FireFlicker campfire;

    public float moveSpeed;

    public Rigidbody2D rb;
    public Camera cam;

    public AudioSource audioSource;
    public AudioClip audioGrass;
    public AudioClip audioWater;
    public AudioClip audioWood;
    public AudioClip audioStone;

    Vector2 movement;

    public bool isRunning;

    // Bool to determine if player can move
    private bool canMove;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public enum Surface
    {
        grass,
        water,
        wood,
        stone
    }

    public Surface surface;
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        canMove = true;

        surface = Surface.grass;
        moveSpeed = 5f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Debug.Log(canMove);

        if (canMove)
        {
            Movement();
        }
        else
        {
            movement = new Vector2(0, 0);
            return;
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

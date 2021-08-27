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

    Vector2 movement;

    public bool isRunning;

    public enum Surface
    {
        grass,
        water,
    }

    public Surface surface;

    #endregion
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        surface = Surface.grass;
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
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

        #region Surface audio
        if (surface == Surface.grass)
        {
            audioSource.clip = audioGrass;
        }

        if (surface == Surface.water)
        {
            audioSource.clip = audioWater;
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
        #endregion
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerHouse")
        {
            SceneManager.LoadScene("PlayerHouse");
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

        Debug.Log(name + ":" + collision.gameObject.name);
    }
}

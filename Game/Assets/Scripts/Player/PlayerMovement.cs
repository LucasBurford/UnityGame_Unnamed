using System.Collections;
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
    public DialogueManager dialogueManager;

    public FireFlicker campfire;

    public Rigidbody2D rb;

    public Camera cam;

    public GlobalLightMove sun;

    public AudioSource audioSource;
    public AudioClip audioGrass;
    public AudioClip audioWater;
    public AudioClip audioWood;
    public AudioClip audioStone;
    public AudioClip audioDarkForestWood;

    public Sprite characterMugshot;
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

    public bool canDodge;
    public float dodgeDistance;
    #endregion

    #region Dialogue
    [Header("Dialogue")]

    public DialogueTrigger pondDialogue;

    // Dark Forest Small Pond Dialogue
    public DialogueTrigger smallPondDialogue;
    private bool smallPondDialogueDone;

    // BoardWalk Dialogue
    public DialogueTrigger boardWalkDialogue;
    private bool boardWalkDialogueDone;

    // Return to Wizard dialogue
    public DialogueTrigger returnToWizardDialogue;
    private bool returnToWizardDialogueDone;

    // Go to explore castle dialogue
    public DialogueTrigger goToExploreCastleDialogue;
    private bool goToExploreCastleDialogueDone;

    // Talk to Wizard dialogue - nothing
    public DialogueTrigger idleWizardDialogue;

    // Castle entrance dialogue
    public DialogueTrigger castleEntranceDialogue;
    private bool castleEntranceDialogueDone;

    #endregion

    #region Misc
    [Header("Misc")]

    public bool isDissolving;
    public bool isReverseDisolving;
    public float fade;

    public bool camBacking;
    public bool camReversing;

    public enum Surface
    {
        grass,
        water,
        wood,
        stone,
        darkForestWood
    }

    public Surface surface;

    public GameObject dissappearingBushes;
    public GameObject castle;
    private bool windowTrigger;
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

        #region Cam backing and reversing
        if (camBacking)
        {
            cam.orthographicSize += 0.03f;
            
            if (cam.orthographicSize >= 14)
            {
                camBacking = false;
                StartCoroutine(WaitToReverseCamera());
            }
        }
        if (camReversing)
        {
            cam.orthographicSize -= 0.03f;

            if (cam.orthographicSize <= 7.5f)
            {
                cam.orthographicSize = 7.5f;
                camReversing = false;
            }
        }
        #endregion
    }

    #region Gameplay Methods
    // Handle Movement
    private void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        #region Dodge
        // If moving right and presses space
        if (movement.x >= 0.01f && Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            // Dodge right
            transform.position = new Vector3(transform.position.x + dodgeDistance, transform.position.y, transform.position.z);
            FindObjectOfType<AudioManager>().Play("PlayerDodge");
            canDodge = false;
            StartCoroutine(WaitToResetDodge());
        }
        // If moving left and presses space
        else if (movement.x <= -0.1f && Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            // Dodge left
            transform.position = new Vector3(transform.position.x - dodgeDistance, transform.position.y, transform.position.z);
            FindObjectOfType<AudioManager>().Play("PlayerDodge");
            canDodge = false;
            StartCoroutine(WaitToResetDodge());
        }

        // If moving up and presses space
        else if (movement.y >= 0.01f && Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            // Dodge up
            transform.position = new Vector3(transform.position.x, transform.position.y + dodgeDistance, transform.position.z);
            FindObjectOfType<AudioManager>().Play("PlayerDodge");
            canDodge = false;
            StartCoroutine(WaitToResetDodge());
        }
        // If moving down and presses space
        else if (movement.y <= 0.01f && Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            // Dodge down
            transform.position = new Vector3(transform.position.x, transform.position.y - dodgeDistance, transform.position.z);
            FindObjectOfType<AudioManager>().Play("PlayerDodge");
            canDodge = false;
            StartCoroutine(WaitToResetDodge());
        }
        #endregion

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

            case Surface.darkForestWood:
                {
                    audioSource.clip = audioDarkForestWood;
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

    private void ChangeDialogueSettings()
    {
        // Set sprite
        dialogueManager.characterMugshot.sprite = characterMugshot;

        // Set enum in DialogueManager
        DialogueManager.characterInConversationWith = DialogueManager.CharacterInConversationWith.player;

    }
    #endregion

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Dialogues
        if (collision.gameObject.name == "PondDialogue")
        {
            // Trigger dialogue
            ChangeDialogueSettings();
            pondDialogue.TriggerDialogue();
        }

        if (collision.gameObject.name == "DarkForestEntry")
        {
            // Set sun to follow player when they start travelling further away from start area
            sun.shouldFollowPlayer = true;

            // Maybe make it darker too
            //sun.ChangeIntensity(0.3f);
        }

        if (collision.gameObject.name == "SmallPondDialogue" && !smallPondDialogueDone)
        {
            smallPondDialogue.TriggerDialogue();
            smallPondDialogueDone = true;
            FindObjectOfType<FemaleWarrior>().ChangeDialogueSettings();
        }

        if (collision.gameObject.name == "BoardWalkDialogue" && !boardWalkDialogueDone)
        {
            boardWalkDialogue.TriggerDialogue();
            boardWalkDialogueDone = true;
            FindObjectOfType<FemaleWarrior>().ChangeDialogueSettings();
        }

        if (collision.gameObject.name == "ReturnToWizardDialogue" && FindObjectOfType<EnemyTracker>().octopusBossIsDead &&!returnToWizardDialogueDone)
        {
            returnToWizardDialogue.TriggerDialogue();
            returnToWizardDialogueDone = true;
        }
        else if (collision.gameObject.name == "ReturnToWizardDialogue")
        {
            idleWizardDialogue.TriggerDialogue();
        }

        if (collision.gameObject.name == "GoExploreCastleDialogue" && FindObjectOfType<EnemyTracker>().octopusBossIsDead && !goToExploreCastleDialogueDone)
        {
            FindObjectOfType<FemaleWarrior>().ChangeDialogueSettings();
            dissappearingBushes.SetActive(false);
            castle.SetActive(true);
            goToExploreCastleDialogue.TriggerDialogue();
            goToExploreCastleDialogueDone = true;
        }

        if (collision.gameObject.name == "CastleEntranceDialogue" && !castleEntranceDialogueDone)
        {
            castleEntranceDialogue.TriggerDialogue();
            FindObjectOfType<FemaleWarrior>().ChangeDialogueSettings();
            FindObjectOfType<CastleManager>().entranceSkeleton.SetActive(true);
            castleEntranceDialogueDone = true;
        }
        #endregion

        if (collision.gameObject.name == "WindowRoomEntrance" && !windowTrigger)
        {
            windowTrigger = true;

            // Pull camera back
            camBacking = true;

            // Play sound
            FindObjectOfType<AudioManager>().Play("WindowRoomEcho");
        }

        if (collision.gameObject.name == "LakeKillZone")
        {
            print("Drowned");
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
    #endregion

    #region Coroutines

    IEnumerator WaitToResetDodge()
    {
        yield return new WaitForSeconds(2);

        canDodge = true;
    }

    IEnumerator WaitToReverseCamera()
    {
        yield return new WaitForSeconds(4);

        camReversing = true;
    }

    // Stop player moving
    public IEnumerator StopPlayerMoving(float seconds)
    {
        canMove = false;

        yield return new WaitForSeconds(seconds);

        canMove = true;
    }
    #endregion
}

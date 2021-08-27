using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Health/Level Stuff
    [Header ("Health/Level Stuff")]

    // Player's current health, and the max it can be
    public float currentHealth;
    public float maxHealth;

    // Amount of XP player has currently got, and their level
    public float currentXP;
    public int playerLevel;

    // Health and level related UI
    public TMP_Text healthText;
    public TMP_Text currentLevelText;
    public TMP_Text collectedPlantsText;
    public HealthBar healthBar;
    #endregion

    #region Powerups/Attack stuff/Items
    [Header("Powerups")]

    public Powerups powerups;

    // Enum to rotate through powerups - player selects one
    public enum Powerups
    {
        none,
        fire,
        lightning,
        ice,
        wind
    }

    // Bools to check if the player can actually use powerups (i.e. they have collected them)
    public bool fire, lightning, ice, wind;

    // Bool to determine if player can attack
    public bool canAttack;

    // Bool to determine if player can heal
    public bool canHeal;

    // Bool to determine if player has the map, and one for it is open or closed - false = closed, true = open
    public bool hasMap, mapState;

    // Bool to determine if player has the torch, and one for if it is on or off - false = off, true = on
    public bool hasTorch, torchState;

    // List of collected healing plants
    public int collectedHealingPlants;
    #endregion

    #region Time of Day Stuff
    [Header ("Time of Day Stuff")]

    // Hold references to sun light
    public GlobalLightMove sun;

    // Enum for day and night
    public enum TimeOfDay
    {
        day,
        night
    }
    public TimeOfDay timeOfDay;

    // Float to act as time
    public float currentTime;

    // Float to act as time increasing - same value as sun increasing and decreasing to match the visuals of sun to actual game day time
    public float timeChange;

    // Bool to determine if time should decrease
    public bool shouldDecrease;

    // Bool to determine if time should increase
    public bool shouldIncrease;
    #endregion

    #region Gameplay
    // This Vector3 will act as a 'checkpoint' - send player back here whenever they die. Set manually at certain locations
    [SerializeField] private Vector3 checkpoint;
    #endregion

    #region Misc
    [Header("Misc")]
    // Reference to crossfade animator
    public Animator crossfadeAnimator;

    public Bandit bandit;

    public PlayerMovement player;

    // Bool to determine if player is in dialogue
    public bool isInDialogue;

    // Hold reference to PlayerAttacks
    public PlayerAttacks playerAttacks;

    // Hold reference to Village text
    public TMP_Text proceedToVillageText;

    // Cause of death text
    public TMP_Text causeOfDeathText;

    // List of enemies to set active at night
    public List<GameObject> enemyList;

    // Get KnightBoss
    public GameObject bossKnight;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        powerups = Powerups.none;

        canAttack = true;

        currentTime = 1;
        timeChange = 0.0001f;
        shouldDecrease = true;
        shouldIncrease = false;

        maxHealth = 100;
        currentHealth = 90;

        currentXP = 0;
        playerLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIElements();
        CheckHealth();
        CheckMouseInput();
        CheckKeyboardInput();
        CheckTime();

        if (!proceedToVillageText.IsDestroyed())
        {
            CheckIfCollectedAllPowerups();
        }
        else
        {
            return;
        }
    }

    #region Health, Level
    // Remove passed in amount from player health
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        FindObjectOfType<AudioManager>().Play("PlayerHurt1");
    }

    // Add passed in amount to player health
    public void Heal(float amount)
    {
        if (currentHealth == maxHealth)
        {
            Debug.Log("Health is already at max!");
        }
        else
        {
            currentHealth += amount;
        }
    }

    // Add passed in amount to player score
    public void GiveXP(float pXP)
    {
        // Add it on
        currentXP += pXP;

        // Check if we need to level up
        CheckXPAmount();
    }
    #endregion

    #region Checks
    // Check player health
    public void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die("Misadventure");
        }
    }
    // Check amount of XP player has and level up if need to 
    private void CheckXPAmount()
    {
        // For every 100 XP player earns, level up once
        if (currentXP >= 100)
        {
            // Level up 
            playerLevel += (int)currentXP / 100;

            // Set XP back to 0
            currentXP = 0;
        }
    }

    // Handle mouse inputs
    private void CheckMouseInput()
    {
        if (Input.GetButtonDown("Fire1") && canAttack && !isInDialogue)
        {
            if (powerups == Powerups.none)
            {
                playerAttacks.MeleeAttack();
                bandit.AttackAnim();
                canAttack = false;
            }
            else if (powerups == Powerups.fire && fire)
            {
                playerAttacks.FireAttack();
                canAttack = false;
            }
            else if (powerups == Powerups.lightning && lightning)
            {
                playerAttacks.LightningAttack();
                canAttack = false;
            }
            else if (powerups == Powerups.ice && ice)
            {
                playerAttacks.IceAttack();
                canAttack = false;
            }
            else if (powerups == Powerups.wind && wind)
            {
                playerAttacks.WindAttack();
                canAttack = false;
            }
        }
    }

    // Handle keyboard inputs
    private void CheckKeyboardInput()
    {
        #region Powerups
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            powerups = Powerups.fire;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            powerups = Powerups.lightning;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            powerups = Powerups.ice;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            powerups = Powerups.wind;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            powerups = Powerups.none;
        }
        #endregion

        // If player has the heal ability, has healing plants in their inventory, is missing some health and presses E
        if (Input.GetKeyDown(KeyCode.E) && canHeal && collectedHealingPlants > 0 && currentHealth < maxHealth)
        {
            // If player has no plants
            if (collectedHealingPlants == 0)
            {
                Debug.Log("No healing plants in inventory!");
            }
            else
            {
                Heal(10);
                collectedHealingPlants--;
                Debug.Log("Healed for 10!");
            }
        }

        // If player presses R, has map
        if (Input.GetKeyDown(KeyCode.R) && hasMap)
        {
            // If map is closed
            if (mapState == false)
            {
                // Set mapState to true, i.e. open
                mapState = true;
            }
            // Else if map is open
            else if (mapState == true)
            {
                // Set map state to false, i.e. closed
                mapState = false;
            }

            // Call UseMap function and pass in state
            playerAttacks.UseMap(mapState);
        }

        // If player presses E, has torch
        if (Input.GetKeyDown(KeyCode.E) && hasTorch)
        {
            // If torch is off
            if (!torchState)
            {
                // Turn torch on
                torchState = true;
            }
            // Else if torch is on
            else if (torchState)
            {
                // Turn torch off
                torchState = false;
            }

            // Call UseTorch and pass in state
            playerAttacks.UseTorch(torchState);
        }
    }

    // Handle day/night cycle and related functions
    private void CheckTime()
    {
        // If it is day time i.e. time should decrease
        if (shouldDecrease)
        {
            // Decrease current time
            currentTime -= timeChange;

            sun.Decrease();
        }
        // Else if it is night time i.e time should increase
        else if (shouldIncrease)
        {
            currentTime += timeChange;

            sun.Increase();
        }

        // If time reaches max day
        if (currentTime >= 1)
        {
            // Stop time from increasing
            shouldIncrease = false;

            // Set enum to day
            timeOfDay = TimeOfDay.day;

            // Wait at day time for 90 seconds
            StartCoroutine(WaitAtDay());
        }
        // Else if time reaches max night
        else if (currentTime <= 0.15)
        {
            // Stop time from decreasing
            shouldDecrease = false;

            // Set enum to night
            timeOfDay = TimeOfDay.night;

            // Wait at night time for 90 seconds
            StartCoroutine(WaitAtNight());
        }

        // If there are enemies in enemyList
        if (enemyList.Count > 0)
        {
            // If it is night time
            if (timeOfDay == TimeOfDay.night)
            {
                // Set each enemy in it to active
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].SetActive(true);
                }
            }
            // Else if it is day time
            else
            {
                // Set each enemy in the list to inactive
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].SetActive(false);
                }
            }
        }
    }

    private void UpdateUIElements()
    {
        healthText.text = currentHealth + "/" + maxHealth;
        currentLevelText.text = playerLevel.ToString();
        collectedPlantsText.text = collectedHealingPlants.ToString();
        healthBar.SetSlider(currentHealth);
    }

    private void CheckIfCollectedAllPowerups()
    {
        if (fire && lightning && ice && wind && !proceedToVillageText.IsDestroyed())
        {
            proceedToVillageText.enabled = true;
            proceedToVillageText.text = "All powerups acquired! \n Proceed to Highpoint Village";
            StartCoroutine(RemoveText());
        }
    }
    #endregion

    #region Gameplay
    // Handle player death
    public void Die(string cause)
    {
        // Display the cause of death
        causeOfDeathText.GetComponent<TextMeshProUGUI>().enabled = true;
        causeOfDeathText.text = cause;

        // Play sound
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        // Send player back to checkpoint location
        player.SetPosition(checkpoint);

        // Trigger fade
        StartCoroutine(DeathCrossfade(2));

        // Freeze movement for a second
        StartCoroutine(player.StopPlayerMoving(2));
    }

    // Set respawn point for player
    public void SetCheckpoint(Vector3 pCheckpoint)
    {
        Debug.Log("Checkpoint set at: " + pCheckpoint);
        checkpoint = pCheckpoint;
    }
    #endregion

    #region Coroutines
    // Wait at day time
    IEnumerator WaitAtDay()
    {
        yield return new WaitForSeconds(90);

        // Start time decreasing
        shouldDecrease = true;
    }

    // Wait at night time
    IEnumerator WaitAtNight()
    {
        yield return new WaitForSeconds(90);

        // Start time increasing 
        shouldIncrease = true;
    }

    public IEnumerator AreaCrossfade(float seconds)
    {
        // Stop player from moving while it fades
        player.CanMove = false;

        // Start regular crossfade animation
        crossfadeAnimator.SetTrigger("Start");

        // Make player invisible
        player.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Wait
        yield return new WaitForSeconds(seconds);

        // Start reverse crossfade animation
        crossfadeAnimator.SetTrigger("Reverse");

        // Make player visible
        player.gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // Alow player to move again
        player.CanMove = true;
    }

    public IEnumerator DeathCrossfade(float seconds)
    {
        // Start regular crossfade animation
        crossfadeAnimator.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(seconds);

        // Disable text
        causeOfDeathText.GetComponent<TextMeshProUGUI>().enabled = false;

        // Start reverse crossfade animation
        crossfadeAnimator.SetTrigger("Reverse");
    }

    // Remove text after a certain time
    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(5);
        Destroy(proceedToVillageText);
    }
    #endregion
}

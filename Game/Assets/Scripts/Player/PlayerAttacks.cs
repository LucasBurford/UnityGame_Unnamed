using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerAttacks : MonoBehaviour
{
    #region References
    // Reference to GameManager
    public GameManager gameManager;

    // Reference to Bandit script
    public Bandit bandit;

    // Reference to Hit effect
    public GameObject hitEffect;

    // Point that all attacks originate from
    public Transform attackPoint;

    // Melee attack range
    public float attackRange;

    // Enemy layers
    public LayerMask enemyLayers;

    // Reference to cameras
    public Camera mainCam;
    public Camera mapCam;

    // HUD canvas
    public Canvas HUD;

    // Map markers 
    public GameObject mapMarkers;

    // Reference to player rigid body
    public Rigidbody2D playerRB;

    // Reference to torch light
    public Light2D torchLight;

    // Mouse position
    Vector2 mousePos;
    #endregion

    #region Attack prefabs
    // FireBall prefab
    public GameObject firePrefab;
    // Lightning Strike prefab
    public GameObject lightningPrefab;
    // Ice Attack prefab
    public GameObject icePrefab;
    // Wind Attack prefab
    public GameObject windPrefab;
    #endregion

    #region Attack forces
    // FireBall shoot force
    public float fireForce;
    public float fireSpeed;
    // IceAttack shoot forece
    public float iceForce;
    public float iceSpeed;
    // WindAttack shoot force
    public float windForce;
    public float windSpeed;
    // MeleeAttack damage
    public float meleeDamage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Initialise force values
        fireForce = 100;
        iceForce = 75;
        windForce = 40;

        fireSpeed = 50;
        iceSpeed = 25;
        windSpeed = 15;

        meleeDamage = 30;

        attackRange = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly get mouse pos to use lightning attack
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    // Cast Melee Attack
    public void MeleeAttack()
    {
        StartCoroutine(PlaySwordSwipe());
    }

    // Cast FireBall Attack
    public void FireAttack()
    {
        GameObject bullet = Instantiate(firePrefab, mousePos, attackPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // If player is facing left
        if (bandit.facingDirection == 1)
        {
            rb.velocity = transform.right * -fireSpeed;
        }
        // If player is facing right
        else if (bandit.facingDirection == -1)
        {
            rb.velocity = transform.right * fireSpeed;
        }

        FindObjectOfType<AudioManager>().Play("FireBall");

        StartCoroutine(ResetAttackTime());
    }

    // Cast Lightning Attack
    public void LightningAttack()
    {
        // Instantiate lighting prefab at mouse position
        GameObject lightningAttack = Instantiate(lightningPrefab, mousePos, attackPoint.rotation);

        // Play lightning sound
        FindObjectOfType<AudioManager>().Play("LightningStrike");

        StartCoroutine(ResetAttackTime());
    }

    // Cast Ice Attack
    public void IceAttack()
    {
        // Spawn the ice prefab
        GameObject iceAttack = Instantiate(icePrefab, mousePos, attackPoint.rotation);
        // Add rigid body to it
        Rigidbody2D rb = iceAttack.GetComponent<Rigidbody2D>();
        // Send the attack out
        // If player is facing left
        if (bandit.facingDirection == 1)
        {
            rb.velocity = transform.right * -iceSpeed;
        }
        // If player is facing right
        else if (bandit.facingDirection == -1)
        {
            rb.velocity = transform.right * iceSpeed;
        }

        // Play Ice Attack sound
        FindObjectOfType<AudioManager>().Play("IceAttack");

        StartCoroutine(ResetAttackTime());
    }
    
    // Cast Wind Attack
    public void WindAttack()
    {
        // Spawn the wind prefab
        GameObject windAttack = Instantiate(windPrefab, mousePos, attackPoint.rotation);
        // Add rigid body to it
        Rigidbody2D rb = windAttack.GetComponent<Rigidbody2D>();
        // Send the attack out
        // If player is facing left
        if (bandit.facingDirection == 1)
        {
            rb.velocity = transform.right * -windSpeed;
        }
        // If player is facing right
        else if (bandit.facingDirection == -1)
        {
            rb.velocity = transform.right * windSpeed;
        }

        // Play Wind Attack sound
        FindObjectOfType<AudioManager>().Play("WindAttack");

        StartCoroutine(ResetAttackTime());
    }

    // Open or close the map
    public void UseMap(bool state)
    {
        // If map is closed
        if (state == false)
        {
            // Switch normal camera
            mainCam.enabled = true;
            mapCam.enabled = false;

            HUD.enabled = true;

            mapMarkers.SetActive(false);
        }
        // Else if map is open
        else if (state == true)
        {
            // Switch to other camera to simulate map of world
            mainCam.enabled = false;
            mapCam.enabled = true;

            HUD.enabled = false;

            mapMarkers.SetActive(true);
        }
    }

    // Turn torch on or off
    public void UseTorch(bool state)
    {
        // If state is false, i.e. the torch is OFF
        if (state == false)
        {
            // Turn the torch on
            torchLight.enabled = true;
        }
        // Else if state is true, i.e. the torch is ON
        if (state == true)
        {
            // Turn the torch off
            torchLight.enabled = false;
        }
    }

    IEnumerator PlaySwordSwipe()
    {
        yield return new WaitForSeconds(0.3f);

        // Detect enemies in range
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach (Collider2D hObject in hitObjects)
        {
            if (hObject.tag == "Enemy")
            {
                hObject.GetComponent<Enemy>().TakeDamage(meleeDamage);
            }

            if (hObject.tag == "BossKnight")
            {
                hObject.GetComponent<BossKnight>().TakeDamage(meleeDamage);
            }

            FindObjectOfType<AudioManager>().Play("SwordHit");
        }

        FindObjectOfType<AudioManager>().Play("SwordSwipe");

        StartCoroutine(ResetAttackTime());
    }

    IEnumerator ResetAttackTime()
    {
        yield return new WaitForSeconds(1);

        gameManager.canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

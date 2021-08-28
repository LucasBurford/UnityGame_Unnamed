using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class WindRotate : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject enemy;

    public Light2D light2D;

    public TMP_Text windAttainedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Give ability
        gameManager.wind = true;
        // Set aiblity to active
        gameManager.powerups = GameManager.Powerups.wind;

        // Set text to enables
        windAttainedText.enabled = true;
        // Set text
        windAttainedText.text = "New ability gained: \n WIND";

        // Destroy light
        Destroy(light2D);

        // Set enemy active
        enemy.SetActive(true);

        // Move object away
        gameObject.transform.position = new Vector3(1000, 1000);

        // Remove text and object after 4 seconds
        StartCoroutine(RemoveText());
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(4);
        Destroy(windAttainedText);
        Destroy(gameObject);
    }
}

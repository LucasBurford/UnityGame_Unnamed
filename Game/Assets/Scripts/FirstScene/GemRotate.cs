using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemRotate : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public ParticleSystem gemParticles;

    public GameObject player;
    public TMP_Text speedAttainedText;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 2, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight" || collision.gameObject.name == "Player_Dae")
        {
            gameManager.GiveXP(10);
            playerMovement.moveSpeed = 8f;

            speedAttainedText.enabled = true;
            speedAttainedText.text = "Move speed increased!";

            gameObject.transform.position = new Vector3(1000, 1000);
            FindObjectOfType<AudioManager>().Play("GemCollect");
            gemParticles.Stop();
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4);
        Destroy(speedAttainedText);
        Destroy(gameObject);
    }
}

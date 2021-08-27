using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class FireRotate : MonoBehaviour
{
    public ParticleSystem ps;
    public GameManager gameManager;
    public GameObject enemy;
    public Light2D light2D;

    public TMP_Text fireAttainedText;

    private void Start()
    {
        fireAttainedText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            FindObjectOfType<AudioManager>().Play("PowerupAcquire");
            ps.Stop();
            gameManager.fire = true;
            gameManager.powerups = GameManager.Powerups.fire;
            gameObject.transform.position = new Vector3(1000, 1000);
            fireAttainedText.enabled = true;
            fireAttainedText.text = "New ability gained: FIRE";
            Destroy(light2D);
            enemy.SetActive(true);
            StartCoroutine(RemoveText());
        }
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(5);
        Destroy(fireAttainedText);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossKnightDeath : MonoBehaviour
{
    public GameManager gameManager;
    public CameraFollow cam;
    public TMP_Text victoryText;
    public AudioClip newClip;

    public void OnBossKnightDeath()
    {
        FindObjectOfType<AudioManager>().Play("KnightBossVictory");
        FindObjectOfType<AudioManager>().Stop("KnightFightBGMusic");
        cam.ChangeAudioClip(newClip);
        cam.gameObject.GetComponent<AudioSource>().Play();

        gameManager.maxHealth = 120;
        gameManager.GiveXP(200);

        victoryText.text = "Congratulations! \n Max health increased \n 200 XP gained";
        StartCoroutine(RemoveText(5));
    }

    IEnumerator RemoveText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(victoryText);
        Destroy(gameObject);
    }

}

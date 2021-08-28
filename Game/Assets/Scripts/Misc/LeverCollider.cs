using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCollider : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject leverDown;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight" || collision.gameObject.name == "Player_Dae")
        {
            FindObjectOfType<AudioManager>().Play("LeverPull");
            FindObjectOfType<AudioManager>().Play("FaceBlockSliding");
            ps.Stop();
            FindObjectOfType<FaceBlock>().x = true;
            //GameObject.Find("crank-down").transform.position = new Vector3(-16, 0.25f, 1);
            leverDown.SetActive(true);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchRotate : MonoBehaviour
{
    public GameManager gameManager;
    public Light2D torchLight;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            gameManager.hasTorch = true;
            torchLight.enabled = true;

            FindObjectOfType<ItemAcquired>().DisplayItemText("Torch \n Press E to use", 5);

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    #region Members
    [Header("References")]
    public PlayerMovement playerMovement;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            playerMovement.surface = PlayerMovement.Surface.stone;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player_Knight")
        {
            playerMovement.surface = PlayerMovement.Surface.grass;
        }
    }
    #endregion
}

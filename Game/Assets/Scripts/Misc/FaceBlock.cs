using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBlock : MonoBehaviour
{
    public bool x;

    // Update is called once per frame
    void Update()
    {
        if (x)
        {
            transform.Translate(0, -1f * Time.deltaTime, 0);

            StartCoroutine(DestroyFaceBlock());
        }
        else
        {
            return;
        }
    }

    IEnumerator DestroyFaceBlock()
    {
        yield return new WaitForSeconds(7);

        Destroy(gameObject);
    }
}

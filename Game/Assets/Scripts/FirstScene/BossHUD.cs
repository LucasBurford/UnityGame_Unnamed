﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHUD : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(target.position.x, target.position.y + 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

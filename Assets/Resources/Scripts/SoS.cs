﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoS : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}

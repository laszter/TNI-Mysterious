using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AjChanZone : MonoBehaviour {

    private bool playerDetected;

    public bool PlayerDetected
    {
        get { return playerDetected; }
        set { playerDetected = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            
        }
    }
}

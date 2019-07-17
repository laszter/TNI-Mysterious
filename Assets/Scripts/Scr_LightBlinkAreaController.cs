using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LightBlinkAreaController : MonoBehaviour {

    [SerializeField] private Scr_Light lightTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightTarget.lightFlashing = 5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightTarget.lightFlashing = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RoomSwitch : MonoBehaviour {

    [SerializeField] private GameObject roomA;
    [SerializeField] private GameObject roomB;
    Collider coll;

    bool playerEnter;

    // Use this for initialization
    void Start () {
        coll = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerEnter)
        {
            roomA.SetActive(false);
            roomB.SetActive(true);
            coll.enabled = false;
            this.enabled = false;
        }
    }
}

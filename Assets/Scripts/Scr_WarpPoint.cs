using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_WarpPoint : MonoBehaviour {

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
            //other.transform.position = new Vector3(other.transform.position.x,42f, other.transform.position.z);
            other.transform.position += new Vector3(0, 34.62f, 0);
        }
    }
}

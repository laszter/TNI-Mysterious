using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Cursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Input.mousePosition;
	}
}

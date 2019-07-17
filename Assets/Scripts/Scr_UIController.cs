using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_UIController : MonoBehaviour {

    [SerializeField] private GameObject[] disappearObject;

	// Use this for initialization
	void Start () {
        DefaultUI();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        DefaultUI();
    }

    public void DefaultUI()
    {
        for(int i = 0; i < disappearObject.Length; i++)
        {
            disappearObject[i].SetActive(false);
        }
    }
}

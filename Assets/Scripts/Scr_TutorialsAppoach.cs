using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TutorialsAppoach : MonoBehaviour {

    [SerializeField] private GameObject uiPanel;
    bool triggered;

    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;

	// Use this for initialization
	void Start () {
        uiPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (triggered)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                uiPanel.SetActive(false);
                player.enabled = true;
                this.enabled = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            uiPanel.SetActive(true);
            player.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BoxSoundController : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip crySound;
    bool play;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !play)
        {
            if(audioSource == null)
            {
                this.enabled = false;
            }
            audioSource.clip = crySound;
            audioSource.loop = true;
            audioSource.Play();
            play = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && play)
        {
            audioSource.Pause();
            audioSource.loop = false;
            play = false;
        }
    }
}

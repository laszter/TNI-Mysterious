using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Scr_Mangmao : MonoBehaviour {

    ParticleSystem mangmaoParticle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            mangmaoParticle = Scr_ObjectPooler.Instance.GetPoolingObject("Mangmao").GetComponent<ParticleSystem>();
            mangmaoParticle.transform.position = gameObject.transform.position;
            mangmaoParticle.gameObject.transform.parent = gameObject.transform;
            mangmaoParticle.Play();
            Debug.Log("Mangmao Deploy");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (mangmaoParticle != null)
            {
                mangmaoParticle.Stop();
                mangmaoParticle.gameObject.SetActive(false);
                mangmaoParticle = null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BoxController : MonoBehaviour {

    public GameObject player;
    private Rigidbody[] rigids;
    public bool triggered;
    public AudioClip impactSound;
    public AudioClip screamSound;
    public GameObject doll_sleep;
    public GameObject doll_sit;
    public GameObject doll_stand;
    public GameObject doll_item;
    public Scr_DollSitPoint sitpoint;
    bool countDown;
    float time = 10f;
    float timer;
    int step;

	// Use this for initialization
	void Start () {
        rigids = GetComponentsInChildren<Rigidbody>();
        step = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (countDown)
        {
            timer += Time.deltaTime;
            if(timer > time)
            {
                for (int i = 0; i < rigids.Length; i++)
                {
                    rigids[i].useGravity = true;
                }
                countDown = false;
            }

            if(timer < 1 && step == 0)
            {
                Debug.Log("Step 0");
                doll_sit.SetActive(false);
                doll_stand.SetActive(true);
                doll_stand.transform.position = player.transform.position + (player.transform.forward * 0.8f);
                doll_stand.transform.position += new Vector3(0, 0.2f, 0);
                step += 1;
            }
            else if (timer > 2.2f && step == 1)
            {
                doll_stand.SetActive(false);
                doll_item.SetActive(true);
                step += 1;
            }
        }

        if (sitpoint.triggered)
        {
            doll_sleep.SetActive(false);
            doll_sit.SetActive(true);
            sitpoint.gameObject.SetActive(false);
            sitpoint.triggered = false;
        }
	}

    public void StartImpact()
    {
        GetComponent<AudioSource>().PlayOneShot(impactSound);
        Vector3 direction;
        float speed;
        for (int i = 0; i < rigids.Length; i++)
        {
            rigids[i].mass = 0.1f;
        }
        for (int i = 0; i < rigids.Length; i++)
        {
            direction = Random.insideUnitSphere.normalized;
            speed = Random.Range(100f, 200f);
            rigids[i].useGravity = false;
            rigids[i].AddForce(direction * speed);
            countDown = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            StartImpact();
            triggered = true;
        }
    }
}

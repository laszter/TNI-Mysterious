using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ChanIdle : MonoBehaviour {

    bool action;
    bool hit;
    Animator anim;
    Transform targetLookAt;
    [SerializeField] GameObject player;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        targetLookAt = player.transform;
    }
	
	// Update is called once per frame
	void Update () {
        LookAtTarget(500f);
	}

    public void ResumeState()
    {
        action = false;
        hit = true;
    }

    protected void LookAtTarget(float rotSpd)
    {
        if (targetLookAt == null) return;

        Vector3 targetPos = targetLookAt.transform.position;
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        Vector3 desiredDirection = targetPos - transform.position;
        Vector3 rot = Vector3.RotateTowards(transform.forward, desiredDirection, Mathf.Deg2Rad * rotSpd * Time.deltaTime, 1);
        transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<Scr_Trap>() != null && coll.GetComponent<Scr_Trap>().dmg > 0)
        {
            action = true;
            Scr_Trap trap = coll.GetComponent<Scr_Trap>();
            anim.SetBool("beware", true);
            anim.SetTrigger("triggerAny");
            targetLookAt = coll.transform;
        }
    }

    public bool IsHit()
    {
        return hit;
    }
}

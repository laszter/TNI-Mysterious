using UnityEngine;

public class Scr_ChiaculonaGhost : MonoBehaviour {

    private Scr_Light lightGhost;
    private bool triggered;
    private bool lightEnterEvent;
    private bool hasSeen;
    private GameObject target;
    private float delayTriggerTime;

	// Use this for initialization
	void Start () {
        lightGhost = GetComponentInChildren<Scr_Light>();
        triggered = false;
        lightEnterEvent = false;
        hasSeen = false;
        delayTriggerTime = 5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (triggered)
        {
            if(target != null)
                LookAtPlayer(180f,true);
        }
	}

    private void GhostTriggered()
    {
        if (triggered) return;
        Debug.Log("Triggered");
        //transform.position = target.transform.position + (target.transform.right.normalized * -2f) + (target.transform.forward.normalized * 2f) - new Vector3(0, 1f, 0);
        transform.position = new Vector3(-7.15f,6.45f,18.13f);
        triggered = true;
    }

    private void LightEventEnter()
    {
        if (lightEnterEvent) return;

        lightGhost.SetThisLightIsOnEvent(true);
        lightEnterEvent = true;
    }

    private void LightEventOut()
    {
        if (!lightEnterEvent) return;

        lightGhost.SetThisLightIsOnEvent(false);
        lightEnterEvent = false;
    }

    public void HasBeenSeen()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        LightEventEnter();
        hasSeen = true;
        DelayTrigger();
        Debug.Log("HasBeenSeen " + target.name);
    }

    public void NotSee()
    {
        LightEventOut();
        hasSeen = false;
    }


    protected void LookAtPlayer(float rotSpd, bool lockVertical)
    {
        Vector3 targetPos = target.transform.position;
        if (lockVertical) targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        Vector3 desiredDirection = targetPos - transform.position;
        Vector3 rot = Vector3.RotateTowards(transform.forward, desiredDirection, Mathf.Deg2Rad * rotSpd * Time.deltaTime, 1);
        transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    private void DelayTrigger()
    {
        if (triggered) return;
        if (delayTriggerTime > 0) delayTriggerTime -= Time.deltaTime;
        else
        {
            if (target != null)
            {
                GhostTriggered();
            }
        }
        //Debug.Log("Delay " + delayTriggerTime);
    }
}

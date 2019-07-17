using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Door : MonoBehaviour {

    [SerializeField] bool ClockWheel;
    private bool moving;
    private bool open;
    private Vector3 origin;
    private Vector3 endpos;
    private float scalingTimer;
    [SerializeField]
    private float scalingTime;

    // Use this for initialization
    void Start () {
        origin = transform.eulerAngles;
        endpos = origin;
        if (ClockWheel)
        {
            endpos += new Vector3(0,75f,0);
        }
        else
        {
            endpos -= new Vector3(0, 75f, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            scalingTimer += Time.deltaTime;
            if (scalingTimer >= scalingTime)
            {
                scalingTimer = scalingTime;
                moving = false;
            }
            //Scaling InformationBox

            if(open)
                transform.eulerAngles = Vector3.Lerp(origin, endpos, scalingTimer / scalingTime);
            else
                transform.eulerAngles = Vector3.Lerp(endpos, origin, scalingTimer / scalingTime);
        }
	}

    public void InteractDoor()
    {
        if (moving) return;

        open = !open;
        moving = true;
        scalingTimer = 0;
    }

    public bool IsOpen()
    {
        return open;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Pickable : MonoBehaviour {

    public float distance;
    private float speedRotate = 50f;
    public Vector3 defaultRotation;
    GameObject targetLookAt;
    bool picking;

    private float speed = 0;
    private float av = 500f;

    private void Start()
    {
        targetLookAt = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (picking)
        {
            ResetRotate();

            if (Input.GetKey(KeyCode.E))
            {
                speed += av * Time.deltaTime;
                speed = Mathf.Clamp(speed, -1000f, 1000f);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                speed -= av * Time.deltaTime;
                speed = Mathf.Clamp(speed, -1000f, 1000f);
            }
            else
            {
                speed = Mathf.Lerp(speed,0, 8f * Time.deltaTime);
            }

            transform.eulerAngles += new Vector3(0, speed, 0) * Time.deltaTime;
        }
    }

    private void ResetRotate()
    {
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(defaultRotation.x, transform.eulerAngles.y, defaultRotation.z), Time.deltaTime * 5f);
    }

    public void Droped()
    {
        picking = false;
    }

    public void Picked()
    {
        picking = true;
    }

}

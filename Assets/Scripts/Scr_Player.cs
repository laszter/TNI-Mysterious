using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperBlur;
using UnityEngine.SceneManagement;

public class Scr_Player : MonoBehaviour {
    private static Scr_Player mInstance;
    public static Scr_Player Instance
    {
        get
        {
            return mInstance;
        }
    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }

    [SerializeField] private Camera playerCam;
    private GameObject targetGhost_1;

    private bool ghost1_HasSeen;
    int mask;

    [SerializeField] private SuperBlurBase blur;

	// Use this for initialization
	void Start () {
        ghost1_HasSeen = false;
        targetGhost_1 = GameObject.Find("chiaculona");
        mask = ~(1 << LayerMask.NameToLayer("Picking") | 1 << LayerMask.NameToLayer("IgnoreRaycast"));
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Vector3 screenPoint = playerCam.WorldToViewportPoint(targetGhost_1.transform.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //Debug.Log("Ghost OnScreen : " + IsInView(this.gameObject,targetGhost_1));
        if(targetGhost_1!=null)
        if(IsInView(this.gameObject, targetGhost_1))
        {
            ghost1_HasSeen = true;
            targetGhost_1.GetComponent<Scr_ChiaculonaGhost>().HasBeenSeen();
        }
        else if(!IsInView(this.gameObject, targetGhost_1) && ghost1_HasSeen)
        {
            ghost1_HasSeen = false;
            targetGhost_1.GetComponent<Scr_ChiaculonaGhost>().NotSee();
        }

        /*foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light"))
        {
            if (light.GetComponent<Scr_Light>() == null) continue;
            if (IsInView(this.gameObject, light, true))
            {
                light.GetComponent<Scr_Light>().InPlayerView();
            }
            else if(!IsInView(this.gameObject, light, true))
            {
                light.GetComponent<Scr_Light>().OutPlayerView();
            }
            //Debug.Log(light.name + " : " + IsInView(this.gameObject, light, true));
        }*/

        CameraShakeCheck();
    }

    private bool IsInView(GameObject origin, GameObject toCheck ,bool noMesh = false)
    {
        Vector3 pointOnScreen;
        if (!noMesh) pointOnScreen = playerCam.WorldToScreenPoint(toCheck.GetComponentInChildren<MeshRenderer>().bounds.center);
        else pointOnScreen = playerCam.WorldToScreenPoint(toCheck.transform.position);
        //Is in front
        if (pointOnScreen.z < 0)
        {
            //Debug.Log("Behind: " + toCheck.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            //Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = toCheck.transform.position - origin.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        Vector3 pointT;
        if (!noMesh) pointT = toCheck.GetComponentInChildren<Renderer>().bounds.center;
        else pointT = toCheck.transform.position;

        Debug.DrawRay(playerCam.transform.position, pointT - playerCam.transform.position);
        if (Physics.Linecast(playerCam.transform.position, pointT, out hit, mask))
        {
            if (hit.transform.name != toCheck.name)
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                //Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        Debug.Log(toCheck.name + " True");
        return true;
    }

    private bool shaking = false;

    // Amplitude of the shake. A larger value shakes the camera harder.
    private float shakeAmount = 0.05f;

    private Vector3 originalPos;

    private void InitCameraShake()
    {
        originalPos = playerCam.transform.localPosition;
        shaking = true;
    }

    private void CameraShakeCheck()
    {
        if (shaking)
        {
            playerCam.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            OnHit();
            if (blur.interpolation >= 1f) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void ResetCameraShake()
    {
        playerCam.transform.localPosition = originalPos;
        shaking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            InitCameraShake();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            ResetCameraShake();
            blur.interpolation = 0f;
        }
    }

    private void OnHit()
    {
        if (blur.interpolation <= 1f) blur.interpolation += Time.deltaTime * 0.3f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Interactor : MonoBehaviour {

	Camera mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float pickupDistance = 2f;
    private float objectDistance;
	public float pickupSmooth;
    private GameObject hitObject = null;
    private GameObject currentHitObject;
    [SerializeField] private GameObject tagPrefab;
    private GameObject tag;
    private LayerMask tempLayer;
    private float tempMass;
    [SerializeField] float distanceofraycast;

    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
        tag = Instantiate(tagPrefab);
        HideTag();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (carrying)
        {
            Carry(carriedObject);
            CheckDrop();
        }
        else
        {
            RayCastBeam();
        }
	}

	void RotateObject(){
		carriedObject.transform.Rotate (5, 10, 15);
	}

	void Carry(GameObject obj)
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        float carryDistance = 0f;
        int mask = ~(1 << LayerMask.NameToLayer("Picking") | 1 << LayerMask.NameToLayer("IgnoreRaycast"));
        if (Physics.Raycast(ray, out hit, objectDistance, mask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            distanceofraycast = Vector3.Distance(ray.origin, hit.point);
            if (Vector3.Distance(ray.origin, hit.point) < objectDistance)
            {
                carryDistance = Vector3.Distance(ray.origin, hit.point);
            }
            else
            {
                carryDistance = objectDistance;
            }
        }
        else {
            carryDistance = objectDistance;
        }
        obj.transform.position = Vector3.Lerp(obj.transform.position, mainCamera.transform.position + mainCamera.transform.forward * carryDistance, Time.deltaTime * pickupSmooth);
    
    }

	void RayCastBeam(){
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        Ray ray = mainCamera.ScreenPointToRay (new Vector3 (x, y));
		RaycastHit hit;
        int mask = ~(1 << LayerMask.NameToLayer("Picking") | 1 << LayerMask.NameToLayer("IgnoreRaycast"));
        if (Physics.Raycast (ray, out hit,100f,mask)) {

            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Scr_InteractObject interactObject = hit.collider.GetComponent<Scr_InteractObject>();
            Scr_Pickable pickable = hit.collider.GetComponent<Scr_Pickable>();
            Scr_Door door = hit.collider.GetComponent<Scr_Door>();
            distanceofraycast = Vector3.Distance(ray.origin, hit.point);
            if (Vector3.Distance(ray.origin, hit.point) > pickupDistance)
            {
                if (hitObject != null)
                    OnReticleExit();
                return;
            }

            if (hitObject != hit.transform.gameObject)
            {
                if (hitObject != null)
                {
                    //------------------- OnReticleExit --------------------
                    OnReticleExit();
                }
                else
                {
                    //------------------- OnReticleEnter --------------------
                    OnReticleEnter(hit.transform.gameObject);
                }
            }
            else
            {
                //hitObject.SendMessage("OnReticleHover", sendParams); // Trigger "OnReticleHover"
                TagForwardToPlayer(tag, this.gameObject, 1000, false);
                TagOnHover(hit.point);
                if (interactObject != null && hit.collider.isTrigger)
                {
                    ShowTag();
                }

                //---------------------- Click --------------------------------------
                //------------------- PickUp Object ----------------------------------------
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (pickable != null && hit.collider.isTrigger)
                    {
                        pickable.Picked();
                        carrying = true;
                        carriedObject = pickable.gameObject;
                        objectDistance = carriedObject.GetComponent<Scr_Pickable>().distance;
                        if (objectDistance <= 0) objectDistance = pickupDistance;
                        carriedObject.GetComponent<Rigidbody>().useGravity = false;
                        tempMass = carriedObject.GetComponent<Rigidbody>().mass;
                        carriedObject.GetComponent<Rigidbody>().mass = 0;
                        tempLayer = carriedObject.layer;
                        carriedObject.gameObject.layer = LayerMask.NameToLayer("Picking");
                        Scr_Trap trap = carriedObject.GetComponentInChildren<Scr_Trap>();
                        if (trap != null)
                        {
                            Collider[] colls = trap.gameObject.GetComponentsInChildren<Collider>();
                            for(int i = 0; i < colls.Length; i++)
                            {
                                if (colls[i].isTrigger)
                                    colls[i].enabled = false;
                            }
                        }

                        HideTag();
                    }
                    
                    if(door != null && hit.collider.isTrigger)
                    {
                        door.InteractDoor();
                    }
                }
            }
		}
        else
        {
            if (hitObject != null)
            {
                //------------------- OnReticleExit --------------------
                OnReticleExit();
            }
            hitObject = null;
        }
    }

    private void OnReticleExit()
    {
        Scr_InteractObject interactObject = hitObject.GetComponent<Scr_InteractObject>();

        //Debug.Log("OnPointerExit" + hitObject.name);
        if (interactObject != null)
        {
            Renderer rend = interactObject.gameObject.GetComponent<Renderer>();
            Shader shader1 = Shader.Find("Standard");
            if (rend == null)
            {
                Scr_InteractObject[] interactInChild = interactObject.gameObject.GetComponentsInChildren<Scr_InteractObject>();
                for (int i = 0; i < interactInChild.Length; i++)
                {
                    Renderer rendInChild = interactInChild[i].gameObject.GetComponent<Renderer>();
                    if(rendInChild != null)
                    foreach (Material mat in rendInChild.materials)
                    {
                        mat.shader = shader1;
                    }
                }
            }
            else
            {
                foreach (Material mat in rend.materials)
                {
                    mat.shader = shader1;
                }
            }
            HideTag();
        }
        hitObject = null;
    }

    private void OnReticleEnter(GameObject hit)
    {
        hitObject = hit;
        Scr_Pickable pickable = hitObject.GetComponent<Scr_Pickable>();
        Scr_Door door = hitObject.GetComponent<Scr_Door>();
        if (pickable != null)
        {
            Renderer rend = pickable.gameObject.GetComponent<Renderer>();
            Shader shader2 = Shader.Find("Outlined/Custom");
            if (rend == null)
            {
                Scr_InteractObject[] interactInChild = pickable.gameObject.GetComponentsInChildren<Scr_InteractObject>();
                for (int i = 0; i < interactInChild.Length; i++)
                {
                    Renderer rendInChild = interactInChild[i].gameObject.GetComponent<Renderer>();
                    if(rendInChild != null)
                    foreach (Material mat in rendInChild.materials)
                    {
                        mat.shader = shader2;
                    }
                }
            }
            else
            {
                foreach (Material mat in rend.materials)
                {
                    mat.shader = shader2;
                }
            }
        }

        /*if (door != null)
        {
            Renderer rend = door.gameObject.GetComponent<Renderer>();
            Shader shader2 = Shader.Find("Unlit/Color");
            foreach (Material mat in rend.materials)
            {
                mat.shader = shader2;
            }
        }*/
        //Debug.Log("OnPointerEnter" + hitObject.name);
    }


    //------------------ PickUp Systemp -----------------------
	void CheckDrop(){
		if (Input.GetKeyDown (KeyCode.F)) {
			DropObject ();
		}
	}

	public void DropObject(){
        if (carriedObject == null) return;
		carrying = false;
		carriedObject.GetComponent<Rigidbody> ().useGravity = true;
        carriedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        carriedObject.GetComponent<Rigidbody>().mass = tempMass;
        carriedObject.layer = tempLayer;
        carriedObject.GetComponent<Scr_Pickable>().Droped();

        Scr_Trap trap = carriedObject.GetComponentInChildren<Scr_Trap>();
        if (trap != null)
        {
            Collider[] colls = trap.gameObject.GetComponentsInChildren<Collider>();
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].isTrigger)
                    colls[i].enabled = true;
            }
        }

        carriedObject = null;
        tempLayer = LayerMask.NameToLayer("Default");
        tempMass = 0;
        objectDistance = 0;
    }
    //----------------- END PickUp Systemp -------------------------

    protected void TagForwardToPlayer(GameObject centerObj, GameObject target, float rotSpd, bool lockVertical)
    {
        Vector3 targetPos = target.transform.position;
        if (lockVertical) targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        Vector3 desiredDirection = targetPos - centerObj.transform.position;
        Vector3 rot = Vector3.RotateTowards(centerObj.transform.forward, desiredDirection, Mathf.Deg2Rad * rotSpd * Time.deltaTime, 1);
        centerObj.transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    public void HideTag()
    {
        tag.SetActive(false);
    }

    public void ShowTag()
    {
        tag.SetActive(true);
    }

    void TagOnHover(Vector3 targetPos)
    {
        tag.transform.position = targetPos + (transform.right.normalized * 0.4f) + (Vector3.forward.normalized * 0.2f) + (Vector3.up.normalized * 0.2f);
    }

    public bool IsCarrying()
    {
        return carrying;
    }
}

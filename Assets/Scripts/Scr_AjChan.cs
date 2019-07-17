using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scr_AjChan : MonoBehaviour {

    public int scaryLevel;
    public Transform currentNode;
    public int scary;
    public float speed;
    NavMeshAgent navMeshAgent;
    Animator anim;
    public bool action;
    public bool walking;
    public float turningTimer;
    private float turningTime = 2f;
    public bool afterMove;
    public bool onEvent;
    public Transform targetLookAt;
    int mask;
    private float signLenght = 15f;
    public GameObject raycastOrigin;
    private float seeTime;
    private float seenTime = 2f;

    //Alert
    public Transform tempNode;
    public bool alert;
    public bool afterAlert;

    //Item AjChan Need
    GameObject ajChanObject;
    bool book;
    bool pizza;

    Scr_AjChanZone zone;

    [Header("Node")]
    [SerializeField] Transform roomB201;
    [SerializeField] Transform frontOffice;
    [SerializeField] Transform office;
    [SerializeField] Transform toilet;
    [SerializeField] Transform storage;
    [SerializeField] Transform exit;

    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        zone = GetComponentInChildren<Scr_AjChanZone>();
        MoveToPosition(roomB201);
        mask = ~(1 << LayerMask.NameToLayer("Picking") | 1 << LayerMask.NameToLayer("IgnoreRaycast"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Scr_GameController.Instance.GameOver)
        {
            return;
        }

        if (IsInView())
        {
            /*seeTime += 1f;
            if (seeTime > seenTime)
            {*/
            navMeshAgent.isStopped = true;
            Scr_GameController.Instance.GameOverType = 0;
            Scr_GameController.Instance.GameOver = true;
            //}
        }
        if (zone.PlayerDetected && !action)
        {
            targetLookAt = Scr_Player.Instance.transform;
            anim.SetBool("beware", true);
            anim.SetTrigger("triggerAny");
            anim.speed = 3f;
            navMeshAgent.isStopped = true;
            action = true;
            zone.PlayerDetected = false;
        }
        /*else
        {
            if (seeTime != 0)
                seeTime = 0;
        }*/

        //Debug.Log("Distance : " + navMeshAgent.remainingDistance);
        if (!action && !alert)
        {
            if (ReachDestination())
            {
                AfterMove();
            }
        }

        if(action)
        {
            LookAtTarget(500f);
        }

        if (alert)
        {
            if (ReachDestination())
            {
                navMeshAgent.speed = 2;
                anim.speed = 1f;
                anim.SetBool("beware", true);
                anim.SetTrigger("triggerAny");
                afterAlert = true;
                alert = false;
            }
        }
        
        Event();
        if(onEvent)
            DelayEvent();

        if(afterAlert && afterMove && !walking)
        {
            if (tempNode != null)
                MoveToPosition(tempNode);
            else
                MoveToPosition(currentNode);
            afterMove = false;
            afterAlert = false;
        }
    }

    void MoveToPosition(Transform movePos)
    {
        if (movePos == null) return;

        RaycastHit hit;
        if (Physics.Raycast(movePos.transform.position, -movePos.transform.up, out hit))
        {
            currentNode = movePos;
            navMeshAgent.SetDestination(hit.point);
        }
        walking = true;
        anim.SetBool("walk", true);
        navMeshAgent.isStopped = false;
        afterMove = true;
        turningTimer = 0;
    }

    private bool ReachDestination()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            walking = false;
            anim.SetBool("walk", false);
            navMeshAgent.isStopped = true;
            return true;
        }
        return false;
    }

    private void AfterMove()
    {
        if (currentNode.GetComponent<Scr_Node>() == null) return;

        turningTimer += Time.deltaTime;
        if (turningTimer >= turningTime)
        {
            turningTimer = turningTime;
            afterMove = false;
        }

        if (afterMove)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, currentNode.GetComponent<Scr_Node>().GetSide(), turningTimer / turningTime);   
        }
    }

    private void TurnToTarget()
    {
        if (targetLookAt == null) return;

        turningTimer += Time.deltaTime;
        if (turningTimer >= turningTime)
        {
            turningTimer = turningTime;
        }

        Vector3 direction = transform.position - targetLookAt.position;

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, direction, turningTimer / turningTime);
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

    public void ResumeState()
    {
        if (Scr_GameController.Instance.GameOver) return;

        action = false;
        navMeshAgent.isStopped = false;

        if(walking)
            anim.SetBool("walk", true);

        turningTimer = 0;
        anim.speed = 1f;
        AfterAlert();
    }

    private void Event()
    {
        if (!afterMove && ReachDestination())
        {
            if (!onEvent)
            {
                onEvent = true;
                delay = 5f;
            }
            if(delay == 0)
            switch (currentNode.GetComponent<Scr_Node>().location)
            {
                case Scr_Node.nodeLocation.FrontRoomB201:
                        if (!book && ajChanObject != null)
                        {
                            ajChanObject.SetActive(false);
                            book = true;
                            onEvent = false;
                            MoveToPosition(office);
                        }
                    break;
                case Scr_Node.nodeLocation.Office:
                        if (!pizza && ajChanObject != null)
                        {
                            ajChanObject.SetActive(false);
                            pizza = true;
                            onEvent = false;
                            MoveToPosition(toilet);
                        }
                    break;
                case Scr_Node.nodeLocation.Toilet:
                    onEvent = false;
                    MoveToPosition(frontOffice);
                    break;
                case Scr_Node.nodeLocation.FrontOffice:
                    onEvent = false;
                    MoveToPosition(storage);
                    break;
                case Scr_Node.nodeLocation.Exit:
                        onEvent = false;
                        Scr_GameController.Instance.GameOverType = 1;
                        Scr_GameController.Instance.GameOver = true;
                        //gameObject.SetActive(false);
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.GetComponent<Scr_Door>() != null)
        {
            if (!coll.GetComponent<Scr_Door>().IsOpen())
            {
                coll.GetComponent<Scr_Door>().InteractDoor();
            }
        }

        if(coll.GetComponent<Scr_Trap>() != null && coll.GetComponent<Scr_Trap>().dmg > 0 && !coll.GetComponent<Scr_Trap>().Triggered)
        {
            if (!Scr_GameController.Instance.GameOver)
            {
                action = true;
                Scr_Trap trap = coll.GetComponent<Scr_Trap>();
                trap.Triggered = true;
                anim.SetBool("beware", !trap.veryscary);
                anim.SetBool("scary", trap.veryscary);
                anim.SetTrigger("triggerAny");
                navMeshAgent.isStopped = true;
                targetLookAt = coll.transform;
                Hit(trap);
            }
        }

        if (coll.GetComponent<Scr_ChanObject>() != null)
        {
            ajChanObject = coll.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.GetComponent<Scr_Door>() != null)
        {
            if (coll.GetComponent<Scr_Door>().IsOpen())
            {
                coll.GetComponent<Scr_Door>().InteractDoor();
            }
        }

        if (coll.GetComponent<Scr_ChanObject>() != null)
        {
            ajChanObject = null;
        }
    }

    float delay;
    void DelayEvent()
    {
        if (delay > 0) delay -= Time.deltaTime;
        else delay = 0;
    }

    private void Hit(Scr_Trap trap)
    {
        scaryLevel += trap.dmg;
    }

    private bool IsInView()
    {
        RaycastHit hit;
        /*
        Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + raycastOrigin.transform.forward * 15f, Color.red);
        //Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward - raycastOrigin.transform.right) * 15f, Color.red);
        //Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward + raycastOrigin.transform.right) * 15f, Color.red);
        Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward - (raycastOrigin.transform.right/2)) * 15f, Color.green);
        Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward + (raycastOrigin.transform.right/2)) * 15f, Color.green);
        //Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward - (raycastOrigin.transform.right / 4)) * 15f, Color.blue);
        //Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward + (raycastOrigin.transform.right / 4)) * 15f, Color.blue);
        Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward - (raycastOrigin.transform.right / 8)) * 15f, Color.yellow);
        Debug.DrawLine(raycastOrigin.transform.position, raycastOrigin.transform.position + (raycastOrigin.transform.forward + (raycastOrigin.transform.right / 8)) * 15f, Color.yellow);
        */
        int mask = ~(1 << LayerMask.NameToLayer("Picking") | 1 << LayerMask.NameToLayer("IgnoreRaycast"));
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward, out hit,signLenght,mask))
        {
            Debug.DrawLine(raycastOrigin.transform.position, hit.point, Color.red);

            if (hit.transform.CompareTag("Player"))
            {
                return CheckSeenPlayer(hit);
            }
        }
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward + (raycastOrigin.transform.right / 2), out hit, signLenght, mask))
        {
            Debug.DrawLine(raycastOrigin.transform.position, hit.point, Color.green);

            if (hit.transform.CompareTag("Player"))
            {
                return CheckSeenPlayer(hit);
            }
        }
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward - (raycastOrigin.transform.right / 2), out hit, signLenght, mask))
        {
            Debug.DrawLine(raycastOrigin.transform.position, hit.point, Color.green);

            if (hit.transform.CompareTag("Player"))
            {
                return CheckSeenPlayer(hit);
            }
        }
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward + (raycastOrigin.transform.right / 8), out hit, signLenght, mask))
        {
            Debug.DrawLine(raycastOrigin.transform.position, hit.point, Color.yellow);

            if (hit.transform.CompareTag("Player"))
            {
                return CheckSeenPlayer(hit);
            }
        }
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.forward - (raycastOrigin.transform.right / 8), out hit, signLenght, mask))
        {
            Debug.DrawLine(raycastOrigin.transform.position, hit.point, Color.yellow);
            
            if (hit.transform.CompareTag("Player"))
            {
                return CheckSeenPlayer(hit);
            }
        }
        return false;

    }

    private bool CheckSeenPlayer(RaycastHit hitPos)
    {
        if (Vector3.Distance(raycastOrigin.transform.position, hitPos.point) > 6f)
        {
            AlertTrigger(hitPos.transform);
        }
        else
            return true;
        return false;
    }

    private void AlertTrigger(Transform target)
    {
        if (!alert && !afterAlert)
        {
            tempNode = currentNode;
            MoveToPosition(target);
            navMeshAgent.speed = 4;
            anim.speed = 2f;
            alert = true;
            Debug.Log("Alerted");
        }
    }

    public void AfterAlert()
    {
        if (afterAlert)
        {
            MoveToPosition(tempNode);
            afterAlert = false;
        }
    }
    
}

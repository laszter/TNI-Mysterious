using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Scr_CharactorController : MonoBehaviour {

    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;

    [SerializeField] private AudioClip[] m_FootstepSounds;    
    [SerializeField] private AudioClip m_JumpSound;       
    [SerializeField] private AudioClip m_LandSound;

    private bool m_Jump;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_PreviouslyGrounded;
    private bool m_Jumping;
    private AudioSource m_AudioSource;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Debug.Log(horizontalMovement + " " + verticalMovement);

        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
    }

    private void Move()
    {
        Vector3 yVelFix = new Vector3(0, rb.velocity.y, 0);
        rb.velocity = moveDirection * m_WalkSpeed * Time.deltaTime;
        rb.velocity += yVelFix;
    }
}

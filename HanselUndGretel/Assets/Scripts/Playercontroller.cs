using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public static Playercontroller Instance { get { return instance; } }
    private static Playercontroller instance;

    public float m_RunSpeed = 10.0f;
    public float m_WalkSpeed = 5.0f;
    public float m_RotationSpeed = 15.0f;

    [SerializeField]
    public float m_DistanceDragBack = 2.5f;

    public float ReduceStress { get { return m_reduceStress; } }
    [SerializeField, Range(-5,0)]
    private float m_reduceStress = -3.0f;

    private EPlayerState m_currentState;
    public EPlayerState CurrentState { get { return m_currentState; } }

    Rigidbody m_rigid;

    private CompanionAI m_ai;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        Cursor.lockState = CursorLockMode.Locked;

        m_currentState = EPlayerState.Freehanded;
        m_rigid = GetComponent<Rigidbody>();

        m_ai = FindObjectOfType<CompanionAI>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if(Input.GetKeyDown(KeyCode.F)) //Call Companion
        {
            if (m_currentState == EPlayerState.Freehanded) //Follow
            {
                if (Vector3.Distance(m_ai.transform.position, transform.position) <= m_DistanceDragBack)
                {
                    m_ai.Call_FollowMe();
                    m_currentState = EPlayerState.HoldingHand;
                }
                else
                {
                    //ToDo: Too far away!
                }
            }
            else if(m_currentState == EPlayerState.HoldingHand) //Wait
            {
                m_ai.Call_WaitHere();
                m_currentState = EPlayerState.Freehanded;
            }
        }
    }

    void Movement()
    {
        if (IsGrounded())
        {
            if (m_currentState == EPlayerState.HoldingHand && Vector3.Distance(m_ai.transform.position, transform.position) > m_DistanceDragBack)
            {
                //DragBack
                Vector3 dir = (m_ai.transform.position - transform.position).normalized;

                m_rigid.velocity = dir * 1.5f;
            }
            else
            {
                //Move
                Vector3 mov = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
                mov *= m_currentState == EPlayerState.Freehanded ? m_RunSpeed : m_WalkSpeed;

                m_rigid.velocity = mov;
            }
        }

        //Look
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * m_RotationSpeed * Time.deltaTime);
        //m_rigid.angularVelocity = Vector3.up * Input.GetAxis("Mouse X") * m_RotationSpeed * Time.deltaTime;
    }

    bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1.1f);

        return Physics.Raycast(ray, 1.1f); //7
    }

    public enum EPlayerState
    {
        Freehanded,
        HoldingHand,
        HoldingIten,
    }
}

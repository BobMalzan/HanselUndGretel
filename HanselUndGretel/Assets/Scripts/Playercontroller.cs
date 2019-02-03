using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float m_RunSpeed = 10.0f;
    public float m_WalkSpeed = 5.0f;
    public float m_RotationSpeed = 15.0f;

    public Light m_FriendlySpotLight;

    [SerializeField]
    public float m_DistanceDragBack = 2.5f;

    public float ReduceStress { get { return m_reduceStress; } }
    [SerializeField, Range(-5,0)]
    private float m_reduceStress = -3.0f;

    private EPlayerState m_currentState;
    public EPlayerState CurrentState { get { return m_currentState; } }

    Rigidbody m_rigid;
    public Transform m_HoldingBone;

    public AItem CurrentItem { get { return m_currentItem; } }
    private AItem m_currentItem;

    //Animation
    Animator m_Animator;
    int m_aniMovID;
    int m_PickupID;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_currentState = EPlayerState.Freehanded;
        m_rigid = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_aniMovID = Animator.StringToHash("MoveSpeed");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetMouseButtonDown(0)) //Call Companion
        {
            if (m_currentState == EPlayerState.Freehanded) //Follow
            {
                if (Vector3.Distance(ScoreKeeper.Get.Companion.transform.position, transform.position) <= m_DistanceDragBack)
                {
                    ScoreKeeper.Get.Companion.Call_FollowMe();
                    m_currentState = EPlayerState.HoldingHand;
                    m_FriendlySpotLight.enabled = true;
                }
                else
                {
                    //ToDo: Too far away!
                }
            }
            else if (m_currentState == EPlayerState.HoldingHand) //Wait
            {
                ScoreKeeper.Get.Companion.Call_WaitHere();
                m_currentState = EPlayerState.Freehanded;
                m_FriendlySpotLight.enabled = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(m_currentItem != null)//Drop
            {
                m_currentItem.Drop();
                m_currentItem.transform.parent = transform.parent;
                m_currentItem = null;
                m_currentState = EPlayerState.Freehanded;
            }
            else if(CurrentState == EPlayerState.Freehanded)
            {
                m_currentItem = TryPickUp();
                if(m_currentItem != null)
                {
                    m_currentState = EPlayerState.HoldingIten;
                    m_currentItem.PickUp();

                    m_currentItem.transform.parent = m_HoldingBone;
                    m_currentItem.transform.localPosition = m_currentItem.m_holdingPosOffset();
                    m_currentItem.transform.localRotation = Quaternion.Euler(m_currentItem.m_holdingRotOffset());
                }
            }
        }
    }

    private AItem TryPickUp()
    {
        RaycastHit[] m_RaycastHit = Physics.SphereCastAll(transform.position, 2.0f, Vector3.down, 0.0f);

        AItem nearestItem = null;

        foreach(RaycastHit _hit in m_RaycastHit)
        {
            AItem tmp = _hit.transform.gameObject.GetComponent<AItem>();

            if(tmp != null)
            {
                if(nearestItem == null) { nearestItem = tmp; }
                else
                {
                    float distance1 = Vector3.Distance(transform.position, tmp.transform.position);
                    float distance2 = Vector3.Distance(transform.position, nearestItem.transform.position);

                    if(distance2 < distance1) { nearestItem = tmp; }
                }
            }
        }

        return nearestItem;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (m_currentState == EPlayerState.HoldingHand)
        {
            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            m_Animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.Lerp(transform.position, ScoreKeeper.Get.Companion.transform.position, 0.5f) + Vector3.up * 0.5f);
        }
    }

    void Movement()
    {
        if (IsGrounded())
        {
            if (m_currentState == EPlayerState.HoldingHand && Vector3.Distance(ScoreKeeper.Get.Companion.transform.position, transform.position) > m_DistanceDragBack)
            {
                m_Animator.SetFloat(m_aniMovID, 1.0f);

                //DragBack
                Vector3 dir = (ScoreKeeper.Get.Companion.transform.position - transform.position).normalized;
                m_rigid.velocity = dir * 1.5f;
            }
            else
            {
                //Move
                Vector3 mov = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
                mov.Normalize();
                mov *= m_currentState == EPlayerState.Freehanded ? m_RunSpeed : m_WalkSpeed;

                if(mov != Vector3.zero)
                {
                    if(Input.GetAxis("Vertical") < 0)
                    {
                        m_Animator.SetFloat(m_aniMovID, -0.66f);
                    }
                    else
                    {
                        m_Animator.SetFloat(m_aniMovID, 1.0f);
                    }
                }
                else
                {
                    m_Animator.SetFloat(m_aniMovID, 0.0f);
                }

                m_rigid.velocity = mov;
            }
        }

        //Look
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * m_RotationSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * .08f);

        return Physics.Raycast(ray, .08f); //7
    }

    public enum EPlayerState
    {
        Freehanded,
        HoldingHand,
        HoldingIten,
    }
}

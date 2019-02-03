using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAI : MonoBehaviour
{
    [SerializeField]
    float m_stressMax = 100.0f;
    [SerializeField]
    float m_followingDistance = 1.0f;
    [SerializeField]
    float m_StressLimit4Resist = 5.0f;
    [SerializeField, Range(-5,0)]
    float m_CalmDownOutsideStressZone = -2.0f;

    Rigidbody m_rigid;

    //Animation
    Animator m_Animator;
    int m_aniMovID;
    int m_aniJumpID;
    int m_aniGroundedID;

    float m_currentStress;

    ECompanionState m_currentState;
    public ECompanionState State { get { return m_currentState; } }
StressZone m_currentStressZone;

    private void Awake()
    {
        Playercontroller[] ctrs = FindObjectsOfType<Playercontroller>();
        m_rigid = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        m_aniMovID = Animator.StringToHash("MoveSpeed");
        m_aniJumpID = Animator.StringToHash("Jump");
        m_aniGroundedID = Animator.StringToHash("Grounded");
    }

    public void SetStressZone(StressZone _zone)
    {
        if(m_currentStressZone != null)
        {
            Debug.LogWarning("Hey! There was overlaping stresszones!... Please srink one!");
        }
        m_currentStressZone = _zone;

        Debug.Log("EnterStressZone!");
    }

    public void RemoveStressZone(StressZone _zone)
    {
        if(m_currentStressZone != _zone)
        {
            Debug.LogWarning("Hey! You exit a stresszone but you are still in a other!");
        }
        else
        {
            m_currentStressZone = null;
        }

        Debug.Log("ExitStressZone!");
    }

    // Update is called once per frame
    void Update()
    {
        BehaviorUpdate();
        UpdateStress();

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(m_currentState == ECompanionState.Follow)
        {
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.Lerp(transform.position, ScoreKeeper.Get.Player.transform.position, 0.5f) + Vector3.up * 0.5f);
        }
    }

    private void BehaviorUpdate()
    {
        switch (m_currentState)
        {
            case ECompanionState.Waiting:
                Vector3 lookAtPos = ScoreKeeper.Get.Player.transform.position;
                lookAtPos.y = transform.position.y;
                transform.LookAt(lookAtPos);
                break;
            case ECompanionState.Follow:
                Vector3 lookAtPos2 = ScoreKeeper.Get.Player.transform.position;
                lookAtPos2.y = transform.position.y;
                transform.LookAt(lookAtPos2);

                float distance = Vector3.Distance(ScoreKeeper.Get.Player.transform.position, transform.position);

                if (distance > m_followingDistance)
                {
                    m_Animator.SetFloat(m_aniMovID, 1.0f);

                    Vector3 dir = (lookAtPos2 - transform.position).normalized;
                    float slowDown = (distance - m_followingDistance) > 0.01f ? -0.5f : (distance - m_followingDistance);
                    m_rigid.MovePosition(transform.position + dir * (ScoreKeeper.Get.Player.m_WalkSpeed - slowDown) * Time.deltaTime);
                }
                else
                {
                    m_Animator.SetFloat(m_aniMovID, 0.0f);
                }
                break;
            case ECompanionState.Panic:
                transform.Rotate(new Vector3(1,1) * Time.deltaTime * 200.0f); //hehehe
                break;
            case ECompanionState.Resist:
                transform.Rotate(Vector3.up * Time.deltaTime * 100.0f); //hehehe
                break;
            default:
                break;
        }
    }

    public void Call_FollowMe()
    {
        ChangeState(ECompanionState.Follow);
    }

    public void Call_WaitHere()
    {
        ChangeState(ECompanionState.Waiting);
    }

    private void UpdateStress()
    {
        float StressGain = 0;

        if(m_currentStressZone == null)
        {
            StressGain += m_CalmDownOutsideStressZone;
        }
        else
        {
            StressGain += m_currentStressZone.CalcStressInZone(m_currentState == ECompanionState.Follow || m_currentState == ECompanionState.Resist ? ScoreKeeper.Get.Player.transform.position : transform.position);
        }

        if(m_currentState == ECompanionState.Follow)
        {
            StressGain += ScoreKeeper.Get.Player.ReduceStress;
        }
        
        if(m_currentState == ECompanionState.Follow && StressGain >= m_StressLimit4Resist)
        {
            ChangeState(ECompanionState.Resist);
        }
        else if(m_currentState == ECompanionState.Resist && StressGain<= m_StressLimit4Resist)
        {
            ChangeState(ECompanionState.Follow);
        }

//         DebugCanvas.Instance.ShowStressPegel(StressGain);
//         DebugCanvas.Instance.ShowStress(m_currentStress);

        m_currentStress += StressGain * Time.deltaTime;
        if(m_currentStress < 0.0f) { m_currentStress = 0.0f; }

        if (m_currentStress >= m_stressMax)
        {
            ChangeState(ECompanionState.Panic);
        }
    }

    void ChangeState(ECompanionState _state)
    {
        m_Animator.SetFloat(m_aniMovID, 0.0f);

        switch (_state)
        {
            case ECompanionState.Waiting:
                break;
            case ECompanionState.Follow:
                break;
            case ECompanionState.Panic:
                Debug.Log("COMPANION: Panic!");
                //GameOver OR Flee
                break;
            case ECompanionState.Resist:
                Debug.Log("COMPANION: Resist!");
                break;
            default:
                break;
        }

        m_currentState = _state;
    }

    public enum ECompanionState
    {
        Waiting,
        Follow,
        Panic,
        Resist,
    }
}

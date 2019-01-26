using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressZone : MonoBehaviour
{
    [Range(-5,10)]
    public float StressLevel;
    public AnimationCurve StressIncrease;

    private float Distance;

    private void Awake()
    {
        Vector3 size = GetComponent<BoxCollider>().size;
        size.y = 0;

        Distance = size.magnitude / 2.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        CompanionAI ai = other.GetComponent<CompanionAI>();

        if(ai != null)
        {
            ai.SetStressZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CompanionAI ai = other.GetComponent<CompanionAI>();

        if (ai != null)
        {
            ai.RemoveStressZone(this);
        }
    }

    public float CalcStressInZone(Vector3 _pos)
    {
        Vector3 pos1 = _pos;
        Vector3 pos2 = transform.position;
        pos1.y = 0;
        pos2.y = 0;

        float value = Vector3.Distance(pos1, pos2) / Distance;
        value = 1.0f - value;

        value = Mathf.Clamp01(value);

        Debug.Log("Value:" + value);

        return StressIncrease.Evaluate(value) * StressLevel;
    }
}

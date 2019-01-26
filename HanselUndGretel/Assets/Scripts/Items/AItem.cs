using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AItem : MonoBehaviour
{
    Rigidbody m_rigid;
    Collider m_collider;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    public abstract Vector3 m_holdingPosOffset();
    public abstract Vector3 m_holdingRotOffset();

    public virtual void PickUp()
    {
        m_rigid.isKinematic = true;
        m_collider.enabled = false;
    }

    public virtual void Drop()
    {
        m_rigid.isKinematic = false;
        m_collider.enabled = true;
    }
}

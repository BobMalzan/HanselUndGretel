using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : AReactor
{
    public AnimationClip m_GateOpen;
    public AnimationClip m_GateClose;
    private Animation m_animator;

    public override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animation>();
    }

    public override string HintText()
    {
        return "A closed door!";
    }

    public override string SucessText()
    {
        return "A open door!";
    }

    protected override void Act(bool _active)
    {
        //Do door open! Play animation AND/OR remove collision
        m_animator.clip = _active ? m_GateOpen : m_GateClose;
        m_animator.Play();
    }
}

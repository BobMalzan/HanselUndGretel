using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : AActor
{
    public bool m_SingleUse;
    private bool isUsed;
    private Animation m_animator;

    public AnimationClip m_OnAnimation;
    public AnimationClip m_OffAnimation;

    public new void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animation>();
    }

    public override string HintText()
    {
        return isUsed ? "The Switch is broken!" : "You need free hands for use!";
    }

    public override bool Interact()
    {
        if(Playercontroller.Instance.CurrentState != Playercontroller.EPlayerState.Freehanded) { return false; }

        if(m_SingleUse)
        {
            if(isUsed)
            {
                return false;
            }
            isUsed = true;
        }

        m_animator.clip = !IsAktive ? m_OnAnimation : m_OffAnimation;
        m_animator.Play();

        IsAktive = !IsAktive;
        return true;
    }

    public override bool InteractWith(AItem _item)
    {
        return false;
    }

    public override string SucessText()
    {
        return "You flip the switch " + (IsAktive ? "ON!" : "OFF!") + (m_SingleUse ? "...But the switch broke!" : "");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AActor : AReactor
{
    private bool m_Useable;
    protected override void Act(bool _active)
    {
        m_Useable = _active;
    }

    public bool IsAktive
    {
        get { return m_IsAktive; }
        protected set
        {
            if(value != m_IsAktive)
            {
                m_IsAktive = value;
                AReactor.StateChange();
            }
        }
    }

    private bool m_IsAktive;

    abstract public bool Interact();
    abstract public bool InteractWith(AItem _item);
}

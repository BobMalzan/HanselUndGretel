using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : AActor
{
    public StressZone m_ManipulateStressZone;
    [Range(-5,5)]
    public float m_StressChange;
    public Light m_light;

    public ParticleSystem[] m_Effects;

    public override string HintText()
    {
        return "It needs a burning torch to flame up!";
    }

    public override bool Interact()
    {
        return false;
    }

    public override bool InteractWith(AItem _item)
    {
        if(_item is Torch)
        {
            m_ManipulateStressZone.StressLevel = m_StressChange;

            foreach (ParticleSystem eff in m_Effects)
            {
                eff.Play();
            }

            m_light.enabled = true;
            return true;
        }
        return false;
    }

    public override string SucessText()
    {
        return "The campfire starts burning!";
    }

    protected override void Act(bool _active)
    {
    }
}

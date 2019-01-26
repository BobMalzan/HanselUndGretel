using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AReactor : MonoBehaviour
{
    //Playerfeedback
    abstract public string SucessText();
    abstract public string HintText();

    //Logic
    private bool usedStatus = false;
    public AActor[] LinkedActors;
    abstract protected void Act(bool _active);

    static private List<AReactor> allReactors;

    //Management
    public bool RequestForAct()
    {
        if (allReactors != null)
        {
            for (int i = 0; i < LinkedActors.Length; i++)
            {
                if (!LinkedActors[i].IsAktive)
                {
                    if(usedStatus)
                    {
                        Act(false);
                        usedStatus = false;
                    }
                    return false;
                }
            }
        }

        if (!usedStatus)
        {
            Act(true);
            usedStatus = true;
        }
        return true;
    }

    static public void StateChange()
    {
        foreach(AReactor reactor in allReactors)
        {
            reactor.RequestForAct();
        }
    }

    public virtual void Awake()
    {
        if(allReactors == null)
        {
            allReactors = new List<AReactor>();
        }

        allReactors.Add(this);
    }
}

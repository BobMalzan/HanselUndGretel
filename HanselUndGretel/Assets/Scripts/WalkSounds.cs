using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSounds : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string soundWalk;
    [FMODUnity.EventRef]
    public string soundRun;

    FMOD.Studio.EventInstance m_WalkEventInstance;
    FMOD.Studio.EventInstance m_RunEventInstance;

    private void Start()
    {
        m_WalkEventInstance = FMODUnity.RuntimeManager.CreateInstance(soundWalk);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_WalkEventInstance, transform, GetComponent<Rigidbody>());

        m_RunEventInstance = FMODUnity.RuntimeManager.CreateInstance(soundRun);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_RunEventInstance, transform, GetComponent<Rigidbody>());
    }

    public void Step()
    {
        m_WalkEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_WalkEventInstance.start();
    }

    public void RunStep()
    {
        m_RunEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_RunEventInstance.start();
    }
}

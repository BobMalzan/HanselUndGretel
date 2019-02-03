using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    public bool m_PlayerIsHans = true;
    public static ScoreKeeper Get { get; protected set; }

    public bool m_IsStartScreen = false;

    public Vector3 m_LastSafePlace;
    public Vector3 m_GoalPosition;

    public GameObject m_Hansel;
    public GameObject m_Gretel;

    [FMODUnity.EventRef]
    public string m_SoundMenuClick;
    FMOD.Studio.EventInstance m_MenuClickEventInstance;

    [FMODUnity.EventRef]
    public string m_SoundMenuBG;
    FMOD.Studio.EventInstance m_MenuBGEventInstance;

    GameObject m_Player;
    GameObject m_Companion;

    public Playercontroller Player { get; private set; }
    public CompanionAI Companion { get; private set; }

    SpeechBubble m_SpeechBubble;
    bool m_ToldAboutHoldHands = false;

    string m_CurrentHint = "I Wanna go HOME...";
    float m_HintTimer = 3.0f;

    // am, Anfang, haendehalten und linke maustaste halten blinken
    // beim Tor, sprite lever anzeigen, dann ctrl_button
    // an der EAR stelle ear icon und maus_rechts blinken
    // bei Angst, blinkenden heartbeat anzeigen
    // tipp bei erster dangerzone: feuer an/aus
    // bei naehe zu feuerstelle kalt, fuer an/aus blinken, falls feuer an, nur feuer an anzeigen
    // im Dangerzone 1 und 2, blinkende torch und nofire (torch lites fire 1 bild)
    // bei naehe zu fackel, space_button fackel blinken
    // in wolf-dangerzone, wolf und Herz blinken
    // wenn fackel in der Hand, haende halten geht nicht: "blank_taste" und nohands blinken

    // haende halten verhindert immer aktion, also bubble hinweis, dass loslassen erforderlich
    // funktion rechte maustaste: soundtrack aktivieren (schon implementiert?) 
    // gesamte letzte dangerzone mit Wolfsgeheul


    // SOUNDS ========================================================================
    // Hand loslassen: stress einspielen
    // ambient immer spielen ausser wenn Ziel erreicht
    // music spielen, wenn ear aktiv oder Ziel erreicht
    // fackel knistert "torch"
    // dangerzone4: wolves
    // sonst scary


    void Awake()
    {
        if (Get != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        Get = this;
        SceneManager.sceneLoaded += LevelWasLoaded;
        m_MenuBGEventInstance = FMODUnity.RuntimeManager.CreateInstance(m_SoundMenuBG);
        m_MenuClickEventInstance = FMODUnity.RuntimeManager.CreateInstance(m_SoundMenuClick);
    }

    public void SelectCharacter(bool _playerIsHans)
    {
        m_PlayerIsHans = _playerIsHans;
    }

    public void RegisterSafePlace(Vector3 _safePlace)
    {
        m_LastSafePlace = _safePlace;
    }

    private void LevelWasLoaded(Scene _scene, LoadSceneMode _mode)
    {
        m_Hansel = GameObject.Find("Hansel");
        m_Gretel = GameObject.Find("Gretel");

        if (m_IsStartScreen)
        {
            m_MenuBGEventInstance.start();
            return;
        }

        m_MenuBGEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if (m_PlayerIsHans)
        {
            m_Player = m_Hansel;
            m_Companion = m_Gretel;
        }
        else
        {
            m_Player = m_Gretel;
            m_Companion = m_Hansel;
        }

        m_Player.GetComponent<CompanionAI>().enabled = false;
        m_Player.GetComponentInChildren<Camera>().enabled = true;
        Player = m_Player.GetComponent<Playercontroller>();
        Player.enabled = true;

        m_Companion.GetComponent<Playercontroller>().enabled = false;
        m_Companion.GetComponentInChildren<Camera>().enabled = false;
        Companion = m_Companion.GetComponent<CompanionAI>();
        Companion.enabled = true;

        Terrain t = FindObjectOfType<Terrain>();
        Transform goal = t.transform.Find("Goal");
        Transform start = t.transform.Find("Start");
        if (goal == null || start == null)
        {
            Debug.LogError("you forgot to define 'goal' and 'start' as positions in your terrain children");
        }
        else
        {
            m_LastSafePlace = start.position;
            m_GoalPosition = goal.position;
            GoToSafePlace(m_LastSafePlace);
        }

        m_SpeechBubble = FindObjectOfType<SpeechBubble>();
        if (!m_ToldAboutHoldHands)
        {
            m_ToldAboutHoldHands = true;
            m_SpeechBubble.SetBubble(SpeechBubble.Icon.NotHands, SpeechBubble.Icon.hands, m_Companion.transform);
        }
    }

    public void SetBubble(bool _playerSpeaks, SpeechBubble.Icon _firstIcon, SpeechBubble.Icon _secondIcon)
    {
        m_SpeechBubble.SetBubble(_firstIcon, _secondIcon, _playerSpeaks ? Player.transform :  Companion.transform);
    }

    private void Update()
    {
        if (m_IsStartScreen)
            return;

        if (m_HintTimer > 0)
        {
            m_HintTimer -= Time.deltaTime;
            if (m_HintTimer < 0)
            {
                if (Vector3.Distance(Player.transform.position, Companion.transform.position) < 25.0f && Companion.State == CompanionAI.ECompanionState.Waiting)
                {
                    m_SpeechBubble.SetBubble(SpeechBubble.Icon.hands, SpeechBubble.Icon.NotHands, m_Companion.transform);
                }
                m_HintTimer = 20.0f;
            }
        }
    }

    void GoToSafePlace(Vector3 _safePlace)
    {
        if (_safePlace == null)
            _safePlace = m_LastSafePlace;

        m_Player.transform.position = _safePlace;
        m_Companion.transform.position = _safePlace + m_Hansel.transform.right;
    }

    public void ReachSafePosition(Vector3 _pos)
    {
        m_LastSafePlace = _pos;
    }

    public void PlayClickSound()
    {
        m_MenuClickEventInstance.start();
    }
}

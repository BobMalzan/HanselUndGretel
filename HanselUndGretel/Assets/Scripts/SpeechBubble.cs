using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Image m_SymbolObject;
    public Image m_ImageObject;

    public enum Icon
    {
        none,
        ctrl_button,
        ear,
        EVILwolf,
        fireplaceOFF,
        fireplaceON,
        hands,
        heartbeat,
        left_mousebutton,
        lever,
        LightFireplace,
        LightTorch,
        NotHands,
        path,
        right_mousebutton,
        space_button_tagged,
        torch,
        torchNoFire
    }

    public Sprite[] m_Sprites;

    Icon m_IconOne;
    Icon m_IconTwo;

    Transform m_BubbleTransform;
    private float m_Timer;
    private float m_Blinker;

    private void Start()
    {
        m_ImageObject.enabled = false;
        m_SymbolObject.enabled = false;
    }

    public void Update()
    {
        UpdateBubble();
    }

    public void SetBubble(Icon _blinkOne, Icon _blinkTwo, Transform _transform)
    {
        m_BubbleTransform = _transform;

        m_IconOne = _blinkOne;
        m_IconTwo = _blinkTwo;

        bool on = _blinkOne != Icon.none;
        m_SymbolObject.enabled = on;
        m_ImageObject.enabled = on;
        if (on)
        {
            m_Timer = 3.0f;
        }
    }

    private Sprite Icon2Sprite(Icon _icon)
    {
        if (_icon == Icon.none)
            return null;

        return m_Sprites[(int)_icon - 1];
    }

    private void UpdateBubble()
    {
        m_Blinker += Time.deltaTime;

        if (m_Timer > 0.0f)
        {
            m_Timer -= Time.deltaTime;

            bool one = (int)(m_Blinker * 1000) % 1000 > 500;
            Debug.Log("one=" + one.ToString());

            Vector3 pos = Camera.main.WorldToScreenPoint(m_BubbleTransform.position + 1.5f * Vector3.up);
            m_SymbolObject.rectTransform.position = pos + new Vector3(60, 20, 0);
            m_ImageObject.rectTransform.position = pos;
            m_SymbolObject.sprite = Icon2Sprite(one ? m_IconOne : m_IconTwo);
            Debug.Log("Sprite name=" + m_SymbolObject.sprite.name);
            if (m_Timer <= 0.0f)
            {
                m_SymbolObject.enabled = false;
                m_ImageObject.enabled = false;
                m_Timer = 0;
            }
        }
    }
}

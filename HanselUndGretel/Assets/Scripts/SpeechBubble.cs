using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Text m_TextObject;
    public Image m_ImageObject;

    private float m_Timer;

    private void Start()
    {
        m_ImageObject.enabled = false;
        m_TextObject.enabled = false;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            string playerName = ScoreKeeper.Get.m_PlayerIsHans ? "Hänsel: " : "Gretel: ";

            SetBubble(playerName + "Hallo, das ist ein testtext!\nUnd noch ne Zeile", Input.mousePosition);
        }

        UpdateBubble();
    }

    private void SetBubble(string _text, Vector3 _screenPos)
    {
        Vector2 boxSize = m_TextObject.rectTransform.sizeDelta * 0.1f;
        Vector3 box = new Vector3(boxSize.x, boxSize.y, 0);

        m_TextObject.text = _text;
        m_TextObject.rectTransform.position = _screenPos;
        m_ImageObject.rectTransform.position = _screenPos - box;
        
        m_ImageObject.rectTransform.sizeDelta = m_TextObject.rectTransform.sizeDelta * 1.2f;
        m_TextObject.enabled = true;
        m_ImageObject.enabled = true;
        m_Timer = 3.0f;
    }

    private void UpdateBubble()
    {
        if (m_Timer > 0.0f)
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0.0f)
            {
                m_TextObject.enabled = false;
                m_ImageObject.enabled = false;
                m_Timer = 0;
            }
        }
    }
}

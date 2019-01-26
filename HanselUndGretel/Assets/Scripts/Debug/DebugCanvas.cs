using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    static public DebugCanvas Instance;
    public Text m_StressText;
    public Text m_StressPegel;
    public Text m_HintText;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void ShowStress(float _stress)
    {
        m_StressText.text = "StressLevel: " + _stress;
    }

    public void ShowHint(string _stress)
    {
        StartCoroutine(HintFade(_stress));
    }

    IEnumerator HintFade(string _text,float _duration = 5.0f)
    {
        m_HintText.gameObject.SetActive(true);
        m_HintText.text = _text;

        yield return new WaitForSecondsRealtime(_duration);

        m_HintText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void ShowStressPegel(float _stress)
    {
        m_StressPegel.text = "StressPegel: " + _stress;
    }
}

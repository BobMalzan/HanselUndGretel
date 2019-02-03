using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScreen : MonoBehaviour
{
    float m_Timer = 5;

    void Update()
    {
        if (m_Timer > 0)
        {
            m_Timer -= Time.deltaTime;
        }

        if (m_Timer <= 0)
        {
            SceneManager.LoadScene("StartScreen");
            ScoreKeeper.Get.m_IsStartScreen = true;
        }
    }
}

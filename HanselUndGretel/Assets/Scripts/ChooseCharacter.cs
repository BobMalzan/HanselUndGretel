using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharacter : MonoBehaviour
{
    public Light m_HanselLight;
    public Light m_GretelLight;

    public void OnSelectHans()
    {
        MenuClickSound();
        ScoreKeeper.Get.SelectCharacter(true);
        m_HanselLight.enabled = true;
        m_GretelLight.enabled = false;
    }

    public void OnSelectGretel()
    {
        MenuClickSound();
        ScoreKeeper.Get.SelectCharacter(false);
        m_HanselLight.enabled = false;
        m_GretelLight.enabled = true;
    }

    public void MenuPlay()
    {
        MenuClickSound();
        ScoreKeeper.Get.m_IsStartScreen = false;
        SceneManager.LoadScene("GameSceneSebastian");
    }

    public void MenuStaff()
    {
        MenuClickSound();
        SceneManager.LoadScene("StaffScene");
    }

    public void MenuExit()
    {
        MenuClickSound();
        Application.Quit();
    }

    void MenuClickSound()
    {
        ScoreKeeper.Get.PlayClickSound();
    }
}

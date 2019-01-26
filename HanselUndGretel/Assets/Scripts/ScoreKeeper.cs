using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    public bool m_PlayerIsHans = true;
    public static ScoreKeeper Get { get; protected set; }

    void Awake()
    {
        if (Get != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        Get = this;


    }

    public void SelectCharacter(bool _playerIsHans)
    {
        m_PlayerIsHans = _playerIsHans;
        SceneManager.LoadScene("GameScene");
    }
}

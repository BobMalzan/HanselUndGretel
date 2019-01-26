using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacter : MonoBehaviour
{
    public void OnSelectHans()
    {
        ScoreKeeper.Get.SelectCharacter(true);
    }

    public void OnSelectGretel()
    {
        ScoreKeeper.Get.SelectCharacter(false);
    }
}

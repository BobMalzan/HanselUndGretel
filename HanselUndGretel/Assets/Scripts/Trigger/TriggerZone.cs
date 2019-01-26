using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public AActor linkedActor;
    private bool playerInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Playercontroller.Instance.gameObject)
        {
            Debug.Log("PLAYER: HasEnterTrigger");
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Playercontroller.Instance.gameObject)
        {
            Debug.Log("Something(PleaseFIX) HasExitTrigger");
            playerInTrigger = false;
        }
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Press E");
                if (linkedActor.Interact())
                {
                    DebugCanvas.Instance.ShowHint(linkedActor.SucessText());
                }
                else
                {
                    DebugCanvas.Instance.ShowHint(linkedActor.HintText());
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Press R");
                DebugCanvas.Instance.ShowHint(linkedActor.HintText());
            }
        }
    }
    //hmmm aktiviere den actor? wenn du in der triggerzone stehst und eine Taste drückst...
    //Kann auch Info Text anzeigen auf bedarf...
    //Zeigt am besten auch die benutzbaren tasten an...
    //E = Interagieren
    //R = Für Hinweiß/Hint
}

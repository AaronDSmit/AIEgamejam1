using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositZone : MonoBehaviour
{
    [SerializeField]
    private PlayerController currentPlayer = null;

    private void OnTriggerEnter(Collider other)
    {
        if (currentPlayer == null)
        {
            PlayerController contact = other.transform.GetComponent<PlayerController>();

            if (contact != null)
            {
                currentPlayer = contact;

                Manager.instance.DisplayDockPrompt(true);

                currentPlayer.CanDock(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentPlayer != null)
        {
            currentPlayer.CanDock(false);
            currentPlayer = null;
            Manager.instance.DisplayDockPrompt(false);
        }
    }
}
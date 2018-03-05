using UnityEngine;

public class DepositZone : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much rubbish will be dropped off each second")]
    private float drainRate;

    private Manager manager;

    private Player currentPlayer = null;

    private bool canDock = true;

    private void Awake()
    {
        manager = GameObject.FindObjectOfType<Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        manager.DisplayDockPrompt(true);

        if (canDock)
            return;

        if (currentPlayer == null)
        {
            Player contact = other.transform.GetComponent<Player>();

            if (contact != null)
            {
                currentPlayer = contact;
                manager.AddScore(true, 5);
                // contact.getRubbish() 
                //contact.EmptyRubbish();
            }
        }
        else
        {
            currentPlayer = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentPlayer != null)
        {
            currentPlayer = null;
        }
    }

    private void DrainRubbish()
    {
        if (currentPlayer)
        {
            //currentPlayer.RemoveRubbish();
        }
    }
}
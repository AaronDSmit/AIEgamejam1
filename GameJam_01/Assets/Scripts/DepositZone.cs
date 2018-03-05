using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositZone : MonoBehaviour
{
    private Manager manager;

    private void Awake()
    {
        manager = GameObject.FindObjectOfType<Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player contact = other.getComponent<Player>();

        //if (contact != null)
        //{
        //    manager.AddScore(true, contact.getRubbish());
        //    contact.EmptyRubbish();
        //}
    }
}
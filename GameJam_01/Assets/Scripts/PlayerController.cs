using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int maxRubbish;

    [SerializeField]
    private float dockTime;

    private int currentRubbish;

    private Player movement;

    private bool canDock = false;

    private Manager manager;

    private void Awake()
    {
        manager = GameObject.FindObjectOfType<Manager>();
    }

    public void Reset()
    {
        canDock = false;
    }

    private void Update()
    {
        if (canDock && Input.GetKeyDown(KeyCode.A))
        {
            // movement.setMove(false);
            canDock = false;
            manager.DisplayDockPrompt(false);

            Invoke("ReleaseDock", dockTime);
        }
    }

    private void ReleaseDock()
    {
        // movement.setMove(true);
    }

    public void CanDock(bool _canDock)
    {
        canDock = _canDock;
    }

    private IEnumerator DrainRubbish()
    {
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Rubbish"))
        {
            Destroy(collision.transform.gameObject);
            currentRubbish++;

            currentRubbish = Mathf.Clamp(currentRubbish, 0, maxRubbish);
        }
    }
}
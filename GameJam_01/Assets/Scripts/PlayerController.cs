using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int maxRubbish;

    [SerializeField]
    private float dockTime;

    [SerializeField]
    private ParticleSystem rubbishDumpParticle;

    [SerializeField]
    private XboxController controller;

    [SerializeField]
    private float currentRubbish;

    private Player movement;

    private bool canDock = false;

    private void Awake()
    {
        movement = GetComponent<Player>();
    }

    public void Reset()
    {
        canDock = false;
    }

    private void Update()
    {
        if (canDock && XCI.GetButton(XboxButton.X, controller))
        {
            movement.setMove(false);

            canDock = false;
            Manager.instance.DisplayDockPrompt(false);

            StartCoroutine("DrainRubbish");

            Invoke("ReleaseDock", dockTime);
        }
    }

    private void ReleaseDock()
    {
        movement.setMove(true);
    }

    public void CanDock(bool _canDock)
    {
        canDock = _canDock;
    }

    public float Rubbish
    {
        get { return currentRubbish; }
    }

    public void RemoveRubbish(float change)
    {
        currentRubbish -= change * currentRubbish;

        currentRubbish = Mathf.Clamp(currentRubbish, 0, maxRubbish);
    }

    private IEnumerator DrainRubbish()
    {
        if (rubbishDumpParticle)
        {
            rubbishDumpParticle.Play();
        }

        float speed = 1 / dockTime;
        float percent = 0;

        float from = currentRubbish;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            currentRubbish = Mathf.Lerp(from, 0, percent);

            yield return null;
        }

        if (rubbishDumpParticle)
        {
            rubbishDumpParticle.Stop();
        }
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
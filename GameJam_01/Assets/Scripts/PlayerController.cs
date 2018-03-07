using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int maxRubbish;

    [SerializeField]
    private int RubbishPerPickup = 10;

    [SerializeField]
    private float dockTime;

    [SerializeField]
    private Sprite player1Highlight;

    [SerializeField]
    private Sprite player2Highlight;

    private GameObject rubbishDumpParticle;

    private XboxController controller;

    private float currentRubbish;

    private Player movement;

    private bool canDock = false;

    private void Awake()
    {
        movement = GetComponent<Player>();

        rubbishDumpParticle = GetComponentInChildren<ParticleSystem>().gameObject;

        if (rubbishDumpParticle)
        {
            rubbishDumpParticle.SetActive(false);
        }

    }

    public void StopDumping()
    {
        StopAllCoroutines();
    }

    public void Reset()
    {
        canDock = false;
    }

    private void Update()
    {
        if (canDock && XCI.GetButton(XboxButton.X, controller) && currentRubbish > 0)
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

    public float RubbishPercent
    {
        get { return currentRubbish / maxRubbish; }
    }

    public void RemoveRubbish(float change)
    {
        currentRubbish -= change * currentRubbish;

        currentRubbish = Mathf.Clamp(currentRubbish, 0, maxRubbish);

        Manager.instance.UpdateMetre(IsPlayerOne(), RubbishPercent);
    }

    private IEnumerator DrainRubbish()
    {
        if (rubbishDumpParticle)
        {
            rubbishDumpParticle.SetActive(true);
        }

        float speed = 1 / dockTime;
        float percent = 0;

        float from = currentRubbish;
        int score = (int)(currentRubbish / dockTime) / RubbishPerPickup;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            currentRubbish = Mathf.Lerp(from, 0, percent);
            Manager.instance.AddScore(IsPlayerOne(), score);

            Manager.instance.UpdateMetre(IsPlayerOne(), RubbishPercent);

            yield return null;
        }

        if (rubbishDumpParticle)
        {
            rubbishDumpParticle.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Rubbish"))
        {
            if (currentRubbish < maxRubbish)
            {
                Destroy(collision.collider.gameObject);

                currentRubbish += RubbishPerPickup;

                currentRubbish = Mathf.Clamp(currentRubbish, 0, maxRubbish);

                Manager.instance.UpdateMetre(IsPlayerOne(), RubbishPercent);
            }
        }
    }

    public void SetController(XboxController controls)
    {
        controller = controls;

        GetComponent<Player>().setController(controls);

        if (IsPlayerOne())
        {
            GetComponentInChildren<SpriteRenderer>().sprite = player1Highlight;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = player2Highlight;
        }
    }

    public bool IsPlayerOne()
    {
        return (int)controller == 1;
    }
}
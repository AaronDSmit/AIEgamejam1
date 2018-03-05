using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private int p1Score;

    [SerializeField]
    private int p2Score;

    [SerializeField]
    private int roundTimeM;

    [SerializeField]
    private int roundTimeS;

    [SerializeField]
    private Text Timer;

    bool waitingToStart = false;

    private int currentMinute;

    private int currentSeconds;

    void Start()
    {
        waitingToStart = true;
    }

    private void StartRound()
    {
        p1Score = 0;
        p2Score = 0;

        currentMinute = roundTimeM;
        currentSeconds = roundTimeS;

        InvokeRepeating("CountDown", 0.0f, 1.0f);
    }

    private void Update()
    {
        if (waitingToStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartRound();

                waitingToStart = false;
            }
        }
    }

    private void EndRound()
    {
        string winner = (p1Score > p2Score) ? "Player 1" : "Player 2";

        Debug.Log(winner + " has won!");
    }

    private void CountDown()
    {
        if (roundTimeS > 0.0)
        {
            roundTimeS -= 1;
        }
        else if (roundTimeM > 0)
        {
            roundTimeM -= 1;
            roundTimeS = 59;
        }
        else
        {
            roundTimeM = 0;
            roundTimeS = 0;

            CancelInvoke();
            EndRound();
        }

        Timer.text = "Time: ";

        if (roundTimeM < 10)
        {
            Timer.text += "0" + roundTimeM;
        }
        else
        {
            Timer.text += roundTimeM;
        }

        Timer.text += ":";

        if (roundTimeS < 10)
        {
            Timer.text += "0" + roundTimeS;
        }
        else
        {
            Timer.text += roundTimeS;
        }
    }

    public void AddScore(bool player1, int score)
    {
        if (player1)
        {
            p1Score += score;
        }
        else
        {
            p2Score += score;
        }
    }
}
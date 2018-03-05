using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private int p1Score;

    private int p2Score;

    [SerializeField]
    private int roundTimeMinutes;

    [SerializeField]
    private int roundTimeSeconds;

    [SerializeField]
    private float flashSpeed;

    [Header("UI References")]
    [SerializeField]
    private Text Timer;

    [SerializeField]
    private Text player1Score;

    [SerializeField]
    private Text player2Score;

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

        currentMinute = roundTimeMinutes;
        currentSeconds = roundTimeSeconds;

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
        if (currentSeconds > 0.0)
        {
            currentSeconds -= 1;
        }
        else if (currentMinute > 0)
        {
            currentMinute -= 1;
            currentSeconds = 59;
        }
        else
        {
            currentMinute = 0;
            currentSeconds = 0;

            CancelInvoke();

            FlashTimer();

            EndRound();
        }


        // Format Text to match a digital Clock
        Timer.text = "";

        if (currentMinute < 10)
        {
            Timer.text += "0" + currentMinute;
        }
        else
        {
            Timer.text += currentMinute;
        }

        Timer.text += ":";

        if (currentSeconds < 10)
        {
            Timer.text += "0" + currentSeconds;
        }
        else
        {
            Timer.text += currentSeconds;
        }
    }

    public void AddScore(bool player1, int score)
    {
        if (player1)
        {
            p1Score += score;
            player1Score.text = p1Score.ToString();
        }
        else
        {
            p2Score += score;
            player2Score.text = p2Score.ToString();
        }
    }

    public void DisplayDockPrompt(bool display)
    {

    }

    private void FlashTimer()
    {
        if (waitingToStart)
        {
            Timer.enabled = true;
            Timer.text = "";
        }
        else
        {
            Timer.enabled = !Timer.enabled;
            Invoke("FlashTimer", flashSpeed);
        }
    }
}
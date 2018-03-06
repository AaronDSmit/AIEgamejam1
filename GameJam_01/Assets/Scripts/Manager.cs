using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Global Manager ref
    public static Manager instance;

    private int p1Score;

    private int p2Score;

    [Header("Round Time")]
    [SerializeField]
    private int roundTimeMinutes;

    [SerializeField]
    private int roundTimeSeconds;

    [SerializeField]
    [Tooltip("The rate at which the timer flashes while at 00:00")]
    private float flashSpeed;

    [Header("UI References")]
    private Text Timer;

    private Text player1Score;

    private Text player2Score;

    private GameObject dockPrompt;

    private bool inMainMenu = false;
    private bool inSelectMenu = false;

    private bool waitingToStart = false;

    private int currentMinute;

    private int currentSeconds;

    // Main Menu UI

    [SerializeField]
    private int currentButton;

    private bool canMove = true;

    private float timeNextMove;

    [SerializeField]
    private bool player1Ready = false;

    [SerializeField]
    private bool player2Ready = false;

    private int player1Char;

    private int player2Char;

    [SerializeField]
    private Text[] buttons;

    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject PlayerSelect;

    [SerializeField]
    private float buttonSelectDelay;

    [SerializeField]
    private Color selectedColour = Color.white;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //stop the game from having more than one Manager
            Destroy(gameObject);

            return;
        }

        //Don't destroy the Manager in scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentButton = 0;
        inMainMenu = true;

        player1Char = 0;
        player2Char = 0;

        buttons[currentButton].color = selectedColour;
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
        if (inMainMenu)
        {
            if (canMove && Mathf.Abs(XCI.GetAxisRaw(XboxAxis.LeftStickY, XboxController.All)) > 0.5f)
            {
                buttons[currentButton].color = Color.black;

                if (XCI.GetAxisRaw(XboxAxis.LeftStickY, XboxController.All) > 0.0f)
                {
                    currentButton = 0;
                }
                else
                {
                    currentButton = 1;
                }

                buttons[currentButton].color = selectedColour;

                canMove = false;
                timeNextMove = Time.time + buttonSelectDelay;
            }

            if (XCI.GetButton(XboxButton.A, XboxController.All))
            {
                if (currentButton == 0)
                {
                    MainMenu.SetActive(false);
                    PlayerSelect.SetActive(true);


                    inMainMenu = false;
                    inSelectMenu = true;
                }

                if (currentButton == 1)
                {
                    Application.Quit();
                }
            }

            if (!canMove && Time.time > timeNextMove)
            {
                canMove = true;
            }
        }

        if (inSelectMenu)
        {
            // Player 1
            if (XCI.GetButton(XboxButton.A, XboxController.First))
            {

            }

            // Player 2
            if (XCI.GetButton(XboxButton.A, XboxController.Second))
            {

            }


            if (player1Ready && player2Ready)
            {
                buttons[2].color = selectedColour;

                if (XCI.GetButton(XboxButton.A, XboxController.All))
                {
                    SceneManager.LoadScene(1);
                }
            }
        }

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


        if (Timer)
        {
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
    }

    public void AddScore(bool player1, int score)
    {
        if (player1)
        {
            p1Score += score;

            if (player1Score)
            {
                player1Score.text = p1Score.ToString();
            }
        }
        else
        {
            p2Score += score;

            if (player2Score)
            {
                player2Score.text = p2Score.ToString();
            }
        }
    }

    public void DisplayDockPrompt(bool display)
    {
        dockPrompt.SetActive(display);
    }

    private void FlashTimer()
    {
        if (Timer)
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
}
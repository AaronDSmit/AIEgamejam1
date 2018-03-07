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
    private int roundTimeMinutes = 0;

    [SerializeField]
    private int roundTimeSeconds = 0;

    [SerializeField]
    [Tooltip("The rate at which the timer flashes while at 00:00")]
    private float flashSpeed = 0.5f;

    [Header("UI References")]
    private Text Timer;

    private Text player1Score;
    private Text player2Score;

    private RectTransform player1Metre;
    private RectTransform player2Metre;

    private GameObject player1Win;
    private GameObject player2Win;
    private GameObject tie;

    private GameObject dockPrompt;

    private bool inMainMenu;
    private bool inSelectMenu;

    private int currentMinute;

    private int currentSeconds;

    private Transform p1SpawnPoint;

    private Transform p2SpawnPoint;

    // Main Menu UI

    [Header("Menu Variables")]
    [SerializeField]
    private GameObject[] players;

    private int currentButton;

    private bool canMove = true;

    private bool canMoveP1 = true;
    private bool canMoveP2 = true;

    private float timeNextMove;

    private float timeNextMoveP1;
    private float timeNextMoveP2;

    private bool player1Ready;

    [SerializeField]
    private bool player2Ready;

    [SerializeField]
    private Vector2Int player1Index;

    [SerializeField]
    private Vector2Int player2Index;

    private int player1Char = 0;
    private int player2Char = 1;

    private Text[] buttons;

    private GameObject MainMenu;

    private GameObject PlayerSelect;

    private Image[,] selectionGrid;

    [SerializeField]
    private float buttonSelectDelay;

    [SerializeField]
    private Color selectedColour = Color.white;

    [SerializeField]
    private Sprite playerOneHighlight;

    [SerializeField]
    private Sprite playerTwoHighlight;

    [SerializeField]
    private Sprite defaultBorder;

    [SerializeField]
    private Sprite lockedInBorder;

    private int gridWidth = 5;

    private int gridHeight = 3;

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
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartGame();
    }

    private void StartGame()
    {
        // get references to UI
        GameObject canvas = FindObjectOfType<Canvas>().gameObject;

        if (canvas)
        {
            MainMenu = canvas.transform.GetChild(0).gameObject;

            Button[] canvasButtons = canvas.GetComponentsInChildren<Button>();

            buttons = new Text[canvasButtons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = canvasButtons[i].GetComponent<Text>();
            }

            PlayerSelect = canvas.transform.GetChild(1).gameObject;

            if (PlayerSelect)
            {
                selectionGrid = new Image[gridWidth, gridHeight];

                for (int y = 0; y < gridHeight; y++)
                {
                    for (int x = 0; x < gridWidth; x++)
                    {
                        selectionGrid[x, y] = PlayerSelect.transform.GetChild(2).GetChild(y).GetChild(x).GetComponent<Image>();
                        selectionGrid[x, y].sprite = defaultBorder;
                    }
                }

                PlayerSelect.SetActive(false);
            }
        }

        currentButton = 0;
        inMainMenu = true;
        inSelectMenu = false;

        player1Ready = false;
        player2Ready = false;

        player1Index = new Vector2Int(0, 0);
        player2Index = new Vector2Int(4, 0);

        selectionGrid[player1Index.x, player1Index.y].sprite = playerOneHighlight;
        selectionGrid[player2Index.x, player2Index.y].sprite = playerTwoHighlight;

        buttons[currentButton].color = selectedColour;
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

            if (XCI.GetButtonUp(XboxButton.A, XboxController.All))
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
            if (!player1Ready)
            {
                // Move left/Right
                if (canMoveP1 && Mathf.Abs(XCI.GetAxisRaw(XboxAxis.LeftStickX, XboxController.First)) > 0.5f)
                {
                    if (XCI.GetAxisRaw(XboxAxis.LeftStickX, XboxController.First) > 0.0f)
                    {
                        selectionGrid[player1Index.x, player1Index.y].sprite = defaultBorder;
                        player1Index.x = MoveX(player1Index.x, 1);
                        selectionGrid[player1Index.x, player1Index.y].sprite = playerOneHighlight;
                    }
                    else
                    {
                        selectionGrid[player1Index.x, player1Index.y].sprite = defaultBorder;
                        player1Index.x = MoveX(player1Index.x, -1); ;
                        selectionGrid[player1Index.x, player1Index.y].sprite = playerOneHighlight;
                    }

                    canMoveP1 = false;
                    timeNextMoveP1 = Time.time + buttonSelectDelay;
                }

                // Move up/down
                if (canMoveP1 && Mathf.Abs(XCI.GetAxisRaw(XboxAxis.LeftStickY, XboxController.First)) > 0.5f)
                {
                    if (XCI.GetAxisRaw(XboxAxis.LeftStickY, XboxController.First) > 0.0f)
                    {
                        selectionGrid[player1Index.x, player1Index.y].sprite = defaultBorder;
                        player1Index.y = MoveY(player1Index.y, -1);
                        selectionGrid[player1Index.x, player1Index.y].sprite = playerOneHighlight;
                    }
                    else
                    {
                        selectionGrid[player1Index.x, player1Index.y].sprite = defaultBorder;
                        player1Index.y = MoveY(player1Index.y, 1);
                        selectionGrid[player1Index.x, player1Index.y].sprite = playerOneHighlight;
                    }

                    canMoveP1 = false;
                    timeNextMoveP1 = Time.time + buttonSelectDelay;
                }

                if (!canMoveP1 && Time.time > timeNextMoveP1)
                {
                    canMoveP1 = true;
                }

                if (XCI.GetButtonDown(XboxButton.A, XboxController.First))
                {
                    selectionGrid[player1Index.x, player1Index.y].sprite = lockedInBorder;
                    player1Ready = true;
                }
            }

            // Player 2
            if (!player2Ready)
            {
                // Move left/Right
                if (canMoveP2 && Mathf.Abs(XCI.GetAxisRaw(XboxAxis.LeftStickX, XboxController.Second)) > 0.5f)
                {
                    if (XCI.GetAxisRaw(XboxAxis.LeftStickX, XboxController.Second) > 0.0f)
                    {
                        selectionGrid[player2Index.x, player2Index.y].sprite = defaultBorder;
                        player2Index.x = MoveX(player2Index.x, 1);
                        selectionGrid[player2Index.x, player2Index.y].sprite = playerTwoHighlight;
                    }
                    else
                    {
                        selectionGrid[player2Index.x, player2Index.y].sprite = defaultBorder;
                        player2Index.x = MoveX(player2Index.x, -1); ;
                        selectionGrid[player2Index.x, player2Index.y].sprite = playerTwoHighlight;
                    }

                    canMoveP2 = false;
                    timeNextMoveP2 = Time.time + buttonSelectDelay;
                }

                // Move up/down
                if (canMoveP2 && Mathf.Abs(XCI.GetAxisRaw(XboxAxis.LeftStickY, XboxController.Second)) > 0.5f)
                {
                    if (XCI.GetAxisRaw(XboxAxis.LeftStickY, XboxController.Second) > 0.0f)
                    {
                        selectionGrid[player2Index.x, player2Index.y].sprite = defaultBorder;
                        player2Index.y = MoveY(player2Index.y, -1);
                        selectionGrid[player2Index.x, player2Index.y].sprite = playerTwoHighlight;

                    }
                    else
                    {
                        selectionGrid[player2Index.x, player2Index.y].sprite = defaultBorder;
                        player2Index.y = MoveY(player2Index.y, 1);
                        selectionGrid[player2Index.x, player2Index.y].sprite = playerTwoHighlight;
                    }

                    canMoveP2 = false;
                    timeNextMoveP2 = Time.time + buttonSelectDelay;
                }

                if (XCI.GetButtonDown(XboxButton.A, XboxController.Second))
                {
                    selectionGrid[player2Index.x, player2Index.y].sprite = lockedInBorder;
                    player2Ready = true;
                }

                if (!canMoveP2 && Time.time > timeNextMoveP2)
                {
                    canMoveP2 = true;
                }
            }


            if (player1Ready && player2Ready)
            {
                buttons[2].color = selectedColour;

                if (XCI.GetButtonDown(XboxButton.A, XboxController.All))
                {
                    inSelectMenu = false;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }

    private int MoveX(int value, int change)
    {
        if (change > 0 && value < 4)
        {
            return value + 1;
        }

        if (change < 0 && value > 0)
        {
            return value - 1;
        }

        return value;
    }

    private int MoveY(int value, int change)
    {
        if (change > 0 && value < 2)
        {
            return value + 1;
        }

        if (change < 0 && value > 0)
        {
            return value - 1;
        }

        return value;
    }

    private void EndRound()
    {
        if (p1Score > p2Score)
        {
            player1Win.SetActive(true);
        }
        else if (p1Score < p2Score)
        {
            player2Win.SetActive(true);
        }
        else
        {
            tie.SetActive(true);
        }

        Invoke("RestartGame", 3.0f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateMetre(bool playerOne, float percent)
    {
        if (playerOne)
        {
            player1Metre.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600 * percent);
        }
        else
        {
            player2Metre.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600 * percent);
        }
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
        if (dockPrompt) dockPrompt.SetActive(display);
    }

    private void FlashTimer()
    {
        if (Timer)
        {
            Timer.enabled = !Timer.enabled;
            Invoke("FlashTimer", flashSpeed);
        }
    }

    private void OnSceneLoaded(Scene newSecene, LoadSceneMode mode)
    {
        if (newSecene.buildIndex == 1)
        {
            p1SpawnPoint = GameObject.FindGameObjectWithTag("p1SpawnPoint").transform;
            p2SpawnPoint = GameObject.FindGameObjectWithTag("p2SpawnPoint").transform;


            player1Char = player1Index.y * gridWidth + player1Index.x;
            player2Char = player2Index.y * gridWidth + player2Index.x;

            PlayerController playerOne = Instantiate(players[player1Char], p1SpawnPoint.position, Quaternion.identity).GetComponent<PlayerController>();
            PlayerController playerTwo = Instantiate(players[player2Char], p2SpawnPoint.position, Quaternion.identity).GetComponent<PlayerController>();

            playerOne.SetController(XboxController.First);
            playerTwo.SetController(XboxController.Second);

            dockPrompt = GameObject.FindGameObjectWithTag("DockPrompt");

            if (dockPrompt) dockPrompt.SetActive(false);

            player1Score = GameObject.FindGameObjectWithTag("p1score").GetComponent<Text>();
            player2Score = GameObject.FindGameObjectWithTag("p2score").GetComponent<Text>();

            player1Metre = GameObject.FindGameObjectWithTag("p1Metre").transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            player2Metre = GameObject.FindGameObjectWithTag("p2Metre").transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

            player1Win = GameObject.FindGameObjectWithTag("p1Win");
            player2Win = GameObject.FindGameObjectWithTag("p2Win");

            tie = GameObject.FindGameObjectWithTag("Tie");

            if (player1Win) player1Win.SetActive(false);
            if (player2Win) player2Win.SetActive(false);
            if (tie) tie.SetActive(false);

            Timer = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();

            StartRound();
        }
        else if (newSecene.buildIndex == 0)
        {
            StartGame();
        }
    }
}
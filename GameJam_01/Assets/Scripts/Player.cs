using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;



public class Player : MonoBehaviour
{



    [SerializeField]
    private float baseSpeed = 10;

    private float speed;

    [SerializeField]
    private float DashSpeed = 50;

    [SerializeField]
    private XboxController controller;



    [SerializeField]
    private float Timer = 0.1f;

    private Rigidbody rb;

    [SerializeField]
    Vector3 movement;

    [SerializeField]
    private float hitforce = 100;


    [Range(0.0f, 1.0f)]
    public float dustpercent = 0.0f;

    PlayerController rival;


    private bool dash = false;
    private bool CanMove = true;


    private float y;
    private float x;

    private float prevRotatex;
    private float prevRotatey;

    private float resetTimer;



    // Use this for initialization
    void Awake()
    {
        prevRotatex = 0.0f;
        prevRotatey = 0.0f;
        speed = baseSpeed;
        resetTimer = Timer;

        rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {


        if (!CanMove)
        {
            rb.velocity = Vector3.zero;
            return;

        }

        Move();
   

        if (XCI.GetButtonDown(XboxButton.A, controller))
        {
            dash = true;


        }




    }


    private void FixedUpdate()
    {
        Dash();
    }



    void Dash()
    {


        if (dash)
        {

            speed = DashSpeed;
            Timer -= Time.fixedDeltaTime;

            if (Timer <= 0)
            {
                speed = baseSpeed;
                dash = false;

                ResetCoolTimer();

            }
        }





    }



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rival = collision.gameObject.GetComponent<PlayerController>();

            GameObject enemy = collision.gameObject;

            Rigidbody erb = enemy.GetComponent<Rigidbody>();

            Vector3 direction = (erb.transform.position - rb.transform.position) + rb.velocity;



            direction.Normalize();

            if (speed >= DashSpeed)
            {


                rival.RemoveRubbish(dustpercent);

                erb.AddForce(direction * hitforce, ForceMode.Impulse);
            }
            else
            {
                return;
            }
        }

       


    }


    public void setMove(bool move)
    {
        CanMove = move;

    }


    void Move()
    {
        y = XCI.GetAxisRaw(XboxAxis.LeftStickY, controller);
        x = XCI.GetAxisRaw(XboxAxis.LeftStickX, controller);


        movement = new Vector3(x, 0, y);

        if (x != 0 || y != 0)
        {
            rb.velocity = transform.forward * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;

        }

        if (x == 0f)
        {
            x = prevRotatex;
        }
        else
        {
            prevRotatex = x;
        }

        if (y == 0f)
        {
            y = prevRotatey;
        }

        else
        {
            prevRotatey = y;
        }

        if (x != 0 || y != 0)
        {
            Vector3 direction = new Vector3(x, 0, y);


            transform.rotation = Quaternion.LookRotation(direction);
        }




    }




    private void ResetCoolTimer()
    {
        Timer = resetTimer;
    }

    public void setController(XboxController control)
    {
        controller = control;
    }

}



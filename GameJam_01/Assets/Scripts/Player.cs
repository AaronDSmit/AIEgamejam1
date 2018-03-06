using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;



public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject[] player;
    [SerializeField]
    private float baseSpeed = 10;

    private float speed;

    [SerializeField]
    private float DashSpeed = 20;

    [SerializeField]
    private int BaseMashAmount = 10;

    [SerializeField]
    private XboxController controller;

    [SerializeField]
    private float time;

    [SerializeField]
    float TimesMAshed = 0;

    private float timer;

    private bool dash = false;

    [SerializeField]
    private bool CanMove = true;

    private int MashAmount;

    private float y;
    private float x;


    // Use this for initialization
    void Awake()
    {
        speed = baseSpeed;
        MashAmount = BaseMashAmount;
    }

    // Update is called once per frame
    void Update()
    {

        if (!CanMove)
        {
            return;
        }

        y = XCI.GetAxis(XboxAxis.LeftStickY, controller) * Time.deltaTime * speed;
        x = XCI.GetAxis(XboxAxis.LeftStickX, controller) * Time.deltaTime * speed;

        transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);

        rotation();
        Dash();
    }


    void buttonMash()
    {

        CanMove = false;
        if (XCI.GetButton(XboxButton.B, controller))
        {
            ++TimesMAshed;
            if (TimesMAshed >= MashAmount)
            {
                CanMove = true;
            }
        }

    }

    void Dash()
    {
        timer = 0;
        timer += Time.deltaTime;

        if (XCI.GetButtonDown(XboxButton.A))
        {

            speed = DashSpeed;


            dash = true;


        }



        if (timer > time)
        {
            speed = baseSpeed;
            dash = false;
        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        if (speed >= DashSpeed)
        {

            buttonMash();
        }
    }


    void rotation()
    {
        y = XCI.GetAxisRaw(XboxAxis.LeftStickY, controller);
        x = XCI.GetAxisRaw(XboxAxis.LeftStickX, controller);

        Vector3 direction = new Vector3(x, 0, y);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }

    public void setMove(bool move)
    {
        CanMove = move;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Player : MonoBehaviour {

    [SerializeField]
    private GameObject[] player;
    [SerializeField]
    private float baseSpeed = 10;

    private float speed;

    [SerializeField]
    private float DashSpeed = 20;
    [SerializeField]
    private float RubbishCap = 15;

    private float ROtatespeed = 300;

    [SerializeField]
    private int BaseMashAmount = 10;

    private bool dash;

    private int MashAmount;

    private float x;
    private float z;


    // Use this for initialization
    void Awake ()
    {
        speed = baseSpeed;
        MashAmount = BaseMashAmount;
	}
	
	// Update is called once per frame
	void Update ()
    {

        x = Input.GetAxis("Horizontal") * Time.deltaTime * ROtatespeed;
        z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }


    void buttonMash()
    {

        speed = baseSpeed;

    }

    void Dash()
    {

    }



    private void OnCollisionEnter(Collision collision)
    {
        if(speed >= DashSpeed)
        {
            speed = 0;
            buttonMash();
        }
    }

}

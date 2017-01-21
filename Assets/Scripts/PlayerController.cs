using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int player;
    public GameObject rotator;
    public GameObject head;
    public Animator body;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Groove();
        Shimmy();
        Pose();
	}

    private void Pose()
    {
        if (Input.GetButtonDown("A_"+ player))
        {
            body.Play("A");
        }
        else if (Input.GetButtonDown("B_" + player))
        {

            body.Play("B");
        }
        else if (Input.GetButtonDown("X_" + player))
        {

            body.Play("X");
        }
        else if (Input.GetButtonDown("Y_" + player))
        {
            body.Play("Y");
        }
    }

    private void Shimmy()
    {
        head.transform.localPosition = new Vector3(-Input.GetAxis("Triggers_" + player) * 0.2f + 0.44f, head.transform.localPosition.y, head.transform.localPosition.z);
    }

    private void Groove()
    {
        rotator.transform.localEulerAngles = new Vector3(-Input.GetAxis("L_YAxis_" + player) * 30, 0, -Input.GetAxis("L_XAxis_" + player) * 30);
    }
}

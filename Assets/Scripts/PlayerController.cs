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
		//TODO Maybe move all input checking to be in the handler?
        Groove();
        Shimmy();
	}

	public void PlayAnimationForButton(InputButton inputButton)
	{
		if (inputButton.buttonName.Equals ("A_"))
		{
			body.Play(player == 2 ? "2A" : "A");
		}
		else if (inputButton.buttonName.Equals ("B_"))
		{
			body.Play(player == 2 ? "2B" : "B");
		}
		else if (inputButton.buttonName.Equals ("X_"))
		{
			body.Play(player == 2 ? "2X" : "X");
		}	
		else if(inputButton.buttonName.Equals ("Y_"))
		{
			body.Play(player == 2 ? "2Y" : "Y");
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

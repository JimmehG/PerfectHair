using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    public int joystick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles = new Vector3(-Input.GetAxis("L_YAxis_" + joystick) * 30,0, -Input.GetAxis("L_XAxis_" + joystick) * 30);

    }
}

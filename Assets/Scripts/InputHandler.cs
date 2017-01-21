using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	InputButton[] inputButtons;

	AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		Object[] buttonObjects = Resources.LoadAll ("", typeof(InputButton));
		Debug.Log ("Found buttons: " + buttonObjects.Length);
		inputButtons = new InputButton[buttonObjects.Length];
		for(int i = 0; i < buttonObjects.Length; i++)
		{
			inputButtons [i] = (InputButton) buttonObjects [i];
			Debug.Log ("Input Button: " + inputButtons[i].buttonName);
		}
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (InputButton inputButton in inputButtons)
		{
			for (int i = 0; i < 2; i++)
			{
				if (Input.GetButtonDown (inputButton.buttonName + (i + 1)))
				{
					Debug.Log ("P" + (i + 1) + " pressed " + inputButton.buttonName + (i + 1));
					audioSource.PlayOneShot (inputButton.buttonAudio.getRandomAudioClip());
				}
			}
		}
	}
}

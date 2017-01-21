﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentState : MonoBehaviour
{
	enum State {STATE_BREATHER, STATE_ROUND, STATE_VICTORY};

	[SerializeField]
	Text countDownText;

	[SerializeField]
	AudioSource[] audioSources;

	[Header("Players")]

	[SerializeField]
	Slider[] sliders;

	[SerializeField]
	Text[] text;

	[SerializeField]
	[Tooltip("Player one keys")]
	List<KeyCode> playerOneKeyCodes;

	[SerializeField]
	[Tooltip("Player two keys")]
	List<KeyCode> playerTwoKeyCodes;

	private List<KeyCode>[] playerKeyCodes;

	private int highestLevel = 0;

	[Header("Round Time")]

	[SerializeField]
	[Tooltip("The minimum time a key will be available for button mashing.")]
	int minKeyTime = 2;
	[SerializeField]
	[Tooltip("The maximum time a key will be available for button mashing.")]
	int maxKeyTime = 5;

	[Header("Game State")]

	[ReadOnlyAttribute]
	[SerializeField]
	State state = State.STATE_BREATHER;

	[ReadOnlyAttribute]
	[SerializeField]
	[Tooltip("The current button to press")]
	KeyCode[] currentKey;

	[ReadOnlyAttribute]
	[SerializeField]
	[Tooltip("Number of times pressed for each player")]
	int[] count;

	[ReadOnlyAttribute]
	[SerializeField]
	int countDown;

	// Use this for initialization
	void Start ()
	{
		if (audioSources.Length > 1)
		{
			for (int i = 1; i < audioSources.Length; i++)
			{
				audioSources [i].mute = true;
			}
		}

		playerKeyCodes = new List<KeyCode>[2];
		playerKeyCodes [0] = playerOneKeyCodes;
		playerKeyCodes [1] = playerTwoKeyCodes;
		currentKey = new KeyCode[2];
		count = new int[2];
		PickKey ();
	}

	private void PickKey()
	{
		int pickedKey = Random.Range (0, 2);
		for (int i = 0; i < 2; i++)
		{
			count[i] = 0;
			currentKey [i] = playerKeyCodes[i] [pickedKey];
			text [i].text = "Press " + currentKey [i];
		}
		countDown = Random.Range (minKeyTime, maxKeyTime + 1);
		countDownText.text = "Time left: " + countDown;
		Debug.Log("Press for the next " + countDown + " seconds");
		state = State.STATE_ROUND;
		InvokeRepeating ("Countdown", 1, 1);
		Invoke ("KeyDone", countDown);
	}

	private void Countdown()
	{
		countDown--;
		countDownText.text = "Time left: " + countDown;
	}

	private void KeyDone()
	{
		CancelInvoke ();
		int winner = -1;
		int loser = -1;
		countDownText.text = "Times up!";
		state = State.STATE_BREATHER;
		//Debug.Log("You pressed " + currentKey + " " + count + " times.");
		if (count [0] > count [1])
		{
			winner = 0;
			loser = 1;
		}
		else if (count [1] > count [0])
		{
			winner = 1;
			loser = 0;
		}
		else
		{
			Debug.Log ("its a draw");
			text [0].text = "";
			text [1].text = "";
		}

		if (winner != -1)
		{
			sliders [winner].value++;
			int currentValue = (int) sliders [winner].value;
			if (sliders [winner].value < 5)
			{
				text [winner].text = "You won";
				text [loser].text = "You lost";
			}
			else
			{
				countDownText.text = "Player " + (winner+1) + " wins";
				text [winner].text = "";
				text [loser].text = "";
				state = State.STATE_VICTORY;
			}

			if (sliders [winner].value > highestLevel)
			{
				highestLevel = (int) sliders [winner].value;
				for (int i = 0; i < audioSources.Length; i++)
				{
					if (i != highestLevel)
					{
						audioSources [i].mute = true;
					}
					else
					{
						audioSources [i].mute = false;
					}

				}
			}
		}
		if(state != State.STATE_VICTORY)
			Invoke ("PickKey", 2);
	}

	// Update is called once per frame
	void Update ()
	{
		if (state == State.STATE_ROUND)
		{
			for (int i = 0; i < 2; i++)
			{
				if (Input.GetKeyDown (currentKey [i]))
				{
					Debug.Log ("Player " + (i + 1) + " pressed!");
					count [i]++;
					text [i].text = currentKey [i] + ": " + count [i];
				}
			}
		}
	}
}

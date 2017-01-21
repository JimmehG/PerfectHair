using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentState : MonoBehaviour
{
	enum State {STATE_BREATHER, STATE_ROUND, STATE_VICTORY};

	[SerializeField]
	Text countDownText;

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
		countDownText.text = "Times up!";
		state = State.STATE_BREATHER;
		//Debug.Log("You pressed " + currentKey + " " + count + " times.");
		if (count [0] > count [1])
		{
			Debug.Log ("Player one wins");
			sliders [0].value++;
			if (sliders [0].value < 5)
			{
				text [0].text = "You won";
				text [1].text = "You lost";
			}
			else
			{
				countDownText.text = "Player one wins";
				text [0].text = "";
				text [1].text = "";
				state = State.STATE_VICTORY;
			}
		}
		else if (count [1] > count [0])
		{
			Debug.Log ("Player two wins");
			sliders [1].value++;
			if (sliders [1].value < 5)
			{
				text [1].text = "You won";
				text [0].text = "You lost";
			}
			else
			{
				countDownText.text = "Player two wins";
				text [0].text = "";
				text [1].text = "";
				state = State.STATE_VICTORY;
			}
		}
		else
		{
			Debug.Log ("its a draw");
			text [0].text = "";
			text [1].text = "";
		}
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

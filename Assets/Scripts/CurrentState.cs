using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CurrentState : MonoBehaviour
{
	enum State {STATE_BREATHER, STATE_ROUND, STATE_VICTORY};

	[SerializeField]
	Text countDownText;

	[SerializeField]
	AudioSource[] audioSources;

	[SerializeField]
	AudioClip riseAudioClip;

	Dictionary<string, ButtonAudio> buttonAudioDictionary;

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

	int currentWinner = -1;

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
				audioSources [i].volume = 0.0f;
			}
		}

		playerKeyCodes = new List<KeyCode>[2];
		playerKeyCodes [0] = playerOneKeyCodes;
		playerKeyCodes [1] = playerTwoKeyCodes;
		currentKey = new KeyCode[2];
		count = new int[2];
		PickKey ();

		buttonAudioDictionary = new Dictionary<string, ButtonAudio> ();
		ButtonAudio[] buttonAudios = (ButtonAudio[]) Resources.FindObjectsOfTypeAll(typeof(ButtonAudio));
		foreach (ButtonAudio buttonAudio in buttonAudios)
		{
			buttonAudioDictionary.Add (buttonAudio.buttonName, buttonAudio);
		}
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
		//countDownText.text = "Time left: " + countDown;
		countDownText.text = "";
		Debug.Log("Press for the next " + countDown + " seconds");
		state = State.STATE_ROUND;
		//InvokeRepeating ("Countdown", 1, 1);
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
			currentWinner = 0;
			winner = 0;
			loser = 1;
		}
		else if (count [1] > count [0])
		{
			currentWinner = 1;
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
			GetComponent<AudioSource> ().PlayOneShot (riseAudioClip);
			int newValue = (int) sliders [winner].value + 1;
			StartCoroutine(IncreaseSliderOverTime ());
			if (newValue < 5)
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

			if (newValue > highestLevel)
			{
				highestLevel = newValue;
				StartCoroutine(IncreaseVolumeOverTime());
			}
		}
		if(state != State.STATE_VICTORY)
			Invoke ("PickKey", 2);
	}

	private IEnumerator IncreaseVolumeOverTime()
	{
		while (audioSources [highestLevel].volume < 1.0f)
		{
			audioSources [highestLevel].volume = audioSources [highestLevel].volume + 0.1f;
			audioSources [highestLevel-1].volume = audioSources [highestLevel-1].volume - 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator IncreaseSliderOverTime()
	{
		int nextValue = (int) sliders [currentWinner].value + 1;
		while (sliders [currentWinner].value < nextValue)
		{
			sliders [currentWinner].value = sliders [currentWinner].value + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		sliders [currentWinner].value = nextValue;
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
					ButtonAudio buttonAudio = buttonAudioDictionary [currentKey [i].ToString()];
					GetComponent<AudioSource> ().PlayOneShot (buttonAudio.getRandomAudioClip ());
				}
			}
		}
	}
}

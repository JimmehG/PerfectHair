using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CStateTwo : MonoBehaviour {

	[SerializeField]
	bool usingControllers = false;

    enum GameStates { before, game, heart, win };

    GameStates GameState;

	InputButton[] inputButtons;
	InputButton currentButton;
    
    public GameObject title;

    public Text AnnounceDisplay;

    public SpriteRenderer CommandBubble;

    public MeshRenderer APrompt;
    public MeshRenderer BPrompt;
    public MeshRenderer XPrompt;
    public MeshRenderer YPrompt;
    MeshRenderer CurrPrompt;

	[SerializeField]
	PlayerController[] playerCharacters;

	[SerializeField]
	DynamicSoundtrack dynamicSoundtrack;

    [SerializeField]
    AudioClip RiseAudioClip;

    public AudioClip ChangeAudioClip;

	AudioSource audioSource;
    
    Scores Score;

    [SerializeField]
    Slider[] sliders;

    [SerializeField]
    Hearts[] hearts;

    class Scores
    {
        public Scores()
        {
            p1R = 0;
            p2R = 0;
            p1G = 0;
            p2G = 0;
        }
        public int p1R;
        public int p2R;
        public int pressesToWinR = 80;
        public int p1G;
        public int p2G;
        int roundsToWinG = 7;

        public void changeP1(int change)
        {
            p1R = p1R + change;
            if (p1R < 0)
                p1R = 0;
        }

        public void changeP2(int change)
        {
            p2R = p2R + change;
            if (p2R < 0)
                p2R = 0;
        }

        public bool p1WinR()
        {
            return p1R >= pressesToWinR;
        }

        public bool p2WinR()
        {
            return p2R >= pressesToWinR;
        }

        public bool p1WinG()
        {
            return p1G >= roundsToWinG;
        }

        public bool p2WinG()
        {
            return p2G >= roundsToWinG;
        }

        public int Highest()
        {
			return Mathf.Max(p1G, p2G);
        }
    }

    public Animator Heart;
    private float HeartTime;
    public float FinishedHeartTime = 6f;
    private float SwitchTime;
    private float FinishedSwitchTime;

    // Use this for initialization
    void Start () {
		Object[] buttonObjects = Resources.LoadAll ("", typeof(InputButton));
		Debug.Log ("Found buttons: " + buttonObjects.Length);
		inputButtons = new InputButton[buttonObjects.Length];
		for(int i = 0; i < buttonObjects.Length; i++)
		{
			inputButtons [i] = (InputButton) buttonObjects [i];
			Debug.Log ("Input Button: " + inputButtons[i].buttonName);
		}

        Score = new Scores();
        AnnounceDisplay.text = "Press START to begin".ToUpper();

		audioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update () {
		switch(GameState)
        {
            case GameStates.before:
                if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2"))
                {
                    GameState = GameStates.game;
                    AnnounceDisplay.text = "";
                    title.SetActive(false);
                    SwitchButton();
                }
                break;
            case GameStates.game:

                if (Score.p1WinR() && Score.p2WinR())
                {
                    AnnounceDisplay.text = "TIE GO AGAIN!";
					ResetRound ();
                }
                else if (Score.p1WinR() || Score.p2WinR())
                {
                    GameState = GameStates.heart;
					int newScore;
					if (Score.p1WinR ())
					{
						newScore = Score.p1G + 1;
					}
					else
					{
						newScore = Score.p2G + 1;
					}
					Debug.Log ("New score: " + newScore + " highest score: " + Score.Highest());
					if (newScore > Score.Highest ())
					{//work out whether we've gone to a new stage
						dynamicSoundtrack.IncreaseSoundtrackStage ();
					}
                    //todo trashtalk //AnnounceDisplay.text = one of the trashtalks;
                    CurrPrompt.enabled = false;
                    Heart.Play(Score.p1WinR() ? "1" : "2");
					audioSource.PlayOneShot(RiseAudioClip);
                    CommandBubble.enabled = false;
                }

                SwitchTime += Time.deltaTime;
                if (SwitchTime >= FinishedSwitchTime)
                {
                    SwitchButton();
                }


                break;
            case GameStates.heart:
                //play heart animation and break for next round (taunt time)
                HeartTime += Time.deltaTime;
				AnnounceDisplay.text = ((int)Mathf.Round(FinishedHeartTime - HeartTime)).ToString() + " seconds till the next round!".ToUpper();
                if (HeartTime >= FinishedHeartTime)
                {
                    if (Score.p1WinR()) {
                        Score.p1G++;
                        hearts[0].AddHeart();
                    } else {
                        Score.p2G++;
                        hearts[1].AddHeart();
                    }
                    HeartTime = 0;
                    if (Score.p1WinG() || Score.p2WinG())
                        GameState = GameStates.win;
                    else
                    {
                        GameState = GameStates.game;
						ResetRound ();
                        AnnounceDisplay.text = "";
                        SwitchButton();
                    }

                }
                break;
            case GameStates.win:
                AnnounceDisplay.text = "PLAYER " + (Score.p1WinG() ? "1" : "2") + " WINS!";
				if (Score.p1WinG ())
				{
					playerCharacters [0].PlayAnimationForButton (currentButton);
				}
				else
				{
					playerCharacters [1].PlayAnimationForButton (currentButton);
				}
                if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2"))
                {
                    GameState = GameStates.before;
					ResetGame ();
                    AnnounceDisplay.text = "Press START to begin".ToUpper();
                }
                break;
        }
	}

    private void SwitchButton()
    {
		currentButton = inputButtons [Random.Range(0, inputButtons.Length)];
        FinishedSwitchTime = UnityEngine.Random.Range(1, 8);
		audioSource.PlayOneShot(ChangeAudioClip);
        SwitchTime = 0;
        if (CurrPrompt != null)
            CurrPrompt.enabled = false;
        CommandBubble.enabled = true;

		if (currentButton.buttonName.Equals ("A_"))
		{
			CurrPrompt = APrompt;
		}
		else if (currentButton.buttonName.Equals ("B_"))
		{
			CurrPrompt = BPrompt;
		}
		else if (currentButton.buttonName.Equals ("X_"))
		{
			CurrPrompt = XPrompt;
		}	
		else if(currentButton.buttonName.Equals ("Y_"))
		{
			CurrPrompt = YPrompt;
		}

		CurrPrompt.enabled = true;
		if (!usingControllers)
		{
			AnnounceDisplay.text = "P1: " + currentButton.keyCodes[0] + " P2: " + currentButton.keyCodes[1];
		}
    }

    public void ChangeScore(int player, int change)
    {
        if(player == 0)
        {
            Score.changeP1(change);
            sliders[player].value = Score.p1R;

        }
        else
        {
            Score.changeP2(change);
            sliders[player].value = Score.p2R;
        }
    }

	public void ResetRound()
	{
		Score.p1R = 0;
		Score.p2R = 0;
		sliders[0].value = Score.p1R;
		sliders[1].value = Score.p2R;
	}

	public void ResetGame()
	{
		ResetRound ();

		Score.p1G = 0;
		Score.p2G = 0;

		hearts [0].ResetHearts ();
		hearts [1].ResetHearts ();
	}
		
	public void PressButton(int player, InputButton inputButton)
	{
		if (GameState == GameStates.game)
		{
			audioSource.PlayOneShot (inputButton.buttonAudio.getRandomAudioClip());
			playerCharacters [player].PlayAnimationForButton (inputButton);
			int negValue = -5;
			if (currentButton == inputButton)
			{
				ChangeScore (player, 1);
			}
			else
			{
				ChangeScore (1, negValue);
			}
		}
	}
}

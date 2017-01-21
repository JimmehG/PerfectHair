using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CStateTwo : MonoBehaviour {


    enum GameStates { before, game, heart, win };

    enum ButtonEnum { A_, B_, X_, Y_ }

    GameStates GameState;

    ButtonEnum CurrentButton;

    public Text ButtonDisplay;

    public Text AnnounceDisplay;
    
    [SerializeField]
    AudioSource[] AudioSources;

    [SerializeField]
    AudioClip RiseAudioClip;
    
    public ButtonAudio A;
    public ButtonAudio B;
    public ButtonAudio X;
    public ButtonAudio Y;

    public AudioClip ChangeAudioClip;
    
    Scores Score;
    
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
            return Math.Max(p1G, p2G);
        }
    }

    public Animator Heart;
    private float HeartTime;
    public float FinishedHeartTime = 6f;
    private float SwitchTime;
    private float FinishedSwitchTime;

    // Use this for initialization
    void Start () {
        if (AudioSources.Length > 1)
        {
            for (int i = 1; i < AudioSources.Length; i++)
            {
                AudioSources[i].volume = 0.0f;
            }
        }
        Score = new Scores();
        AnnounceDisplay.text = "Press START to begin";
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
                    SwitchButton();
                }
                break;
            case GameStates.game:
                CheckInput();

                if (Score.p1WinR() && Score.p2WinR())
                {
                    AnnounceDisplay.text = "TIE GO AGAIN!";
                    Score.p1R = 0;
                    Score.p2R = 0;
                }
                else if (Score.p1WinR() || Score.p2WinR())
                {
                    GameState = GameStates.heart;
                    //todo trashtalk //AnnounceDisplay.text = one of the trashtalks;
                    ButtonDisplay.text = "";
                    Heart.Play(Score.p1WinR() ? "1" : "2");
                    GetComponent<AudioSource>().PlayOneShot(RiseAudioClip);
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
                AnnounceDisplay.text = ((int)Math.Round(FinishedHeartTime - HeartTime)).ToString() + " seconds till the next round!";
                if (HeartTime >= FinishedHeartTime)
                {
                    var newValue = 0;
                    if (Score.p1WinR()) {
                        Score.p1G++;
                        newValue = Score.p1G;
                    } else {
                        Score.p2G++;
                        newValue = Score.p2G;
                    }
                    HeartTime = 0;
                    if (Score.p1WinG() || Score.p2WinG())
                        GameState = GameStates.win;
                    else
                    {
                        GameState = GameStates.game;
                        Score.p1R = 0;
                        Score.p2R = 0;
                        AnnounceDisplay.text = "";
                        SwitchButton();
                        if (newValue > Score.Highest())
                        {
                            StartCoroutine(IncreaseVolumeOverTime());
                        }
                    }

                }
                break;
            case GameStates.win:
                AnnounceDisplay.text = "Player " + (Score.p1WinG() ? "1" : "2") + " wins!";
                if (Input.GetButtonDown("Start_1") || Input.GetButtonDown("Start_2"))
                {
                    GameState = GameStates.before;
                    AnnounceDisplay.text = "Press START to begin";
                }
                break;
        }
	}

    private void SwitchButton()
    {
        CurrentButton = (ButtonEnum)UnityEngine.Random.Range(0, 3);
        ButtonDisplay.text = CurrentButton.ToString().Substring(0, 1);
        FinishedSwitchTime = UnityEngine.Random.Range(1, 8);
        GetComponent<AudioSource>().PlayOneShot(ChangeAudioClip);
        SwitchTime = 0;
    }

    void CheckInput()
    {
        var negValue = 5;
        if (Input.GetButtonDown("A_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(A.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.A_)
                Score.p1R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("A_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(A.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.A_)
                Score.p2R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("B_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(B.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.B_)
                Score.p1R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("B_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(B.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.B_)
                Score.p2R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("X_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(X.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.X_)
                Score.p1R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("X_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(X.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.X_)
                Score.p2R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("Y_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(Y.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.Y_)
                Score.p1R++;
            else
                Score.p1R -= negValue;
        }
        if (Input.GetButtonDown("Y_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(Y.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.Y_)
                Score.p2R++;
            else
                Score.p1R -= negValue;
        }
    }

    private IEnumerator IncreaseVolumeOverTime()
    {
        while (AudioSources[Math.Min(Score.Highest(),AudioSources.Length-1)].volume < 1.0f)
        {
            AudioSources[Math.Min(Score.Highest(), AudioSources.Length - 1)].volume = AudioSources[Math.Min(Score.Highest(), AudioSources.Length - 1)].volume + 0.1f;
            AudioSources[Math.Min(Score.Highest(), AudioSources.Length - 1) - 1].volume = AudioSources[Math.Min(Score.Highest(), AudioSources.Length - 1) - 1].volume - 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

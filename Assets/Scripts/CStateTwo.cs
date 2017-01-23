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
    
    public GameObject title;

    public Text AnnounceDisplay;

    public SpriteRenderer CommandBubble;

    public MeshRenderer APrompt;
    public MeshRenderer BPrompt;
    public MeshRenderer XPrompt;
    public MeshRenderer YPrompt;
    MeshRenderer CurrPrompt;

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
        AnnounceDisplay.text = "Press START to begin".ToUpper();
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
                    CurrPrompt.enabled = false;
                    Heart.Play(Score.p1WinR() ? "1" : "2");
                    GetComponent<AudioSource>().PlayOneShot(RiseAudioClip);
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
                AnnounceDisplay.text = ((int)Math.Round(FinishedHeartTime - HeartTime)).ToString() + " seconds till the next round!";
                if (HeartTime >= FinishedHeartTime)
                {
                    var newValue = 0;
                    if (Score.p1WinR()) {
                        Score.p1G++;
                        newValue = Score.p1G;
                        hearts[0].AddHeart();
                    } else {
                        Score.p2G++;
                        newValue = Score.p2G;
                        hearts[1].AddHeart();
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
                    AnnounceDisplay.text = "Press START to begin".ToUpper();
                }
                break;
        }
	}

    private void SwitchButton()
    {
        CurrentButton = (ButtonEnum)UnityEngine.Random.Range(0, 4);
        FinishedSwitchTime = UnityEngine.Random.Range(1, 8);
        GetComponent<AudioSource>().PlayOneShot(ChangeAudioClip);
        SwitchTime = 0;
        if (CurrPrompt != null)
            CurrPrompt.enabled = false;
        CommandBubble.enabled = true;
        switch (CurrentButton)
        {
            case ButtonEnum.A_:
                CurrPrompt = APrompt;
                break;
            case ButtonEnum.B_:
                CurrPrompt = BPrompt;
                break;
            case ButtonEnum.X_:
                CurrPrompt = XPrompt;
                break;
            case ButtonEnum.Y_:
                CurrPrompt = YPrompt;
                break;
        }
        CurrPrompt.enabled = true;
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

    void CheckInput()
    {
        var negValue = -5;
        if (Input.GetButtonDown("A_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(A.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.A_)
                ChangeScore(0, 1);
            else
                ChangeScore(0, negValue);
        }
        if (Input.GetButtonDown("A_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(A.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.A_)
                ChangeScore(1, 1);
            else
                ChangeScore(1, negValue);
        }
        if (Input.GetButtonDown("B_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(B.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.B_)
                ChangeScore(0, 1);
            else
                ChangeScore(0, negValue);
        }
        if (Input.GetButtonDown("B_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(B.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.B_)
                ChangeScore(1, 1);
            else
                ChangeScore(1, negValue);
        }
        if (Input.GetButtonDown("X_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(X.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.X_)
                ChangeScore(0, 1);
            else
                ChangeScore(0, negValue);
        }
        if (Input.GetButtonDown("X_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(X.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.X_)
                ChangeScore(1, 1);
            else
                ChangeScore(1, negValue);
        }
        if (Input.GetButtonDown("Y_1"))
        {
            GetComponent<AudioSource>().PlayOneShot(Y.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.Y_)
                ChangeScore(0, 1);
            else
                ChangeScore(0, negValue);
        }
        if (Input.GetButtonDown("Y_2"))
        {
            GetComponent<AudioSource>().PlayOneShot(Y.getRandomAudioClip());
            if (CurrentButton == ButtonEnum.Y_)
                 ChangeScore(1, 1);
            else
                ChangeScore(1, negValue);
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

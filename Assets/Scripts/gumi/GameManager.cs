using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
# endif


public class GameManager : MonoBehaviour
{
    AudioSource myAudio;
    public static bool musicStart = false;
    public static int[] score = { 0, 0, 0 }; // Perfect, Good, Bad

    public static bool gameEnd = false;
    public static bool gameStart = false;
    bool isCount = false;

    public static float currentLoud;

    /* Panel Management */
    public GameObject Title; // EndPanel (Title, Score, Perfect, Good, Bad)
    public GameObject ScorePanel; // Score Panel (Perfect, Good ,Bad)
    public GameObject countDown;

    bool initShow;

    public GameObject loudText;
    public GameObject pause;
    public GameObject retry;

    public void Awake()
    {
        musicStart = false;
        gameEnd = false;
        gameStart = false;
        currentLoud = 0;
        for (int i = 0; i < score.Length; i++)
        {
            score[i] = 0;
        }
    }

    void Start()
    {
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        #endif
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart && !musicStart) // 음악 재생 X
        {
            myAudio.Play(); // 음악 재생
            musicStart = true; // 음악 재생 시작
        }

        if (DefaultObserverEventHandler.TrackingSuccess && !Pause.isPause)
        {
            if (!gameStart && !isCount)
            {
                isCount = true;
                StartCoroutine("CountDown");
            }
            else
            {
                if (!initShow)
                {
                    initShow = true;
                    gameObject.GetComponent<Pause>().PauseObject.SetActive(true);
                    ScorePanel.SetActive(true);
/*                    loudText.SetActive(true);*/
                }
                ShowScore();
/*                loudText.GetComponent<Text>().text = $"Loudness : {currentLoud}";*/
            }
        }

        if (musicStart && Pause.isPause)
        {
            myAudio.Pause();
        }
        else if (musicStart && !Pause.isPause && !myAudio.isPlaying)
        {
            myAudio.UnPause();
        }


        if (!Pause.isPause && musicStart && !myAudio.isPlaying) // 음악이 재생이 시작된 상태이고, 음악이 끝났다면 
        {
            Debug.Log("끝");
            gameEnd = true; // 겜 끝
        }


        if (gameEnd) // 겜 끝이면
        {
            StartCoroutine("SetTitle"); // 2초 뒤 End Panel 보여주기 (점수)
            ScorePanel.SetActive(false);
        }
    }

    IEnumerator CountDown()
    {
        Pause.countBlock = true;
        countDown.SetActive(true);
        countDown.GetComponent<Text>().text = "3";
        yield return new WaitForSeconds(1f);

        countDown.GetComponent<Text>().text = "2";
        yield return new WaitForSeconds(1f);

        countDown.GetComponent<Text>().text = "1";
        yield return new WaitForSeconds(1f);

        countDown.SetActive(false);

        gameStart = true;
        Pause.countBlock = false;
    }

    IEnumerator SetTitle()
    {
        yield return new WaitForSeconds(2f);

        Title.SetActive(true);
        pause.SetActive(false);
        retry.SetActive(false);
        Title.transform.GetChild(1).GetComponent<Text>().text = $"점수 : {(int)(((float)((score[0]*2)+(score[1]))/(float)((score[0]+score[1]+score[2])*2))*100)}점";
        Title.transform.GetChild(2).GetComponent<Text>().text = $"ㆍPerfect : {score[0]}";
        Title.transform.GetChild(3).GetComponent<Text>().text = $"ㆍGood : {score[1]}";
        Title.transform.GetChild(4).GetComponent<Text>().text = $"ㆍBad : {score[2]}";
    }

    void ShowScore()
    {
        ScorePanel.transform.GetChild(0).GetComponent<Text>().text = $"ㆍPerfect : {score[0]}";
        ScorePanel.transform.GetChild(1).GetComponent<Text>().text = $"ㆍGood : {score[1]}";
        ScorePanel.transform.GetChild(2).GetComponent<Text>().text = $"ㆍBad : {score[2]}";
    }
}

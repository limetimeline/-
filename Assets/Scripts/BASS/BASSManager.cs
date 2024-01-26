using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
# endif


public class BASSManager : MonoBehaviour
{
    public static bool gameStart = false;
    bool isCount = false;

    public static float currentLoud;

    /* Panel Management */
    public GameObject countDown;

    bool initShow;

    public GameObject loudText;

    public void Awake()
    {
        gameStart = false;
        currentLoud = 0;
    }

    // Update is called once per frame
    void Update()
    { 
        if (DefaultObserverEventHandler.TrackingSuccess && !PauseBASS.isPause)
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
                    gameObject.GetComponent<PauseBASS>().PauseObject.SetActive(true);
                    // loudText.SetActive(true);
                }
                // loudText.GetComponent<Text>().text = $"Loudness : {currentLoud}";
            }
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
        PauseBASS.countBlock = false;
    }
}

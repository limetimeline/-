using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseBASS : MonoBehaviour
{
    public static bool isPause;
    public static bool countBlock;
    public GameObject PauseObject;
    public GameObject StartObject;

    public GameObject countDown;

    public void Awake()
    {
        Time.timeScale = 1;
        isPause = false;
        countBlock = false;
    }

    public void PauseUnity()
    {
        if (!countBlock)
        {
            if (!isPause)
            {
                Time.timeScale = 0;
                isPause = true;
                PauseObject.SetActive(false);
                StartObject.SetActive(true);
            }
            else
            {
                StartCoroutine("Delay");
                
                PauseObject.SetActive(true);
                StartObject.SetActive(false);
            }
        } 
    }

    IEnumerator Delay()
    {

        countBlock = true;
        countDown.SetActive(true);
        countDown.GetComponent<Text>().text = "3";
        yield return new WaitForSecondsRealtime(1f);

        countDown.GetComponent<Text>().text = "2";
        yield return new WaitForSecondsRealtime(1f);

        countDown.GetComponent<Text>().text = "1";
        yield return new WaitForSecondsRealtime(1f);
        isPause = false;
        countDown.SetActive(false);
        countBlock = false;
        Time.timeScale = 1;
        DeviceObserverSettings a = new DeviceObserverSettings();
        ModelTargetSettings b = new ModelTargetSettings();
        a.ResetDeviceObserver();
        b.ResetModelTargets();

    }
}

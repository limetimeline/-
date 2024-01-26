using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrans : MonoBehaviour
{
    public static int bass=0;
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void OnClickMain()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickTabup()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickNorae()
    {
        SceneManager.LoadScene(4);
    }

    public void OnClickGumi()
    {
        SceneManager.LoadScene(5);
    }

    public void OnClickBASS()
    {
        SceneManager.LoadScene(3);
        bass = 0;
    }
    public void OnClickBASS1()
    {
        SceneManager.LoadScene(3);
        bass = 1;
    }
    public void OnClickBASS2()
    {
        SceneManager.LoadScene(3);
        bass = 2;
    }
    public void OnClickBASS3()
    {
        SceneManager.LoadScene(3);
        bass = 3;
    }
    public void OnClickBASS4()
    {
        SceneManager.LoadScene(3);
        bass = 4;
    }



    // btn
    public void ExitBtn()
    {
        Application.Quit();
    }

    public void HelpBtn()
    {
        SceneManager.LoadScene(1);
    }
}

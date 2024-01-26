using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static bool isMusic = false;
    float noteSpeed;
    UnityEngine.UI.Image noteImage;
    public bool dead = false;

    public void Awake()
    {
        isMusic = false;
    }

    void Start()
    {
        noteImage = GetComponent<UnityEngine.UI.Image>();
        noteSpeed = 5f;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameStart && !Pause.isPause)
        {
            transform.localPosition += Vector3.up * noteSpeed * Time.deltaTime;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MicroGPT : MonoBehaviour
{
    // 마이크로부터 얻은 오디오 데이터를 저장할 AudioClip 변수
    private AudioClip micInput;

    // 데시벨 값을 저장할 변수
    private float decibelValue;

    // 데시벨 값을 얻을 때마다 호출되는 함수
    private void GetDecibel()
    {
        // 마이크로부터 오디오 데이터를 얻어옵니다.
        float[] samples = new float[micInput.samples];
        micInput.GetData(samples, 0);

        // 오디오 데이터의 크기를 계산하여 데시벨 값을 얻습니다.
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample);
        }
        float rms = Mathf.Sqrt(sum / micInput.samples);
        decibelValue = 20f * Mathf.Log10(rms);


        Debug.Log($"decibel : {decibelValue}");
        GameManager.currentLoud = decibelValue;
        BASSManager.currentLoud = decibelValue;

    }

    // 초기화 및 마이크 입력 시작
    private void Start()
    {
        // 마이크를 사용 가능한지 확인합니다.
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        // 첫 번째 마이크를 사용하도록 설정합니다.
        string microphoneName = Microphone.devices[0];
        micInput = Microphone.Start(microphoneName, true, 1, AudioSettings.outputSampleRate);

        // 데시벨 값을 주기적으로 얻기 위해 코루틴을 시작합니다.
        StartCoroutine(GetDecibelRoutine());
    }


    // 마이크 입력 중지
    private void OnDestroy()
    {
        Microphone.End(null);
    }

    // 데시벨 값을 얻기 위한 코루틴 함수
    private System.Collections.IEnumerator GetDecibelRoutine()
    {
        while (true)
        {
            GetDecibel();
            yield return null;
        }
    }
}

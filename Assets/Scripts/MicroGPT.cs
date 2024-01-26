using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MicroGPT : MonoBehaviour
{
    // ����ũ�κ��� ���� ����� �����͸� ������ AudioClip ����
    private AudioClip micInput;

    // ���ú� ���� ������ ����
    private float decibelValue;

    // ���ú� ���� ���� ������ ȣ��Ǵ� �Լ�
    private void GetDecibel()
    {
        // ����ũ�κ��� ����� �����͸� ���ɴϴ�.
        float[] samples = new float[micInput.samples];
        micInput.GetData(samples, 0);

        // ����� �������� ũ�⸦ ����Ͽ� ���ú� ���� ����ϴ�.
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

    // �ʱ�ȭ �� ����ũ �Է� ����
    private void Start()
    {
        // ����ũ�� ��� �������� Ȯ���մϴ�.
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        // ù ��° ����ũ�� ����ϵ��� �����մϴ�.
        string microphoneName = Microphone.devices[0];
        micInput = Microphone.Start(microphoneName, true, 1, AudioSettings.outputSampleRate);

        // ���ú� ���� �ֱ������� ��� ���� �ڷ�ƾ�� �����մϴ�.
        StartCoroutine(GetDecibelRoutine());
    }


    // ����ũ �Է� ����
    private void OnDestroy()
    {
        Microphone.End(null);
    }

    // ���ú� ���� ��� ���� �ڷ�ƾ �Լ�
    private System.Collections.IEnumerator GetDecibelRoutine()
    {
        while (true)
        {
            GetDecibel();
            yield return null;
        }
    }
}

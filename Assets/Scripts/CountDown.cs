using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text time;
    public int TotalTime = 300;
    public AudioSource timeoutAudio;

    void Start()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        while (TotalTime >= 0)
        {
            time.text = string.Format("{0:D2}:{1:D2}", TotalTime / 60, TotalTime % 60);
            yield return new WaitForSeconds(1);
            TotalTime--;
        }
    }

    void Update()
    {
        if (TotalTime == 20)
        {
            time.color = Color.red;
            timeoutAudio.Play();
        }

        if (TotalTime == 0)
        {
            GameManager.GameOver(true);
        }
    }
}

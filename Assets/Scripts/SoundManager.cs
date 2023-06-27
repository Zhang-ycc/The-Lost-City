using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmAudio;
    public AudioSource beAudio;
    public AudioSource winAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Be()
    {
        bgmAudio.Stop();
        beAudio.Play();
    }

    void Win()
    {
        bgmAudio.Stop();
        winAudio.Play();
    }
}

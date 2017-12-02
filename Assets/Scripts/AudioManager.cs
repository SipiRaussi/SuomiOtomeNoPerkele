using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    AudioClip[] mainTheme;
    AudioSource audioSource;
    float currentTime;


    private void Awake()
    {
        LoadAudio();
        PlayAudio();
    }

    private void LoadAudio()
    {
        AssetBundle music = AssetBundle.LoadFromFile("AssetBundles/audio.music");
        mainTheme = new AudioClip[music.GetAllAssetNames().Length];
        mainTheme[0] = music.LoadAsset<AudioClip>("FinlandVania - MainTheme - Intro");
        mainTheme[1] = music.LoadAsset<AudioClip>("FinlandVania - MainTheme");

        audioSource = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (audioSource.clip == mainTheme[0] && Time.time >= currentTime + mainTheme[0].length)
        {
            audioSource.clip = mainTheme[1];
            audioSource.Play();
            audioSource.loop = true;
        }
    }

    private void PlayAudio()
    {
        audioSource.clip = mainTheme[0];
        audioSource.Play();
        currentTime = Time.time;
    }
}

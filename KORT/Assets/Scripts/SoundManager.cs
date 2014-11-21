﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();

                if (_instance == null) Debug.LogError("Missing SoundManager");
                else DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public AudioSource[] background_music;
    public bool play_on_awake = true;
    private static int track = 0;
    

    public void Awake()
    {
        // if this is the first instance, make this the singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            // keep new scene's music
            StopAllMusic();
            Debug.Log("poa: " + play_on_awake);
            _instance.play_on_awake = play_on_awake;
            _instance.background_music = background_music;

            if (_instance.play_on_awake) StartBeginMusic();


            // destroy other instances that are not the already existing singleton
            if (this != _instance)
                Destroy(this.gameObject);
        }

        StopAllMusic();
    }

    public void Start()
    {
        if (play_on_awake) StartBeginMusic();
    }

    private static void StopAllMusic()
    {
        for (int i = 0; i < _instance.background_music.Length; ++i)
        {
            _instance.background_music[i].Stop();
        }
    }
    private static void StartBeginMusic()
    {
        if (_instance.background_music.Length > 0)
            _instance.background_music[track].Play();
    }

    public static void SetMusic(int new_track)
    {
        if (new_track >= _instance.background_music.Length) return;

        _instance.background_music[new_track].Stop();
        track = new_track;
        _instance.background_music[track].Play();
    }
    public static void StopMusic()
    {
        if (track >= _instance.background_music.Length) return;

        _instance.background_music[track].Stop();
    }
    public static void StartMusic()
    {
        if (track >= _instance.background_music.Length) return;

        _instance.background_music[track].Play();
    }

}

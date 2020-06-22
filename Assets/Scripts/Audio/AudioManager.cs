using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance; //prevents it from creating 2 audio managers when changing scenes.

    GameManager GameManagerIstance;
    public bool switchMusic;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Play("Menu Soundtrack");

        switchMusic = false;

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;


        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);

        if (s == null)
        {
            Debug.Log("Sound: " + name + "not found. Maybe name is wrong?");
            return;
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

        s.source.Play();


    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);

        if (s == null)
        {
            Debug.Log("Sound: " + name + "not found. Maybe name is wrong?");
            return;
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

        s.source.Stop();


    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (GameManagerIstance.GameStart == true && switchMusic == false)
            {
                switchMusic = true;
                Stop("Menu Soundtrack");
                Play("Game Soundtrack");
            }
        }
    }
}
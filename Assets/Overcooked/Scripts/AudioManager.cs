using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, fastsfxSource, sfxPlayOnce;

    public static AudioManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {


        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }



    public void PlaySFX(string name, float pitch = 1f)
    {

        sfxSource.pitch = pitch;

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);

        }


    }

    public void FastSFXOnce(string name, float pitch = 1f)
    {
        fastsfxSource.pitch = pitch;
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            if(!fastsfxSource.isPlaying)
            {
                fastsfxSource.PlayOneShot(s.clip);
            }

        }


    }

    public void SFXOnce(string name, float pitch = 1f)
    {
        sfxPlayOnce.pitch = pitch;
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            if (!sfxPlayOnce.isPlaying)
            {
                sfxPlayOnce.PlayOneShot(s.clip);
            }

        }
    }

    public void PauseAllAudio()
    {
        PauseAudioSource(musicSource);
        PauseAudioSource(sfxSource);
        PauseAudioSource(fastsfxSource);
        PauseAudioSource(sfxPlayOnce);
    }
    public void UnpauseAllAudio()
    {
        UnpauseAudioSource(musicSource);
        UnpauseAudioSource(sfxSource);
        UnpauseAudioSource(fastsfxSource);
        UnpauseAudioSource(sfxPlayOnce);
    }
    private void PauseAudioSource(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            Debug.Log("music paused");
            source.Pause();
        }
    }

    private void UnpauseAudioSource(AudioSource source)
    {
        if (source != null && source.clip != null)
        {
            Debug.Log("music unpaused");
            source.UnPause();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;

    // Audio source collections for each player.
    private Dictionary<int, AudioSource[]> playerAudioSources;

    public AudioSource musicSource;

    // Individual sources for all players.
    public AudioSource sfxSource1, sfxPlayOnce1;
    public AudioSource sfxSource2, sfxPlayOnce2;
    public AudioSource sfxSource3, sfxPlayOnce3;
    public AudioSource sfxSource4, sfxPlayOnce4;
    public AudioSource sfxSourceNonPlayer, fastsfxSourceNonPlayer;

    public static AudioManager instance;
    public float fadeDuration;

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
        InitializePlayerAudioSources();
    }

    private void InitializePlayerAudioSources()
    {
        playerAudioSources = new Dictionary<int, AudioSource[]>
        {
            { 0, new AudioSource[] { sfxSource1, sfxPlayOnce1 } },
            { 1, new AudioSource[] { sfxSource2, sfxPlayOnce2 } },
            { 2, new AudioSource[] { sfxSource3, sfxPlayOnce3 } },
            { 3, new AudioSource[] { sfxSource4, sfxPlayOnce4 } },
            { 4, new AudioSource[] { sfxSourceNonPlayer, fastsfxSourceNonPlayer}}
        };
    }

    private AudioSource[] GetPlayerAudioSources(int playerID)
    {
        if (playerAudioSources.TryGetValue(playerID, out var sources))
        {
            return sources;
        }
        else
        {
            Debug.LogWarning($"Player ID {playerID} does not have assigned audio sources.");
            return null;
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



    public void PlaySFX(string name, int playerID, float pitch = 1f)
    {
        var sources = GetPlayerAudioSources(playerID);
        if (sources == null) return;

        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
            return;
        }

        sources[0].pitch = pitch; // sfxSource
        sources[0].PlayOneShot(s.clip);
    }


    public void FastSFXOnce(string name, float pitch = 1f)
    {
        fastsfxSourceNonPlayer.pitch = pitch;
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            if (!fastsfxSourceNonPlayer.isPlaying)
            {
                fastsfxSourceNonPlayer.PlayOneShot(s.clip);
            }

        }


    }

    public void SFXOnce(string name, int playerID, float pitch = 1f)
    {
        var sources = GetPlayerAudioSources(playerID);
        if (sources == null) return;

        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
            return;
        }

        sources[1].pitch = pitch; // sfxPlayOnce
        if (!sources[1].isPlaying)
        {
            sources[1].PlayOneShot(s.clip);
        }
    }

    public void PauseAllAudio()
    {
        AudioListener.pause = true;
    }
    public void UnpauseAllAudio()
    {
        AudioListener.pause = false;
    }

    IEnumerator StopAudioGradually(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void StopAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        if (allAudioSources.Length == 0) return;

        foreach (AudioSource audioS in allAudioSources)
        {
            StartCoroutine(StopAudioGradually(audioS, fadeDuration));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    float musicVolume = 0.5f;
    float sfxVolume = 0.7f;

    public Sound[] songs;
    public Sound[] soundEffects;
    public Sound[] characterVoices;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in songs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = musicVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = sfxVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
        foreach (Sound s in characterVoices)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = sfxVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void PlaySong(string name)
    {
        Sound s = Array.Find(songs, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Song: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlaySoundEffect(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound effect: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public IEnumerator CrossfadeSongs(string song1, string song2)
    {
        Sound s = Array.Find(songs, sound => sound.name == song1);
        Sound s2 = Array.Find(songs, sound => sound.name == song2);
        float currentTime = 0;
        float sourceVolume = s.source.volume;
        while (currentTime < 1f)
        {
            s.source.volume = Mathf.Lerp(sourceVolume, 0f, currentTime / 1f);
            currentTime += Time.deltaTime;
            yield return null;
        }
        s.source.Stop();
        s.source.volume = sourceVolume;
        s2.source.Play();
    }

    public IEnumerator FadeSongVolumeDown(string song)
    {
        Sound s = Array.Find(songs, sound => sound.name == song);
        float currentTime = 0;
        float sourceVolume = s.source.volume;
        while (currentTime < 0.5f)
        {
            s.source.volume = Mathf.Lerp(sourceVolume, sourceVolume * 0.3f, currentTime / 0.5f);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeSongVolumeUp(string song)
    {
        Sound s = Array.Find(songs, sound => sound.name == song);
        float currentTime = 0;
        float sourceVolume = s.source.volume;
        float targetVolume = musicVolume;
        while (currentTime < 0.5f)
        {
            s.source.volume = Mathf.Lerp(sourceVolume, targetVolume, currentTime / 0.5f);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void AdjustMusicVolume(float _musicVolume)
    {
        musicVolume = _musicVolume;
        foreach (Sound s in songs)
        {
            if (s.source) s.source.volume = musicVolume;
        }
    }

    public void AdjustSfxVolume(float _sfxVolume)
    {
        sfxVolume = _sfxVolume;
        foreach (Sound s in soundEffects)
        {
            if (s.source) s.source.volume = sfxVolume;
        }
        foreach (Sound s in characterVoices)
        {
            if (s.source) s.source.volume = sfxVolume;
        }
    }

    public void SetMusicVolume(float _musicVolume)
    {
        musicVolume = _musicVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetSfxVolume(float _sfxVolume)
    {
        sfxVolume = _sfxVolume;
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }
}

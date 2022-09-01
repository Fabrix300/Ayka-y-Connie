using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    float musicVolume;
    float sfxVolume;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationMenuController : MonoBehaviour
{
    public Animator configMenuOverlayAnim;
    public Animator configMenuPanelAnim;

    [Header("Audio Configuration Properties")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }

    public void OpenConfigMenu()
    {
        musicVolumeSlider.value = audioManager.GetMusicVolume();
        sfxVolumeSlider.value = audioManager.GetSfxVolume();
        gameObject.SetActive(true);
    }

    public void CloseConfigMenu()
    {
        configMenuPanelAnim.SetInteger("state", 1);
        configMenuOverlayAnim.SetInteger("state", 1);
    }

    public void SetMusicVolume(float _musicVolume)
    {
        audioManager.AdjustMusicVolume(_musicVolume);
    }

    public void SetSfxVolume(float _sfxVolume)
    {
        audioManager.AdjustSfxVolume(_sfxVolume);
    }
}

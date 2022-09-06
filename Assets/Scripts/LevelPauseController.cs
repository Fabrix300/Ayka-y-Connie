using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPauseController : MonoBehaviour
{
    public Animator configMenuOverlayAnim;
    public Animator configMenuPanelAnim;

    public static bool LevelIsPaused = false;

    [Header("Audio Configuration Properties")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }

    public void PauseLevel()
    {
        gameObject.SetActive(true);
        musicVolumeSlider.value = audioManager.GetMusicVolume();
        sfxVolumeSlider.value = audioManager.GetSfxVolume();
        Time.timeScale = 0f;
        LevelIsPaused = true;
    }

    public void ResumeLevel()
    {
        configMenuPanelAnim.SetInteger("state", 1);
        configMenuOverlayAnim.SetInteger("state", 1);
        Time.timeScale = 1f;
        LevelIsPaused = false;
    }

    public void SetMusicVolume(float _musicVolume)
    {
        audioManager.AdjustMusicVolume(_musicVolume);
    }

    public void SetSfxVolume(float _sfxVolume)
    {
        audioManager.AdjustSfxVolume(_sfxVolume);
    }

    public void ExitLevel()
    {
        StartCoroutine(ExitLevelCoroutine());
    }

    public IEnumerator ExitLevelCoroutine()
    {
        Time.timeScale = 1f;
        LevelIsPaused = false;
        configMenuPanelAnim.SetInteger("state", 1);
        configMenuOverlayAnim.SetInteger("state", 1);
        yield return new WaitForSeconds(1f);
        GameManager.instance.LoadSceneByName("GameMap");
    }
}

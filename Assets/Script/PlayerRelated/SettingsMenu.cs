using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (masterSlider != null)
        {
            float savedMaster = PlayerPrefs.GetFloat("MasterVolume", 1f);
            masterSlider.value = savedMaster;
            SetMasterVolume(savedMaster);
        }

        if (musicSlider != null)
        {
            float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicSlider.value = savedMusic;
            SetMusicVolume(savedMusic);
        }

        if (sfxSlider != null)
        {
            float savedSfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.value = savedSfx;
            SetSfxVolume(savedSfx);
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSfxVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }
}
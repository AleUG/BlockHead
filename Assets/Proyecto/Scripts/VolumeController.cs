using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VolumeController : MonoBehaviour
{
    [SerializeField] public Slider volumeSlider;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private GameObject canvasOptions;

    private void Start()
    {
        // Obtiene el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Comprueba si el nombre de la escena actual es "MenuPrincipal", "Cinem�ticaIntro" o "Cinem�ticaOutro"
        if (currentSceneName == "MenuPrincipal" || currentSceneName == "Cinem�ticaIntro" || currentSceneName == "Cinem�ticaOutro" || currentSceneName == "Cinem�ticaLevel_2" || currentSceneName == "Cinem�ticaLevel_1" || currentSceneName == "Credits")
        {
            canvasOptions.SetActive(false);
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
            SetMasterVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetMasterVolume()
    {
        float volume2 = volumeSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume2) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume2);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");

        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }
}

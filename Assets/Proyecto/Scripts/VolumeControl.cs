using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public bool useAudioSource;
    public List<AudioSource> audioSource;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        //  0 -> 1  | 0.000000000001 flotantes 
        //  0 -> 1  | * 100 | 0 -> 100 
        //  0 -> 10 | 0 1 2 3 4 5 6 7 8 9 10
        float initialVolume = PlayerPrefs.GetFloat("volumneMaster", 0.5f);
        volumeSlider.value = initialVolume;
        UpdateVolume(initialVolume);
    }


    public void ChangeVolume(float volume)
    {
        Debug.Log(volume);
        PlayerPrefs.SetFloat("volumneMaster", volume);
        UpdateVolume(volume);
    }

    public void UpdateVolume(float newVolume)
    {
        if (useAudioSource)
        {
            for (int i = 0; i < audioSource.Count; i++)
            {
                audioSource[i].volume = newVolume;
            }
        }
            
        else
        {
            audioMixer.SetFloat("Master", newVolume * 20f);
        }
    }
}

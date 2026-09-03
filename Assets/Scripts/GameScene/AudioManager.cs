using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;
    public enum AudioMixerType { Master,BGM,SFX}
    public AudioMixer audioMixer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void SetAudioVolume(AudioMixerType audioMixerType,float volume)
    {
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20);
    }


    public void SetBGMVolume(Slider slider)
    {
        SetAudioVolume(AudioMixerType.BGM, slider.value);
        GameManager.instance.BGMVolume = slider.value;
    }

    public void SetMasterVolume(Slider slider)
    {
        SetAudioVolume(AudioMixerType.Master, slider.value);
        GameManager.instance.MasterVolume = slider.value;
    }



    public void SetSFXVolume(Slider slider)
    {
        SetAudioVolume(AudioMixerType.SFX, slider.value);
        GameManager.instance.SFXVolume = slider.value;
    }
}

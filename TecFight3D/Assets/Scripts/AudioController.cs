using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer primaryMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        float temp;
        primaryMixer.GetFloat("masterVolume", out temp);
        masterSlider.value = temp;
        primaryMixer.GetFloat("musicVolume", out temp);
        musicSlider.value = temp;
        primaryMixer.GetFloat("sfxVolume", out temp);
        sfxSlider.value = temp;
    }

    public void SetMasterVolume()
    {
        primaryMixer.SetFloat("masterVolume", masterSlider.value);
    }
    public void SetMusicVolume()
    {
        primaryMixer.SetFloat("musicVolume", musicSlider.value);
    }
    public void SetSfxVolume()
    {
        primaryMixer.SetFloat("sfxVolume", sfxSlider.value);
    }
}

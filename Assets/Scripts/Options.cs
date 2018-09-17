using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    [SerializeField]
    private Dropdown resDropdown;
    [SerializeField]
    private Toggle fullscreenToggle;
    [SerializeField]
    private Slider sensSlider;
    [SerializeField]
    private Text sensValue;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider volumeSlider;

    private Resolution[] resolutions;
    private int resIndex;

    private void Start()
    {
        LoadSettings();
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("resolution", index);
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        Screen.fullScreen = isFullscreen;
    }

    public void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("mouseSens", value);
        sensValue.text = value.ToString("n1");
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        audioMixer.SetFloat("volume", volume);
    }

    private void LoadSettings()
    {
        /* Loaded PlayerPrefs:
        resolution
        fullscreen
        mouseSens
        volume */

        SetResolutionsDropdown();
        SetFullscreenToggle();
        SetMouseSensSlider();
        SetVolumeSlider();
    }

    private void SetVolumeSlider()
    {
        float vol = PlayerPrefs.GetFloat("volume");
        volumeSlider.value = vol;
    }

    private void SetFullscreenToggle()
    {
        int isFS = PlayerPrefs.GetInt("fullscreen");
        if (isFS == 1) fullscreenToggle.isOn = true;
        else fullscreenToggle.isOn = false;
    }

    private void SetMouseSensSlider()
    {
        float sens = PlayerPrefs.GetFloat("mouseSens");

        if (sens == 0 ||
            sens > sensSlider.maxValue || 
            sens < sensSlider.minValue)
        {
            sens = 3f;
        }
        sensSlider.value = sens;
        sensValue.text = sens.ToString("n1");
    }

    private void SetResolutionsDropdown()
    {
        List<string> options = new List<string>();

        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        resIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
                        
        }

        resIndex = PlayerPrefs.GetInt("resolution");
        resDropdown.AddOptions(options);
        resDropdown.value = resIndex;
        resDropdown.RefreshShownValue();
    }
}

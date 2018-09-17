using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    [SerializeField]
    private Dropdown resDropdown;
    [SerializeField]
    private Slider sensSlider;
    [SerializeField]
    private Text sensValue;
    [SerializeField]
    private AudioMixer audioMixer;

    private Resolution[] resolutions;
    private int resIndex;

    private void Start()
    {
        SetResolutionsDropdown();
        SetSlider();
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetFloat("resolution", index);
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("mouseSens", value);
        sensValue.text = value.ToString("n1");
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    private void SetSlider()
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

            //if (resolutions[i].width == Screen.currentResolution.width &&
            //    resolutions[i].height == Screen.currentResolution.height)
            //    resIndex = i;

        }
        resDropdown.AddOptions(options);
        resDropdown.value = resIndex;
        resDropdown.RefreshShownValue();
    }
}

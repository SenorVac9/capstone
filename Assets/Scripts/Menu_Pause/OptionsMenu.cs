using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public Dropdown ResolutionsDropdown;
    public Toggle MotionBlurToggle;

    public PostProcessingProfile DemoPostProcess;
    //public MotionBlurComponent MotionBlur;
    //antialiasing
    //low medium high
    //post processing setting 

    Resolution[] resolutions;


    void Start()
    {
        resolutions = Screen.resolutions;
        DemoPostProcess.motionBlur.enabled = true;

        ResolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
              resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionsDropdown.AddOptions(options);
        ResolutionsDropdown.value = currentResolutionIndex;
        ResolutionsDropdown.RefreshShownValue();

    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        AudioMixer.SetFloat("volume", volume); //Must create audio mixer in unity
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetMotionBlur (bool isMotionBlur)
    {
        MotionBlurToggle.enabled = isMotionBlur;
        if (MotionBlurToggle.enabled)
        {
            DemoPostProcess.motionBlur.enabled = true;
            
        }
        else
        {
            DemoPostProcess.motionBlur.enabled = false;

        }
    }
}

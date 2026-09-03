using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;

public class ResolutionManager : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;

    public Toggle ScreenSizeToggle;

    public List<Resolution> resolutions = new List<Resolution>();
    public int ResolutionIndex = 0;

    public GameObject ScreenSettingPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 3840, height = 2160 });

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i <resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (resolutions[i].width == GameManager.instance.ScreenWidthSize && resolutions[i].height == GameManager.instance.ScreenHeightSize)
            {
                ResolutionIndex = i;
            }

            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = ResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        ScreenSizeToggle.isOn = GameManager.instance.isFullScreen;
        SetResolution(ResolutionIndex);
        ScreenSettingPanel.SetActive(false);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
       
        Screen.SetResolution(resolution.width, resolution.height, ScreenSizeToggle.isOn);
        GameManager.instance.ScreenWidthSize = resolution.width;
        GameManager.instance.ScreenHeightSize = resolution.height;
    }    

    public void ChangeScreenMode()
    {
        if(ScreenSizeToggle.isOn == true)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else if(ScreenSizeToggle.isOn == false)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
        GameManager.instance.isFullScreen = ScreenSizeToggle.isOn;
    }

    public void ViewScreenSettingPanel()
    {
        ScreenSettingPanel.SetActive(!ScreenSettingPanel.activeSelf);
    }

    // Update is called once per frame
   
}

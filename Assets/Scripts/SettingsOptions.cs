using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOptions : MonoBehaviour
{
    public TMP_Text mouseSensError;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    public bool isFullscreen = false;
    public Slider fpsSlider;
    public int fpsLimit = 30;
    public TMP_Text fpsValue;
    public TMP_Text fpsIngame;

    public TMP_Text nsfwText;
    public bool isNSFWOn = false;


    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    void Start()
    {
        StartCoroutine(FpsCounter());
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value.ToString() + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        fpsValue.text = fpsLimit.ToString();

    }
    private IEnumerator FpsCounter()
    {
        fpsIngame.text = "FPS: " + Random.Range(fpsLimit - 10, fpsLimit).ToString();
        yield return new WaitForSeconds(1f);
        StartCoroutine(FpsCounter());
    }

    public void MouseError()
    {
        StartCoroutine(ShowMouseError());
    }
    private IEnumerator ShowMouseError()
    {
        mouseSensError.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        mouseSensError.gameObject.SetActive(false);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetFpsLimit()
    {
        fpsLimit = (int)fpsSlider.value;
        Application.targetFrameRate = fpsLimit;
    }

    public void EnableVsync(bool isVsyncOn)
    {
        QualitySettings.vSyncCount = isVsyncOn ? 1 : 0;
    }

    public void EnableNSFW(bool isNSFWOn)
    {
        nsfwText.gameObject.SetActive(isNSFWOn);
    }
    public void TogglePostProcess(GameObject postProcess)
    {
        postProcess.SetActive(!postProcess.activeSelf);
    }
}

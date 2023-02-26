using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Wilberforce;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField] GameObject activePane;
    [SerializeField] GameObject[] infoPanes;
    [SerializeField] Dropdown ColourFilterDropdown;
    [SerializeField] GameObject MainCamera;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMPro.TMP_Text soundLabel;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject pane in infoPanes)
        {
            pane.SetActive(false);
        }

        if (!PlayerPrefs.HasKey(Constants.COLOUR_MODE_PREF_KEY))
        {
            PlayerPrefs.SetInt(Constants.COLOUR_MODE_PREF_KEY, 0);
        }

        if (!PlayerPrefs.HasKey(Constants.VOLUME_PREF_KEY))
        {
            PlayerPrefs.SetFloat(Constants.VOLUME_PREF_KEY, 1);
        }
        
        ColourFilterDropdown.onValueChanged.AddListener(delegate
        {
            switchColorMode();
        });

        volumeSlider.onValueChanged.AddListener(delegate
        {
            changeVolume();
        });

        LoadSettings();
    }

    public void TogglePanes(GameObject tobeActivePane)
    {
        if (activePane != null)
        {
            activePane.SetActive(false);
        }

        tobeActivePane.SetActive(true);
        activePane = tobeActivePane;
    }

    private void switchColorMode()
    {
        PlayerPrefs.SetInt(Constants.COLOUR_MODE_PREF_KEY, ColourFilterDropdown.value);
        MainCamera.GetComponent<Colorblind>().Type = ColourFilterDropdown.value;
    }

    private void changeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        LoadSettings();
    }

    private void LoadSettings()
    {
        ColourFilterDropdown.value = PlayerPrefs.GetInt(Constants.COLOUR_MODE_PREF_KEY);
        PlayerPrefs.SetFloat(Constants.VOLUME_PREF_KEY, volumeSlider.value);
        soundLabel.text = (PlayerPrefs.GetFloat(Constants.VOLUME_PREF_KEY) * 100).ToString("0.00");
    }

}

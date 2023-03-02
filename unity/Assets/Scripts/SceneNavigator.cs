using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wilberforce;

public class SceneNavigator : MonoBehaviour
{
    [SerializeField] public static SceneNavigator Instance;
    private static string previousSceneName;
    [SerializeField] GameObject MainCamera;
    private static bool gamepaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("muliple instances of SceneNavigator");
        }
        Instance = this;
    }


    public void GoToPreviousScene()
    {
        if (previousSceneName != null)
        {
            SceneManager.LoadScene(previousSceneName);
           
        }
    }

    public void GoToScene(string name)
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        if (previousSceneName == "SceneWithHealthbar" && name == "OptionsMenu")
        {
            gamepaused = true;
        }
        else
        {
            gamepaused = false;
        }
        SceneManager.LoadScene(name);
        LoadPlayerPrefs();
    }

    private void LoadPlayerPrefs()
    {
        MainCamera.GetComponent<Colorblind>().Type = PlayerPrefs.GetInt(Constants.COLOUR_MODE_PREF_KEY);
        AudioListener.volume = PlayerPrefs.GetFloat(Constants.VOLUME_PREF_KEY);
        //TODO LOAD Player stats  
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

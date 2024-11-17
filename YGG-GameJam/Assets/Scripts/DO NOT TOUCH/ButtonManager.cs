using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenuPanel;

    [Header("Play Panel")]
    [SerializeField] GameObject playPanel;
    [SerializeField] Button startButton;
    [SerializeField] int currentGameMode;
    [SerializeField] string sceneName;


    void Start()
    {
        startButton.interactable = false;
        PlayerPrefs.SetInt("gameMode", 0);
        
    }
    void Update()
    {
        currentGameMode = PlayerPrefs.GetInt("gameMode");
        if (currentGameMode > 0)
        {
            startButton.interactable = true;
        }
    }
    public void OnClickPlay()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void OnClickStart()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void OnClickGameModeOne()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        sceneName = "GameModeOne";
    }
    public void OnClickGameModeTwo()
    {
        PlayerPrefs.SetInt("gameMode", 2);
        sceneName = "GameModeTwo";
    }
    public void OnClickGameModeThree()
    {
        PlayerPrefs.SetInt("gameMode", 3);
        sceneName = "GameModeThree";
    }
}

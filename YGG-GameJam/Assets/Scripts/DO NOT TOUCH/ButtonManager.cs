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

    public void OnClickStart(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void OnClickGameModeOne()
    {
        PlayerPrefs.SetInt("gameMode", 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class ButtonManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioManager _audioManager;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenuPanel;
    public GameObject loadingScreen;
    public Image loadingBarFill;
    public GameObject gameModeOneButton;

    [Header("Play Panel")]
    [SerializeField] GameObject playPanel;
    [SerializeField] GameObject startButton;
    [SerializeField] int currentGameMode;
    [SerializeField] string sceneName;

    [Header("Help Panel")]
    [SerializeField] GameObject helpPanel;
    [SerializeField] GameObject backHelpButton;
    [SerializeField] GameObject helpButton;
    [Header("About Panel")]
    [SerializeField] GameObject aboutPanel;
    [SerializeField] GameObject aboutPanelOne;
    [SerializeField] GameObject aboutPanelTwo;
    [SerializeField] GameObject backAboutButton;
    [SerializeField] GameObject aboutButton;
    [SerializeField] GameObject aboutNextButton;
    [SerializeField] GameObject aboutPreviousButton;
    [Header("Exit Panel")]
    [SerializeField] GameObject exitPanel;
    [SerializeField] GameObject exitNoButton;
    [SerializeField] GameObject exitButton;

    [Header("Game Mode Pause")]
    public EventSystem eventSystem; 
    public GameObject noButton; 
    public GameObject mainMenuButton; 
    [Header("Game Mode One")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject menuWarning;
    [SerializeField] GameObject menuGameOverWarning;
    public GameObject noGameOverButton;
    public GameObject mainMenuGameOverButton;
    public GameObject[] targetGameObject; // GameObject to activate
    public GameObject[] gameObjectToDetect; // GameObject to check for selection
    public TextMeshProUGUI hoverTxt;
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }
    void Update()
    {
        currentGameMode = PlayerPrefs.GetInt("gameMode");

        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[0]) 
        {
            targetGameObject[0].SetActive(true);
            hoverTxt.text = "-2 PLAYERS\n-FREE FOR ALL";
        }
        else 
        { 
            targetGameObject[0].SetActive(false);
        }
        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[1])
        {
            targetGameObject[1].SetActive(true);
            hoverTxt.text = "-4 PLAYERS\n-DUO KITCHEN ROYALE";
        }
        else
        {
            targetGameObject[1].SetActive(false);
        }
        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[2])
        {
            targetGameObject[2].SetActive(true);
            hoverTxt.text = "-4 PLAYERS\n-FREE FOR ALL";
        }
        else
        {
            targetGameObject[2].SetActive(false);
        }
    }

    // MAIN MENU ==============================================
    public void OnClickStart()
    {
        eventSystem.SetSelectedGameObject(gameModeOneButton);
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);
        _audioManager.PlayButtonClickSound();
    }

    public void OnClickGameModeOne()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        LoadScene();
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickGameModeTwo()
    {
        PlayerPrefs.SetInt("gameMode", 2);
        LoadScene();
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickGameModeThree()
    {
        PlayerPrefs.SetInt("gameMode", 3);
        LoadScene();
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickBack() 
    {
        eventSystem.SetSelectedGameObject(startButton);
        mainMenuPanel.SetActive(true);
        playPanel.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }

    public void OnClickHelp() 
    {
        eventSystem.SetSelectedGameObject(backHelpButton);
        mainMenuPanel.SetActive(false);
        helpPanel.SetActive(true);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickHelpBack() 
    {
        eventSystem.SetSelectedGameObject(helpButton);
        mainMenuPanel.SetActive(true);
        helpPanel.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickAbout() 
    {
        eventSystem.SetSelectedGameObject(backAboutButton);
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(true);
        aboutPanelOne.SetActive(true);
        aboutPanelTwo.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickAboutBack()
    {
        eventSystem.SetSelectedGameObject(aboutButton);
        mainMenuPanel.SetActive(true);
        aboutPanel.SetActive(false);
        aboutPanelOne.SetActive(false);
        aboutPanelTwo.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickAboutNext()
    {
        eventSystem.SetSelectedGameObject(aboutPreviousButton);
        aboutPanelOne.SetActive(false);
        aboutPanelTwo.SetActive(true);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickAboutPrevious()
    {
        eventSystem.SetSelectedGameObject(backAboutButton);
        aboutPanelOne.SetActive(true);
        aboutPanelTwo.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickExit() 
    {
        eventSystem.SetSelectedGameObject(exitNoButton);
        exitPanel.SetActive(true);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickNoExit()
    {
        eventSystem.SetSelectedGameObject(exitButton);
        exitPanel.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickYesExit()
    {
        Application.Quit();
        _audioManager.PlayButtonClickSound();
    }

    public void LoadScene()
    {
        int sceneId = PlayerPrefs.GetInt("gameMode") + 1;
        StartCoroutine(LoadSceneAsync(sceneId));
        _audioManager.PlayButtonClickSound();
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

    // GAME MODE===================================
    public void OnClickPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        _audioManager.PlayButtonClickSound();
        _audioManager.musicSource.Pause();
        _audioManager.upSfxSource.Pause();
        _audioManager.apSfxSource.Pause();
    }
    public void OnClickResume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        _audioManager.PlayButtonClickSound();
        _audioManager.musicSource.UnPause();
        _audioManager.upSfxSource.UnPause();
        _audioManager.apSfxSource.UnPause();
    }
    public void OnClickMenu()
    {
        eventSystem.SetSelectedGameObject(noButton);
        menuWarning.SetActive(true);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickYes()
    {
        int sceneId = 1;
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsync(sceneId));
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickNo()
    {
        eventSystem.SetSelectedGameObject(mainMenuButton);
        menuWarning.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }

    // GAME OVER
    public void OnClickGameOverMenu()
    {
        menuGameOverWarning.SetActive(true);
        eventSystem.SetSelectedGameObject(noGameOverButton);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickGameOverNo()
    {
        eventSystem.SetSelectedGameObject(mainMenuGameOverButton);
        menuGameOverWarning.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickGameOverYes()
    {
        int sceneId = 1;
        StartCoroutine(LoadSceneAsync(sceneId));
        _audioManager.PlayButtonClickSound();
    }

}

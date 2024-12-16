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
    public AudioSource buttonAudioSource;
    public AudioClip buttonAudioClip;
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
        buttonAudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        currentGameMode = PlayerPrefs.GetInt("gameMode");

        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[0]) 
        {
            targetGameObject[0].SetActive(true);
            hoverTxt.text = "-2 PLAYERS\n-FREE FOR ALL";
            PlayButtonClickSound();
        }
        else 
        { 
            targetGameObject[0].SetActive(false);
        }
        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[1])
        {
            targetGameObject[1].SetActive(true);
            hoverTxt.text = "-4 PLAYERS\n-DUO KITCHEN ROYALE";
            PlayButtonClickSound();
        }
        else
        {
            targetGameObject[1].SetActive(false);
        }
        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[2])
        {
            targetGameObject[2].SetActive(true);
            hoverTxt.text = "-4 PLAYERS\n-FREE FOR ALL";
            PlayButtonClickSound();
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
        PlayButtonClickSound();
    }

    public void OnClickGameModeOne()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        LoadScene();
        PlayButtonClickSound();
    }
    public void OnClickGameModeTwo()
    {
        PlayerPrefs.SetInt("gameMode", 2);
        LoadScene();
        PlayButtonClickSound();
    }
    public void OnClickGameModeThree()
    {
        PlayerPrefs.SetInt("gameMode", 3);
        LoadScene();
        PlayButtonClickSound();
    }
    public void OnClickBack() 
    {
        eventSystem.SetSelectedGameObject(startButton);
        mainMenuPanel.SetActive(true);
        playPanel.SetActive(false);
        PlayButtonClickSound();
    }

    public void OnClickHelp() 
    {
        eventSystem.SetSelectedGameObject(backHelpButton);
        mainMenuPanel.SetActive(false);
        helpPanel.SetActive(true);
        PlayButtonClickSound();
    }
    public void OnClickHelpBack() 
    {
        eventSystem.SetSelectedGameObject(helpButton);
        mainMenuPanel.SetActive(true);
        helpPanel.SetActive(false);
        PlayButtonClickSound();
    }
    public void OnClickExit() 
    {
        eventSystem.SetSelectedGameObject(exitNoButton);
        exitPanel.SetActive(true);
        PlayButtonClickSound();
    }
    public void OnClickNoExit()
    {
        eventSystem.SetSelectedGameObject(exitButton);
        exitPanel.SetActive(false);
        PlayButtonClickSound();
    }
    public void OnClickYesExit()
    {
        Application.Quit();
        PlayButtonClickSound();
    }

    public void LoadScene()
    {
        int sceneId = PlayerPrefs.GetInt("gameMode") + 1;
        StartCoroutine(LoadSceneAsync(sceneId));
        PlayButtonClickSound();
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
        PlayButtonClickSound();
    }
    public void OnClickResume()
    {
        pausePanel.SetActive(false);
        GameManager.instance.gameMusic.Play();
        Time.timeScale = 1;
        PlayButtonClickSound();
    }
    public void OnClickMenu()
    {
        eventSystem.SetSelectedGameObject(noButton);
        menuWarning.SetActive(true);
        PlayButtonClickSound();
    }
    public void OnClickYes()
    {
        int sceneId = 1;
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsync(sceneId));
        PlayButtonClickSound();
    }
    public void OnClickNo()
    {
        eventSystem.SetSelectedGameObject(mainMenuButton);
        menuWarning.SetActive(false);
        PlayButtonClickSound();
    }

    // GAME OVER
    public void OnClickGameOverMenu()
    {
        menuGameOverWarning.SetActive(true);
        eventSystem.SetSelectedGameObject(noGameOverButton);
        PlayButtonClickSound();
    }
    public void OnClickGameOverNo()
    {
        eventSystem.SetSelectedGameObject(mainMenuGameOverButton);
        menuGameOverWarning.SetActive(false);
        PlayButtonClickSound();
    }
    public void OnClickGameOverYes()
    {
        int sceneId = 1;
        StartCoroutine(LoadSceneAsync(sceneId));
        PlayButtonClickSound();
    }

    public void PlayButtonClickSound()
    {
        float randomPitch = Random.Range(0.7f, 1);
        buttonAudioSource.pitch = randomPitch;
        buttonAudioSource.clip = buttonAudioClip;
        buttonAudioSource.Play();
    }
}

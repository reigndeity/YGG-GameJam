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
    [SerializeField] GameObject helpOnePanel;
    [SerializeField] GameObject backHelpOneButton;
    [SerializeField] GameObject nextHelpOneButton;
    [SerializeField] GameObject HelpTwoPanel;
    [SerializeField] GameObject backHelpTwoButton;
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
    public GameObject[] gameObjectToDetect; // GameObject to check for selection
    public GameObject[] mapPlacement; //0 = left, 1 = middle, 2 = right
    public TextMeshProUGUI hoverTxt;
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        Cursor.visible = false;
    }
    void Update()
    {
        currentGameMode = PlayerPrefs.GetInt("gameMode");

        MapSelection();               
    }

    // MAP SELECTION =========================================
    void MapSelection()
    {
        if (gameObjectToDetect != null)
        {
            RectTransform mapOne = gameObjectToDetect[0].GetComponent<RectTransform>();
            RectTransform mapTwo = gameObjectToDetect[1].GetComponent<RectTransform>();
            RectTransform mapThree = gameObjectToDetect[2].GetComponent<RectTransform>();

            RectTransform placementOne = mapPlacement[0].GetComponent<RectTransform>();
            RectTransform placementTwo = mapPlacement[1].GetComponent<RectTransform>();
            RectTransform placementThree = mapPlacement[2].GetComponent<RectTransform>();

            if (eventSystem.currentSelectedGameObject == gameObjectToDetect[0])
            {
                mapOne.anchoredPosition = Vector2.Lerp(mapOne.anchoredPosition, placementTwo.anchoredPosition, Time.deltaTime * 10);
                mapTwo.anchoredPosition = Vector2.Lerp(mapTwo.anchoredPosition, placementThree.anchoredPosition, Time.deltaTime * 10);
                mapThree.anchoredPosition = Vector2.Lerp(mapThree.anchoredPosition, placementOne.anchoredPosition, Time.deltaTime * 10);

                mapOne.sizeDelta = Vector2.Lerp(mapOne.rect.size, placementTwo.rect.size, Time.deltaTime * 10);
                mapTwo.sizeDelta = Vector2.Lerp(mapTwo.rect.size, placementThree.rect.size, Time.deltaTime * 10);
                mapThree.sizeDelta = Vector2.Lerp(mapThree.rect.size, placementOne.rect.size, Time.deltaTime * 10);

                hoverTxt.text = "-2 PLAYERS\n-FREE FOR ALL";
            }

            if (eventSystem.currentSelectedGameObject == gameObjectToDetect[1])
            {
                mapTwo.anchoredPosition = Vector2.Lerp(mapTwo.anchoredPosition, placementTwo.anchoredPosition, Time.deltaTime * 10);
                mapThree.anchoredPosition = Vector2.Lerp(mapThree.anchoredPosition, placementThree.anchoredPosition, Time.deltaTime * 10);
                mapOne.anchoredPosition = Vector2.Lerp(mapOne.anchoredPosition, placementOne.anchoredPosition, Time.deltaTime * 10);

                mapTwo.sizeDelta = Vector2.Lerp(mapTwo.rect.size, placementTwo.rect.size, Time.deltaTime * 10);
                mapThree.sizeDelta = Vector2.Lerp(mapThree.rect.size, placementThree.rect.size, Time.deltaTime * 10);
                mapOne.sizeDelta = Vector2.Lerp(mapOne.rect.size, placementOne.rect.size, Time.deltaTime * 10);

                hoverTxt.text = "-4 PLAYERS\n-DUO KITCHEN ROYALE";
            }

            if (eventSystem.currentSelectedGameObject == gameObjectToDetect[2])
            {
                mapThree.anchoredPosition = Vector2.Lerp(mapThree.anchoredPosition, placementTwo.anchoredPosition, Time.deltaTime * 10);
                mapOne.anchoredPosition = Vector2.Lerp(mapOne.anchoredPosition, placementThree.anchoredPosition, Time.deltaTime * 10);
                mapTwo.anchoredPosition = Vector2.Lerp(mapTwo.anchoredPosition, placementOne.anchoredPosition, Time.deltaTime * 10);

                mapThree.sizeDelta = Vector2.Lerp(mapThree.rect.size, placementTwo.rect.size, Time.deltaTime * 10);
                mapOne.sizeDelta = Vector2.Lerp(mapOne.rect.size, placementThree.rect.size, Time.deltaTime * 10);
                mapTwo.sizeDelta = Vector2.Lerp(mapTwo.rect.size, placementOne.rect.size, Time.deltaTime * 10);

                hoverTxt.text = "-4 PLAYERS\n-FREE FOR ALL";
            }
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
        eventSystem.SetSelectedGameObject(backHelpOneButton);
        mainMenuPanel.SetActive(false);
        helpPanel.SetActive(true);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickHelpBack() 
    {
        eventSystem.SetSelectedGameObject(helpButton);
        mainMenuPanel.SetActive(true);
        helpOnePanel.SetActive(false);
        helpPanel.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickHelpNext()
    {
        eventSystem.SetSelectedGameObject(backHelpTwoButton);
        helpOnePanel.SetActive(false);
        HelpTwoPanel.SetActive(true);
        nextHelpOneButton.SetActive(false);
        _audioManager.PlayButtonClickSound();
    }
    public void OnClickHelpBackTwo()
    {
        eventSystem.SetSelectedGameObject(backHelpOneButton);
        helpOnePanel.SetActive(true);
        HelpTwoPanel.SetActive(false);
        nextHelpOneButton.SetActive(true);
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
        //menuGameOverWarning.SetActive(true);
        eventSystem.SetSelectedGameObject(noGameOverButton);
        _audioManager.PlayButtonClickSound();
        int sceneId = 1;
        StartCoroutine(LoadSceneAsync(sceneId));
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

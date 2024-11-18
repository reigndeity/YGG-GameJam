using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class ButtonManager : MonoBehaviour
{
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

    void Update()
    {
        currentGameMode = PlayerPrefs.GetInt("gameMode");

        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[0]) 
        {
            targetGameObject[0].SetActive(true);
            hoverTxt.text = "-2 PLAYERS\n-1vs1 STYLE";
        }
        else 
        { 
            targetGameObject[0].SetActive(false);
        }
        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[1])
        {
            targetGameObject[1].SetActive(true);
            hoverTxt.text = "-4 PLAYERS\n-TEAM STYLE";
        }
        else
        {
            targetGameObject[1].SetActive(false);
        }
        if (eventSystem.currentSelectedGameObject == gameObjectToDetect[2])
        {
            targetGameObject[2].SetActive(true);
            hoverTxt.text = "-4 PLAYERS\n-FREE FOR ALL STYLE";
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
    }

    public void OnClickGameModeOne()
    {
        PlayerPrefs.SetInt("gameMode", 1);
        LoadScene();
    }
    public void OnClickGameModeTwo()
    {
        PlayerPrefs.SetInt("gameMode", 2);
        LoadScene();
    }
    public void OnClickGameModeThree()
    {
        PlayerPrefs.SetInt("gameMode", 3);
        LoadScene();
    }
    public void OnClickBack() 
    {
        eventSystem.SetSelectedGameObject(startButton);
        mainMenuPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void OnClickHelp() 
    {
        eventSystem.SetSelectedGameObject(backHelpButton);
        helpPanel.SetActive(true);
    }
    public void OnClickHelpBack() 
    {
        eventSystem.SetSelectedGameObject(helpButton);
        helpPanel.SetActive(false);
    }
    public void OnClickExit() 
    {
        eventSystem.SetSelectedGameObject(exitNoButton);
        exitPanel.SetActive(true);
    }
    public void OnClickNoExit()
    {
        eventSystem.SetSelectedGameObject(exitButton);
        exitPanel.SetActive(false);
    }
    public void OnClickYesExit()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        int sceneId = PlayerPrefs.GetInt("gameMode");
        StartCoroutine(LoadSceneAsync(sceneId));
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
    }
    public void OnClickResume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnClickMenu()
    {
        eventSystem.SetSelectedGameObject(noButton);
        menuWarning.SetActive(true);
    }
    public void OnClickYes()
    {
        int sceneId = 0;
        Time.timeScale = 1;
        StartCoroutine(LoadSceneAsync(sceneId));
    }
    public void OnClickNo()
    {
        eventSystem.SetSelectedGameObject(mainMenuButton);
        menuWarning.SetActive(false);
    }

    // GAME OVER
    public void OnClickGameOverMenu()
    {
        menuGameOverWarning.SetActive(true);
        eventSystem.SetSelectedGameObject(noGameOverButton);
    }
    public void OnClickGameOverNo()
    {
        eventSystem.SetSelectedGameObject(mainMenuGameOverButton);
        menuGameOverWarning.SetActive(false);
    }
    public void OnClickGameOverYes()
    {
        int sceneId = 0;
        StartCoroutine(LoadSceneAsync(sceneId));
    }


}

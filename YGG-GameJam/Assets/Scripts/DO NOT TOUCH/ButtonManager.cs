using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class ButtonManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenuPanel;
    public GameObject loadingScreen;
    public Image loadingBarFill;
    public GameObject gameModeOneButton;

    [Header("Play Panel")]
    [SerializeField] GameObject playPanel;
    [SerializeField] Button startButton;
    [SerializeField] int currentGameMode;
    [SerializeField] string sceneName;

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

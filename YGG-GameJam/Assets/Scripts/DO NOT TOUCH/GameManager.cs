using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Game Properties")]
    public bool gameStart;
    public bool canMove;
    [SerializeField] GameObject[] gamepadCharacters;
    [SerializeField] GameObject[] keyboardCharacters;
    public int gameModeType;
    public int recipeChosen;
    [Header("---------------------------------")]
    public GameObject recipePanel;
    public GameObject[] recipeImg;
    [SerializeField] Animator _spotlightAnimator;
    [SerializeField] Animator _recipePanelAnimator;
    [SerializeField] TextMeshProUGUI countdownTxt;
    [Header("---------------------------------")]
    public float timeRemaining;
    public TextMeshProUGUI timerTxt;
    [Header("---------------------------------")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI winnerTxt;
    [SerializeField] GameObject[] soloWinners;
    [SerializeField] GameObject[] teamWinners;
    [SerializeField] GameObject drawWinners;
    [SerializeField] TextMeshProUGUI winnerHeaderTxt;

    [Header("Player Score Properties")]
    public int playerOneScore;
    public int playerTwoScore;
    public int playerThreeScore;
    public int playerFourScore;
    [SerializeField] TextMeshProUGUI playerOneScoreTxt;
    [SerializeField] TextMeshProUGUI playerTwoScoreTxt;
    [SerializeField] TextMeshProUGUI playerThreeScoreTxt;
    [SerializeField] TextMeshProUGUI playerFourScoreTxt;

    [Header("Script Refernces")]
    [SerializeField] ButtonManager _buttonManager;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] GameObject mainMenuButton;

    void Awake()
    {
        if (instance != null && instance!= this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        gameModeType = PlayerPrefs.GetInt("gameMode");
        gameStart = false;
        countdownTxt.enabled = false;
        StartCoroutine(ShowRecipe());

        _buttonManager = FindObjectOfType<ButtonManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            _buttonManager.OnClickPause();
        }
        if (gameStart == true)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                GameOver();
                timeRemaining = 0;
            }
        }
        // Format the time as minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // Calculate minutes
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // Calculate seconds

        // Display the time as "mm:ss"
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        switch (gameModeType)
        {
            case 1:
                playerOneScoreTxt.text = playerOneScore.ToString("F0");
                playerTwoScoreTxt.text = playerTwoScore.ToString("F0");
                break;
            case 2:
                playerOneScoreTxt.text = playerOneScore.ToString("F0");
                playerTwoScoreTxt.text = playerTwoScore.ToString("F0");
                break;
            case 3:
                playerOneScoreTxt.text = playerOneScore.ToString("F0");
                playerTwoScoreTxt.text = playerTwoScore.ToString("F0");
                playerThreeScoreTxt.text = playerThreeScore.ToString("F0");
                playerFourScoreTxt.text = playerFourScore.ToString("F0");
                break;
        }

    }



    // TEST
   IEnumerator ShowRecipe()
    {
        recipeChosen = Random.Range(0, 3);
        recipePanel.SetActive(true);

        // Deactivate all recipe images initially
        foreach (GameObject img in recipeImg)
        {
            img.SetActive(false);
        }

        // Activate the chosen recipe image temporarily
        recipeImg[recipeChosen].SetActive(true);

        // Start the blinking effect for all images
        yield return StartCoroutine(BlinkObjects(recipeImg, 5f, 0.1f, 0.5f));

        // After blinking, deactivate all images except the chosen one
        for (int i = 0; i < recipeImg.Length; i++)
        {
            if (i == recipeChosen)
            {
                recipeImg[i].SetActive(true);
            }
            else
            {
                recipeImg[i].SetActive(false);
            }
        }

        // Hide the recipe panel after showing the chosen image
        StartCoroutine(BeforeRoundStart());

    }
        IEnumerator BeforeRoundStart()
    {
        yield return new WaitForSeconds(1);
        _spotlightAnimator.SetTrigger("isRecipeChosen");
        yield return new WaitForSeconds(3);
        _recipePanelAnimator.SetTrigger("deactivateRecipe");
        yield return new WaitForSeconds(1.0f);
        recipePanel.SetActive(false);
        countdownTxt.enabled = true;
        countdownTxt.text = "3";
        yield return new WaitForSeconds(1f);
        countdownTxt.text = "2";
        yield return new WaitForSeconds(1f);
        countdownTxt.text = "1";
        yield return new WaitForSeconds(1f);
        countdownTxt.text = "GO!!!";
        gameStart = true;
        yield return new WaitForSeconds(1f);
        countdownTxt.enabled = false;
    }

    IEnumerator BlinkObjects(GameObject[] objects, float totalBlinkDuration, float minBlinkInterval, float maxBlinkInterval)
        {
        float elapsedTime = 0f;
        float currentBlinkInterval = minBlinkInterval;
        int currentIndex = 0;

        while (elapsedTime < totalBlinkDuration)
        {
            // Deactivate all objects
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }

            // Activate the current object
            objects[currentIndex].SetActive(true);

            // Wait for the current interval before moving to the next object
            yield return new WaitForSeconds(currentBlinkInterval);

            // Move to the next object in the array
            currentIndex = (currentIndex + 1) % objects.Length;

            // Gradually increase the interval to slow down the blinking
            elapsedTime += currentBlinkInterval;
            currentBlinkInterval = Mathf.Lerp(minBlinkInterval, maxBlinkInterval, elapsedTime / totalBlinkDuration);
        }

        // Ensure all objects are set to inactive except the chosen one at the end
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
        objects[currentIndex].SetActive(true); // Keep the last active object visible
    }

    public void CompareScores()
    {
        int highestScore = Mathf.Max(playerOneScore, playerTwoScore, playerThreeScore, playerFourScore);
        switch (gameModeType)
        {
            case 1:
                if (highestScore == playerOneScore)
                {
                    soloWinners[0].SetActive(true);
                    winnerTxt.text = "PLAYER 1";
                }
                if (highestScore == playerTwoScore)
                {
                    soloWinners[1].SetActive(true);
                    winnerTxt.text = "PLAYER 2";
                }
                break;
            case 2:
                if (highestScore == playerOneScore)
                {
                    teamWinners[0].SetActive(true);
                    winnerTxt.text = "TEAM 1";
                }
                if (highestScore == playerTwoScore)
                {
                    teamWinners[1].SetActive(true);
                    winnerTxt.text = "TEAM 2";
                }
                break;
            case 3:
                if (highestScore == playerOneScore)
                {
                    soloWinners[0].SetActive(true);
                    winnerTxt.text = "PLAYER 1";
                }
                if (highestScore == playerTwoScore)
                {
                    soloWinners[1].SetActive(true);
                    winnerTxt.text = "PLAYER 2";
                }
                if (highestScore == playerThreeScore)
                {
                    soloWinners[2].SetActive(true);
                    winnerTxt.text = "PLAYER 3";
                }
                if (highestScore == playerFourScore)
                {
                    soloWinners[3].SetActive(true);
                    winnerTxt.text = "PLAYER 4";
                }
                break;
        }

        // Check if there is a tie between players with the highest score
        int tieCount = 0;
        if (playerOneScore == highestScore) tieCount++;
        if (playerTwoScore == highestScore) tieCount++;
        if (playerThreeScore == highestScore) tieCount++;
        if (playerFourScore == highestScore) tieCount++;

        if (tieCount > 1)
        {
            drawWinners.SetActive(true);
            winnerHeaderTxt.text = "● TIE! ●";
            winnerTxt.text = "NOBODY WON...";
        }
    }

    public void GameOver()
    {
        gameStart = false;
        CompareScores();
        gameOverPanel.SetActive(true);
        _eventSystem.SetSelectedGameObject(mainMenuButton);
    }
    
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [Header("---------------------------------")]
    public float timeRemaining;
    public TextMeshProUGUI timerTxt;
    [Header("---------------------------------")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI winnerTxt;


    [Header("Player Score Properties")]
    public int playerOneScore;
    public int playerTwoScore;
    public int playerThreeScore;
    public int playerFourScore;
    [SerializeField] TextMeshProUGUI playerOneScoreTxt;
    [SerializeField] TextMeshProUGUI playerTwoScoreTxt;
    [SerializeField] TextMeshProUGUI playerThreeScoreTxt;
    [SerializeField] TextMeshProUGUI playerFourScoreTxt;

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
        StartCoroutine(ShowRecipe());
    }
    void Update()
    {
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

    IEnumerator GameBegins()
    {
        recipeChosen = Random.Range (0,3);
        recipePanel.SetActive(true);
        switch(recipeChosen)
        {
            case 0:
                recipeImg[0].SetActive(true);
                break;
            case 1:
                recipeImg[1].SetActive(true);
                break;
            case 2:
                recipeImg[2].SetActive(true);
                break;
        }
        yield return new WaitForSeconds(3);
        recipePanel.SetActive(false);
        gameStart = true;
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
        yield return new WaitForSeconds(3);
        recipePanel.SetActive(false);
        gameStart = true;
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
                    winnerTxt.text = "P1 One Wins";
                }
                if (highestScore == playerTwoScore)
                {
                    winnerTxt.text = "P2 Two Wins";
                }
                break;
            case 2:
                if (highestScore == playerOneScore)
                {
                    winnerTxt.text = "Team One Wins";
                }
                if (highestScore == playerTwoScore)
                {
                    winnerTxt.text = "Team Two Wins";
                }
                break;
            case 3:
                if (highestScore == playerOneScore)
                {
                    winnerTxt.text = "P1 Wins";
                }
                if (highestScore == playerTwoScore)
                {
                    winnerTxt.text = "P2 Wins";
                }
                if (highestScore == playerThreeScore)
                {
                    winnerTxt.text = "P3 Wins";
                }
                if (highestScore == playerFourScore)
                {
                    winnerTxt.text = "P4 Wins";
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
            winnerTxt.text = "There is a Tie!";
        }
    }

    public void GameOver()
    {
        gameStart = false;
        CompareScores();
        gameOverPanel.SetActive(true);
    }
    
}


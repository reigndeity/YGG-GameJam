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
        StartCoroutine(GameBegins());
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
        
        switch (gameModeType)
        {
            case 1:
                timerTxt.text = timeRemaining.ToString("F0");
                playerOneScoreTxt.text = "P1 Score: " + playerOneScore.ToString("F0");
                playerTwoScoreTxt.text = "P2 Score: " + playerTwoScore.ToString("F0");
                break;
            case 2:
                timerTxt.text = timeRemaining.ToString("F0");
                playerOneScoreTxt.text = "Team One Score: " + playerOneScore.ToString("F0");
                playerTwoScoreTxt.text = "Team Two Score: " + playerTwoScore.ToString("F0");
                break;
            case 3:
                timerTxt.text = timeRemaining.ToString("F0");
                playerOneScoreTxt.text = "P1 Score: " + playerOneScore.ToString("F0");
                playerTwoScoreTxt.text = "P2 Score: " + playerTwoScore.ToString("F0");
                playerThreeScoreTxt.text = "P3 Score: " + playerThreeScore.ToString("F0");
                playerFourScoreTxt.text = "P4 Score: " + playerFourScore.ToString("F0");
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

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
        timerTxt.text = timeRemaining.ToString("F0");
        playerOneScoreTxt.text = "Team One: " + playerOneScore.ToString("F0");
        playerTwoScoreTxt.text = "Team Two: " + playerTwoScore.ToString("F0");
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
        if (highestScore == playerOneScore)
        {
            winnerTxt.text = "Team One Wins";
        }
        if (highestScore == playerTwoScore)
        {
            winnerTxt.text = "Team Two Wins";
        }
        if (highestScore == playerThreeScore)
        {
        }
        if (highestScore == playerFourScore)
        {
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

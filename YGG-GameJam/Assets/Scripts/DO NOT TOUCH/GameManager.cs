using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Game Properties")]
    public bool canMove;
    [SerializeField] GameObject[] gamepadCharacters;
    [SerializeField] GameObject[] keyboardCharacters;

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
}
